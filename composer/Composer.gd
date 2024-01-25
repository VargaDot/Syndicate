## This Singleton class manages loading and removing scenes. It allows for dynamic loading screens and transfers of variables between scenes without the need for any additional Singletons.
extends Node

signal loading_began(scene_name: String)
## This signal keeps updating you with the progress of loading the scene. Please note that the percentage is done by Godot itself and is known to not be very accurate.[br]
## Issue on GitHub: https://github.com/godotengine/godot/issues/56882
signal loading_value_updated(value: float)

## This signal is called when it is safe to load the next scene. Code returns the status of the previous loading. Code 0 means everything was successful.
signal loading_done(scene_name: String, code)

## Use this signal inside of scenes to activate certain stuff like show CanvasLayer and other stuff that might not be affected by simply enabling process mode.
signal scene_activated(scene_name: String)
## Use this signal inside of scenes to disable certain stuff like hide CanvasLayer and other stuff that might not be affected by simply disabling process mode.
signal scene_disabled(scene_name: String)

signal _loading_continue()

signal loading_screen_activated(screen_name: String)
signal loading_screen_disabled(screen_name: String)

var is_safe_to_continue = false
var is_safe_to_finish = false

@onready var _root = get_tree().current_scene
@onready var _default_parent_node = _root

## This is the base path for loading screens. You can change it with change_default_loading_screen_path method or use load settings.
var loading_screens_default_path = "CanvasLayer/"

## This dictionary contains shortcuts for scene paths. You can use the shortcut key with add_scene_from_string instead of typing the whole path.
var scene_paths: Dictionary = {
	"MainMenu":"res://main_menu/MainMenu.tscn",
	"Game":"res://game/Game.tscn",
}

## This dictionary contains settings presets. You can use the shorcut key as parameter for args instead of typing the settings manually.
var settings_presets: Dictionary = {
	"preset1":{"loading_screen":"LoadingScreen"}
}

## This dictionary contains all loaded scenes and their process states.
var current_scenes: Dictionary = {}

var is_debug_enabled: bool = false

var _variables = {}

var _loading_screen_variables = {}

var _internal_scene_paths: Dictionary = {}

var _subthreads_enabled = false
var _cache_mode = 1

var _is_loading: bool = false
var _loading_path: String = ""
var _loading_scene: PackedScene
var _default_loading_screen: Control = null

var _last_settings = {}
var _last_vars = {}

enum INTERNAL_LOADING_VALS {
	FAILED = 0,
	SUCCESS = 1
}

var _internal_loading_val: INTERNAL_LOADING_VALS = INTERNAL_LOADING_VALS.SUCCESS

func _ready():
	set_process(false)

## Enables or disables additional print messages
func set_debug(value: bool = true):
	is_debug_enabled = value

## Sets the default path in which to look for loading screens.
func change_default_loading_screen_path(path: String):
	if not path.ends_with("/"):
		path += "/"

	loading_screens_default_path = path

## Returns all existing variables for loading screen in Composer's memory
func get_variables_for_loading_screen() -> Dictionary:
	return _loading_screen_variables

## Returns all existing variables in Composer's memory
func get_variables() -> Dictionary:
	return _variables

## Returns a specific variable located in Composer's memory or null if it doesn't exist.
func get_variable(variable) -> Variant:
	if _variables.has(variable):
		return _variables[variable]

	push_error(variable, " variable does not exist!")
	return null

## Updates the value for an existing variable. This can also be used to add variables to Composer between loading in new scenes.
func update_variable(variable: String, value):
	_variables[variable] = value

## Removes a specific variable from Composer's memory
func remove_variable(variable: String):
	_variables.erase(variable)

## Removes all variables from Composer's memory
func clear_variables():
	_variables.clear()

## Adds a shortcut for scene's path
func add_scene_path(scene: String,path: String):
	scene_paths[scene] = path

## Adds a shortcut for scene's path
func add_settings_preset(key: String,settings: Dictionary):
	settings_presets[key] = settings


## Returns the state of given scene. If a scene doesn't exist, it will return false.
func get_scene(scene: String) -> bool:
	if current_scenes.has(scene):
		return current_scenes[scene]

	push_error(scene, " could not be found!")
	return false

## Returns all existing scenes
func get_scenes() -> Dictionary:
	return current_scenes

## Removes all existing scenes
func clear_all_scenes():
	for scene in current_scenes.keys():
		remove_scene(scene)

	current_scenes.clear()

## Removes the given scene
func remove_scene(scene: String):
	if current_scenes.has(scene):
		_last_settings.erase(scene)
		_last_vars.erase(scene)

		var _scene = _root.get_node(scene)
		_scene.queue_free()
		current_scenes.erase(scene)
	else:
		push_error(scene," could not be found!")

## Removes the current active scene. [b]Note that for this to work, there can only be one scene active.[/b]
func remove_current_scene():
	if _is_one_scene_active():
		var _key = current_scenes.find_key(true)
		remove_scene(_key)
		return

	push_error("Couldn't remove current scene. This can happen when there are multiple active scenes or none. Try using remove_scene.")

## Reloads the current active scene. [b]Note that for this to work, there can only be one scene active.[/b][br]
## [b]For valid settings, refer to reload_scene documentation.[/b]
func reload_current_scene(args = {}, vars = {}):
	if _is_one_scene_active():
		var _key = current_scenes.find_key(true)

		reload_scene(_key,args,vars)

	push_error("Couldn't reload current scene. This can happen when there are multiple active scenes or none. Try using reload_scene.")

## Reloads the given scene. It can also a dictionary containing settings and a dictionary for the variables.[br]
## Unique settings for this function are:[br]
## - [b]use_previous_settings[/b] - this will use last settings used when loading this scene and ignore everything else in[br]
## - [b]use_previous_variables[/b] - this will use last variables used when loading this scene[br]
## - [b]no_settings_override[/b] - this will not override last saved settings for this scene with the new ones[br]
## - [b]no_variables_override[/b] - this will not override last saved variables for this scene with the new ones[br]
## [br]
## [b]All settings that work for add_scene will also work here.[/b]
func reload_scene(scene: String,args = {},vars = {}):
	if current_scenes.has(scene):
		if not _internal_scene_paths.has(scene):
			push_error("Could not find scene ", scene)
			return

		await remove_scene(scene)

		var _args_to_use = {}
		var _vars_to_use = {}

		if typeof(args) == TYPE_STRING:
			_args_to_use = settings_presets.get(args,{})

		if args.get("use_previous_settings",false):
			_args_to_use = _last_settings[scene]
		else:
			_args_to_use = args

		if args.get("use_previous_variables",false):
			_vars_to_use = _last_vars[scene]
		else:
			_vars_to_use = vars

		if args.get("no_settings_override",false) == false:
			_last_settings[scene] = _args_to_use

		if args.get("no_variables_override",false) == false:
			_last_vars[scene] = _last_vars

		if typeof(_internal_scene_paths[scene]) == TYPE_STRING:
			add_scene_from_string(_internal_scene_paths[scene],_args_to_use,_vars_to_use)
		else:
			add_scene_from_packed(_internal_scene_paths[scene],_args_to_use,_vars_to_use)
	else:
		push_error("Could not find scene ", scene)

## Disables every scene.
func disable_all_scenes():
	for scene in current_scenes.keys():
		disable_scene(scene)

## Disabled the current active scene. [b]Note that for this to work, there can only be one scene active.[/b]
func disable_current_scene():
	if _is_one_scene_active():
		var _key = current_scenes.find_key(true)
		disable_scene(_key)
		return

	push_error("Couldn't disable current scene. This can happen when there are multiple active scenes or none. Try using remove_scene.")

## Enables the given scene (shows it and turns on processing)
func activate_scene(scene: String):
	if current_scenes.has(scene):
		var _scene = _root.get_node(scene)
		_scene.show()
		_scene.set("process_mode",PROCESS_MODE_INHERIT)
		current_scenes[scene] = true
		scene_activated.emit(_scene.name)
	else:
		push_error("Could not find scene ", scene)

## Disables the given scene (hides it and turns off processing)
func disable_scene(scene: String):
	if current_scenes.has(scene):
		var _scene = _root.get_node(scene)
		scene_disabled.emit(_scene.name)
		_scene.hide()
		_scene.set("process_mode",PROCESS_MODE_DISABLED)
		current_scenes[scene] = false
	else:
		push_error("Could not find scene ", scene)

## Enables the given scene but does not show it (this only turns on processing)
func run_in_background(scene: String):
	if current_scenes.has(scene):
		var _scene = _root.get_node(scene)
		_scene.hide()
		_scene.set("process_mode",PROCESS_MODE_INHERIT)
		current_scenes[scene] = true
	else:
		push_error("Could not find scene ", scene)

## Enable support for loading resources with subthreads.[br]
## [b]Warning! This setting will be overriden if you disable/enable support for it with loading scene settings. Either use this function once globally or manually specify subthreaded loading support for each individual scene.[/b]
func set_subthreads(status = true):
	_subthreads_enabled = status

## Sets different cache mode for loading resources. Refer to Godot's documentation on CACHE_MODE for more details.[br]
## [b]Warning! This setting will be overriden if you set a different cache mode with loading scene settings. Either use this function once globally or manually specify cache mode for each individual scene.[/b]
func set_cache_mode(mode = 0):
	match mode:
		0,1,2:
			_cache_mode = mode
		_:
			push_error("Valid inputs for set_cache_mode are 0, 1 and 2.")

## Sets a default loading screen for the next scene. This is used when last_setting is not present in args. path_override can be added when the loading screen's location is different than in default path.
func set_default_loading_screen(loading_screen_name: String, path_override: String = ""):
	if _root == null:
		await ready

	var path = path_override if path_override else loading_screens_default_path

	var _screen = _root.get_node_or_null(path + loading_screen_name)
	if _screen:
		_default_loading_screen = _screen
	else:
		push_error("Loading screen could not be found in ", path, ": ", loading_screen_name)

## Removes the default loading screen.
func reset_default_loading_screen():
	_default_loading_screen = null

## Sets the default parent node for all scenes added with add_scene.
func set_default_parent_node(path_to_node: String):
	if _root == null:
		await ready

	var node = _root.get_node_or_null(path_to_node)
	if node:
		_default_parent_node = node
	else:
		push_error("Node could not be found in root/", path_to_node)

## Sets the default parent node back to root.
func reset_default_parent_node():
	_default_parent_node = _root

## Adds a scene from PackedScene. Also supports settings for loading. Third argument is for adding/updating Composer variables.[br]
## [b]For list of valid settings, refer to add_scene_from_string![/b][br]
## [b]Warning! "subthreads", "cache_mode" and "singlethread" settings do not have effect in this function!
func add_scene_from_packed(scene: PackedScene, args = {}, vars = {}):
	if _root == null:
		await ready

	if _is_loading:
		push_warning("A scene is already being loaded.")
		return

	is_safe_to_continue = false
	is_safe_to_finish = false

	_is_loading = true
	_loading_scene = scene

	if typeof(args) == TYPE_STRING:
		args = settings_presets.get(args,{})

	args = await _set_preparing_args(args,vars)

	call_deferred("_deferred_add_scene_packed",_loading_scene,args,vars)

func _deferred_add_scene_packed(scene: PackedScene,args,vars):
	if is_debug_enabled: print("DEBUG: Beggining adding scene ", scene, " with args ", args, " and variables ", vars)

	loading_began.emit(scene.resource_name)

	var loading_screen = null
	if args["loading_screen"] != "":
		loading_screen = _get_loading_screen(args["loading_screen"],args["loading_screen_path"])
		if not loading_screen:
			push_error("Loading screen has not been found! ",args["loading_screen"], " at ",args["loading_screen_path"])
			return

		loading_screen_activated.emit(loading_screen.name)
		if !is_safe_to_continue:
			await loading_screen.continue_loading

	args = await _set_preloading_args(args,vars)

	if is_debug_enabled: print("DEBUG: Finished adding scene ", scene)

	var _parent = _get_parent_node(args["parent_node"])
	_parent.add_child(scene.instantiate())

	var _scene = _parent.get_child(_parent.get_child_count() - 1)
	_scene.set("process_mode",PROCESS_MODE_DISABLED)
	current_scenes[_scene.name] = false
	_internal_scene_paths[_scene.name] = scene
	_last_settings[_scene.name] = args
	_last_vars[_scene.name] = vars
	_is_loading = false

	loading_done.emit(_scene.name,0)

	if loading_screen != null:
		loading_screen_disabled.emit(loading_screen.name)
		if !is_safe_to_finish:
			await loading_screen.finished_loading

	_set_after_loading_args(_scene.name,args,vars)

## Adds a scene from a path or a name shortcut if it exists. Also supports settings for loading. Third argument is for adding/updating Composer variables.[br]
## Valid settings are:[br]
## - [b]parent_node[/b]: String - takes the relative path from root to the node which you want the loaded scene to be a child of. Default is
## - [b]loading_screen[/b]: String - takes a name of the loading screen that is located at _root/CanvasLayer.[br]
## - [b]loading_screen_path[/b]: String - location of the loading screen given in loading_screen argument. This will not override the default path.[br]
## - [b]loading_screen_args[/b]: Dictionary - takes a dictionary with all the variables for the loading screen. You can get them later with get_variables_for_loading_screen [br]
## - [b]subthreads[/b]: Bool - this will enable/disable support for loading scenes using subthreads. Default is false, unless you change it with set_subthreads[br]
## - [b]cache_mode[/b]: Integer(0,2) - this will set a different cache mode for loading scene. Refer to CACHE_MODE documentation for more info. Default is 1, unless you change it with set_cache_mode.[br]
## - [b]no_override[/b]: Bool - if true, this will not update variables if they already existed in memory. Default is false.[br]
## - [b]clear_all_previous[/b]: Bool - if true, this will remove all previous existing scenes. Default is false[br]
## - [b]singlethread[/b]: Bool - if true, this will load the scene using only one thread, which will pause the execution of the program. Default is false.[br]
## - [b]activate_on_startup[/b]: Bool - if true, this will automatically enable the scene when it is done loading. [b]Warning! This setting is enabled by default, and you do not have to include it in the settings dictionary. Only include it if you want it disabled![/b][br]
## - [b]run_in_background[/b]: Bool - if true, this will enable the scene when it is done loading, but it will not show it. [b] Warning! Having this setting turned on will ignore "activate_on_startup" even if it's manually included.[/b]
func add_scene_from_string(scene: String, args = {}, vars = {}):
	if _root == null:
		await ready

	if _is_loading:
		push_warning("A scene is already being loaded.")
		return

	is_safe_to_continue = false
	is_safe_to_finish = false

	_is_loading = true
	_loading_path = scene

	if scene_paths.has(scene):
		_loading_path = scene_paths[scene]

	if typeof(args) == TYPE_STRING:
		args = settings_presets.get(args,{})

	args = await _set_preparing_args(args,vars)

	call_deferred("_deferred_add_scene_string",_loading_path,args,vars)

func _deferred_add_scene_string(path: String,args,vars):
	var _loaded_scene

	if is_debug_enabled: print("DEBUG: Beggining loading scene ", path, " with args ", args, " and variables ", vars)
	loading_began.emit(path)

	var loading_screen = null
	if args["loading_screen"] != "":
		loading_screen = _get_loading_screen(args["loading_screen"],args["loading_screen_path"])
		if not loading_screen:
			push_error("Loading screen has not been found: ",args["loading_screen"], " at ",args["loading_screen_path"])
			return

		loading_screen_activated.emit(loading_screen.name)
		if !is_safe_to_continue:
			await loading_screen.continue_loading

	args = await _set_preloading_args(args,vars)

	if args.get("singlethread",false):
		if is_debug_enabled: print("DEBUG: Singlethreaded loading enabled")
		_loaded_scene = ResourceLoader.load(path,"",_cache_mode)
	else:
		var _loaded_state = ResourceLoader.load_threaded_request(path,"",_subthreads_enabled,_cache_mode)
		if is_debug_enabled: print("DEBUG: Loading state: ", _loaded_state)
		if _loaded_state == OK:
			set_process(true)
		else:
			if is_debug_enabled: print("DEBUG: Failed loading scene ", path)
			loading_done.emit(path,_loaded_state)
			push_error("Failed to load scene at path ", path)
			return

		await _loading_continue

		set_process(false)

		if _internal_loading_val == INTERNAL_LOADING_VALS.FAILED:
			if is_debug_enabled: print("DEBUG: Failed loading scene ", path, " Interval loading value is: ", _internal_loading_val)
			loading_done.emit(path,1)
			push_error("Failed to load scene at path ", path, " Code: ",_internal_loading_val)
			return

		_loaded_scene = ResourceLoader.load_threaded_get(path)

	if is_debug_enabled: print("DEBUG: Finished loading scene ", path)

	var _parent
	if args["parent_node"] != "":
		_parent = _get_parent_node(args["parent_node"])
	else:
		_parent = _default_parent_node

	_parent.add_child(_loaded_scene.instantiate())

	var _scene = _parent.get_child(_parent.get_child_count() - 1)
	_scene.set("process_mode",PROCESS_MODE_DISABLED)
	current_scenes[_scene.name] = false
	_internal_scene_paths[_scene.name] = path
	_last_settings[_scene.name] = args
	_last_vars[_scene.name] = vars
	_is_loading = false

	loading_done.emit(_scene.name,0)

	if loading_screen != null:
		loading_screen_disabled.emit(loading_screen.name)
		if !is_safe_to_finish:
			await loading_screen.finished_loading

	_set_after_loading_args(_scene.name,args,vars)

func _get_parent_node(path):
	var _node = _root.get_node_or_null(path)

	if not _node:
		push_error("Parent node could not be found in ", path, "! Using default parent node...")
		return _default_parent_node

	return _node

func _get_loading_screen(screen, path=""):
	var path_to_screen = path if path else loading_screens_default_path
	var _screen = _root.get_node_or_null(path_to_screen + screen)

	if not _screen:
		push_error("Loading screen could not be found in ", path_to_screen + screen, ": ", screen)

	return _screen

func _set_preparing_args(args,vars):
	var _path = args["loading_screen_path"] if args.has("loading_screen_path") else ""

	args["parent_node"] = args.get("parent_node","")

	if _default_loading_screen:
		args["loading_screen"] = args.get("loading_screen",_default_loading_screen.name)
	else:
		args["loading_screen"] = args.get("loading_screen","")

	args["loading_screen_path"] = args.get("loading_screen_path",loading_screens_default_path)
	if args.has("loading_screen_variables"): _loading_screen_variables = args["loading_screen_variables"]
	else: _loading_screen_variables.clear()

	args["subthreads"] = args.get("subthreads",_subthreads_enabled)
	args["cache_mode"] = args.get("cache_mode",_cache_mode)

	if args.get("no_override",false) == false:
		_variables.merge(vars,true)
	else:
		_variables.merge(vars,false)

	return args

func _set_preloading_args(args, _vars):
	if args.get("clear_all_previous",false):
		await clear_all_scenes()

	return args

func _set_after_loading_args(scene_name: String, args, _vars):
	if args.get("run_in_background",false):
		run_in_background(scene_name)
	elif args.get("activate_on_startup",true):
		activate_scene(scene_name)
	else:
		disable_scene(scene_name)

func _is_one_scene_active() -> bool:
	return current_scenes.values().count(true) == 1

func _process(_delta):
	var _percentage = []
	var _progress = ResourceLoader.load_threaded_get_status(_loading_path,_percentage)

	match _progress:
		0,2:
			_internal_loading_val = INTERNAL_LOADING_VALS.FAILED
			_loading_continue.emit()
		3:
			_internal_loading_val = INTERNAL_LOADING_VALS.SUCCESS
			loading_value_updated.emit(1)
			_loading_continue.emit()
		1:
			loading_value_updated.emit(_percentage[0])
