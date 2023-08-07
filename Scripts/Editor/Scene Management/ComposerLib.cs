using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class ComposerLib : Node
{
    /// <summary>
    /// Fires whenever loading scene has successfully started.
    /// </summary>
    [Signal]
    public delegate void LoadingBeganEventHandler(string sceneName);

    /// <summary>
    /// Fires when loading the scene. Percentage value may not be very accurate (https://github.com/godotengine/godot/issues/56882)
    /// </summary>
    [Signal]
    public delegate void LoadingValueUpdatedEventHandler(float value);

    /// <summary>
    /// Fires whenever loading has failed or finished. Check error code to see results. 0 always means success.
    /// </summary>
    [Signal]
    public delegate void LoadingDoneEventHandler(string sceneName,int errorCode);

    [Signal]
    public delegate void _LoadingContinueEventHandler();

    /// <summary>
    /// Fires whenever scene has been activated. Use this signal inside of scene to activate stuff like CanvasLayers and other stuff.
    /// </summary>
    [Signal]
    public delegate void SceneActivatedEventHandler(string sceneName);

    /// <summary>
    /// Fires whenever scene has been disabled. Use this signal inside of scene to deactivate stuff like CanvasLayers and other stuff.
    /// </summary>
    [Signal]
    public delegate void SceneDisabledEventHandler(string sceneName);

    /// <summary>
    /// Fires whenever Composer wants to activate a Loading Screen.
    /// </summary>
    [Signal]
    public delegate void LoadingScreenActivatedEventHandler(string screenName);

    /// <summary>
    /// Fires whenever Composer wants to disable a Loading Screen.
    /// </summary>
    [Signal]
    public delegate void LoadingScreenDisabledEventHandler(string screenName);


    private Dictionary<string,Variant> _internalScenePaths = new(){

    };

    private static Node _root;
    private static Node _defaultParentNode = _root;

    private static bool _subthreadsEnabled = false;
    private static ResourceLoader.CacheMode _cacheMode = ResourceLoader.CacheMode.Replace;

    private static bool _isLoading = false;
    private static string _loadingPath = "";
    private static PackedScene _loadingScene = null;
    private static Control _defaultLoadingScreen = null;

    private static readonly Dictionary<string,Variant> _lastSettings = new(){

    };

    private static readonly Dictionary<string,Variant> _variables = new(){

    };

    private static Dictionary<string,Variant> _loadingScreenVariables = new(){

    };

    private static readonly Dictionary<string,Variant> _lastVariables = new(){

    };

    enum InternalLoadStatus {
        Failed = 0,
        Success = 1,
    };

    private static InternalLoadStatus _internalLoadingVal = InternalLoadStatus.Success;

    private static readonly Dictionary<string,string> scenePaths = new(){
        {"MainMenu","res://Scenes/MainMenu.tscn"},
        {"Game","res://Scenes/Game.tscn"},
    };

    private static readonly Dictionary<string,bool> currentScenes = new(){

    };

    private static readonly Dictionary<string,Dictionary<string,Variant>> settingsPresets = new(){
        {"preset1",new(){
            {"loading_screen","LoadingScreen"},
            {"clear_all_previous",true},
        }}
    };

    private static string loadingScreensDefaultPath = "CanvasLayer/";
    private static bool isDebugEnabled = false;

    public bool IsSafeToContinueLoading {get; set;} = false ;
    public bool IsSafeToFinishLoading {get; set;} = false;

    public override void _Ready()
    {
        base._Ready();

        _root = GetTree().CurrentScene;
        _defaultParentNode = _root;

        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Godot.Collections.Array percentage = new();
        ResourceLoader.ThreadLoadStatus _progress = ResourceLoader.LoadThreadedGetStatus(_loadingPath,percentage);

        switch (_progress)
        {
            case ResourceLoader.ThreadLoadStatus.InvalidResource:
                _internalLoadingVal = InternalLoadStatus.Failed;
                EmitSignal("_LoadingContinue");
                break;
            case ResourceLoader.ThreadLoadStatus.Failed:
                _internalLoadingVal = InternalLoadStatus.Failed;
                EmitSignal("_LoadingContinue");
                break;
            case ResourceLoader.ThreadLoadStatus.InProgress:
                EmitSignal("LoadingValueUpdated",percentage[0]);
                break;
            case ResourceLoader.ThreadLoadStatus.Loaded:
                _internalLoadingVal = InternalLoadStatus.Success;
                EmitSignal("_LoadingContinue");
                break;
        }
    }

    public void SetDebugEnabled(bool value = true)
    {
        isDebugEnabled = value;
    }

    public void ChangeDefaultLoadingScreenPath(string path)
    {
        if (!path.EndsWith("/"))
            path += "/";

        loadingScreensDefaultPath = path;
    }

    /// <summary>
    /// Returns all variables for the loading screen currently in Composer's memory
    /// </summary>
    /// <returns>
    /// A Dictionary with variables for the loading screen
    /// </returns>
    public Dictionary<string,Variant> GetVariablesForLoadingScreen()
    {
        return _loadingScreenVariables;
    }

    /// <summary>
    /// Returns all variables currently in Composer's memory
    /// </summary>
    /// <returns>
    /// A Dictionary with variables
    /// </returns>
    public Dictionary<string,Variant> GetVariables()
    {
        return _variables;
    }

    /// <summary>
    /// Returns given variable.
    /// </summary>
    /// <returns>
    /// Variable's value if it exists. Returns Godot.Variant.Default if variable doesn't exist.
    /// </returns>
    public Variant GetVariable(string variable)
    {
        if (_variables.ContainsKey(variable))
            return _variables[variable];

        GD.PushError($"{variable} does not exist!");
        return default;
    }

    /// <summary>
    /// Updates variable with given value. This can also be used to add variables.
    /// </summary>
    public void UpdateVariable(string variable, Variant value)
    {
        if (_variables.ContainsKey(variable))
            _variables[variable] = value;
        else
            _variables.Add(variable,value);
    }

    /// <summary>
    /// Removes given variable.
    /// </summary>
    public void RemoveVariable(string variable)
    {
        _variables.Remove(variable);
    }

    /// <summary>
    /// Removes all existing variables.
    /// </summary>
    public void ClearVariables()
    {
        _variables.Clear();
    }

    /// <summary>
    /// Adds a shortcut name for loading scene.
    /// </summary>
    public void AddScenePath(string scene, string path)
    {
        scenePaths.Add(scene,path);
    }

    /// <summary>
    /// Adds a settings preset.
    /// </summary>
    public void AddSettingsPreset(string name, Dictionary<string,Variant> settings)
    {
        settingsPresets.Add(name,settings);
    }

    /// <summary>
    /// Returns the state of scene.
    /// </summary>
    /// <returns>
    /// Bool representing state of scene
    /// </returns>
    public bool GetScene(string scene)
    {
        if (currentScenes.ContainsKey(scene))
            return currentScenes[scene];

        GD.PushError($"{scene} could not be found!");
        return false;
    }

    /// <summary>
    /// Returns all scenes with their states.
    /// </summary>
    /// <returns>
    /// Dictionary with scene states
    /// </returns>
    public Dictionary<string,bool> GetScenes()
    {
        return currentScenes;
    }

    /// <summary>
    /// Removes every loaded scene.
    /// </summary>
    public void ClearAllScenes()
    {
        foreach (string scene in currentScenes.Keys)
            RemoveScene(scene);

        currentScenes.Clear();
    }

    /// <summary>
    /// Removes given scene.
    /// </summary>
    public void RemoveScene(string scene)
    {
        if (currentScenes.ContainsKey(scene))
        {
            _lastSettings.Remove(scene);
            _lastVariables.Remove(scene);
            currentScenes.Remove(scene);
            using var _scene = _root.GetNode<Node>(scene);
            _scene.QueueFree();

            GD.Print(_lastSettings);
        }
        else
            GD.PushError($"{scene} could not be found!");
    }

    /// <summary>
    /// Removes current active scene. Note that for this to work there can only be one active scene present.
    /// </summary>
    public void RemoveCurrentScene()
    {
        if (IsOneSceneActive())
        {
            foreach (string key in currentScenes.Keys)
            {
                if (currentScenes[key] == true)
                {
                    RemoveScene(key);
                    return;
                }
            }
            GD.PushError("Couldn't find current scene.");
        }
        else
            GD.PushError("Couldn't remove current scene. This can happen when there are multiple scenes active or none. Try using RemoveScene instead.");
    }

    /// <summary>
    /// Enables the given scene to run in background (hidden but still processing).
    /// </summary>
    public void RunInBackground(string scene)
    {
        if (currentScenes.ContainsKey(scene))
        {
            Node _scene = _root.GetNode<Node>(scene);
            _scene.Set("visible",false);
            _scene.ProcessMode = ProcessModeEnum.Inherit;
            currentScenes[scene] = true;
            EmitSignal("SceneActivated",scene);
        }
        else
            GD.PushError($"Could not find scene {scene}");
    }

    /// <summary>
    /// Enables the given scene and shows it.
    /// </summary>
    public void ActivateScene(string scene)
    {
        if (currentScenes.ContainsKey(scene))
        {
            Node _scene = _root.GetNode<Node>(scene);
            _scene.Set("visible",true);
            _scene.ProcessMode = ProcessModeEnum.Inherit;
            currentScenes[scene] = true;
            EmitSignal("SceneActivated",scene);
        }
        else
            GD.PushError($"Could not find scene {scene}");
    }

    /// <summary>
    /// Disables the given scene and hides it.
    /// </summary>
    public void DisableScene(string scene)
    {
        if (currentScenes.ContainsKey(scene))
        {
            Node _scene = _root.GetNode<Node>(scene);
            _scene.Set("visible",false);
            _scene.ProcessMode = ProcessModeEnum.Disabled;
            currentScenes[scene] = false;
            EmitSignal("SceneDisabled",scene);
        }
        else
            GD.PushError($"Could not find scene {scene}");
    }

    /// <summary>
    /// Disables every loaded scene.
    /// </summary>
    public void DisableAllScenes()
    {
        foreach (string scene in currentScenes.Keys)
            DisableScene(scene);
    }

    /// <summary>
    /// Disables current active scene. Note that for this to work there can only be one active scene present.
    /// </summary>
    public void DisableCurrentScene()
    {
        if (IsOneSceneActive())
        {
            foreach (string key in currentScenes.Keys)
            {
                if (currentScenes[key] == true)
                {
                    DisableScene(key);
                    return;
                }
            }
            GD.PushError("Couldn't find current scene.");
        }
        else
            GD.PushError("Couldn't disable current scene. This can happen when there are multiple scenes active or none. Try using DisableScene instead.");
    }

    /// <summary>
    /// Sets a default loading screen for the next loaded scenes. This is used when last_setting is not present in args. pathOverride can be added when the loading screen's location is different than in default path.
    /// </summary>
    public async void SetDefaultLoadingScreen(string name, string pathOverride="")
    {
        if (_root == null)
            await ToSignal(this,"ready");

        string path = pathOverride != "" ? pathOverride : loadingScreensDefaultPath;
        Control _screen = _root.GetNodeOrNull<Control>($"{path}{name}");

        if (_screen != null)
            _defaultLoadingScreen = _screen;
        else
            GD.PushError($"Could not find loading screen {name}");
    }

    /// <summary>
    /// Removes the default loading screen.
    /// </summary>
    public void ResetDefaultLoadingScreen()
    {
        _defaultLoadingScreen = null;
    }

    /// <summary>
    /// Sets the default parent node for all scenes added with AddScene.
    /// </summary>
    public async void SetDefaultParentNode(string path)
    {
        if (_root == null)
            await ToSignal(this,"ready");

        Node _node = _root.GetNodeOrNull<Node>(path);
        if (_node != null)
            _defaultParentNode = _node;
        else
            GD.PushError($"Node could not be found in root/{path}");
    }

    /// <summary>
    /// Sets the default parent node back to root.
    /// </summary>
    public void ResetDefaultParentNode()
    {
        _defaultParentNode = _root;
    }

    /// <summary>
    /// Enables/Disables support for loading resources with subthreads.
    /// Warning! This setting will be overriden if you disable/enable support for it with loading scene settings. Either use this function once globally or manually specify subthreaded loading support for each individual scene.
    /// </summary>
    public void SetSubthreads(bool status)
    {
        _subthreadsEnabled = status;
    }

    /// <summary>
    /// Sets different cache mode for loading resources. Refer to Godot's documentation on ResourceLoader.CacheMode for more details.
    /// Warning! This setting will be overriden if you disable/enable support for it with loading scene settings. Either use this function once globally or manually specify subthreaded loading support for each individual scene.
    /// </summary>
    public void SetCacheMode(int cacheMode)
    {
        switch (cacheMode)
        {
            case 0:
                _cacheMode = ResourceLoader.CacheMode.Ignore;
                break;
            case 1:
                _cacheMode = ResourceLoader.CacheMode.Reuse;
                break;
            case 2:
                _cacheMode = ResourceLoader.CacheMode.Replace;
                break;
            default:
                GD.PushError("Invalid cache mode. Valid options are 0, 1 and 2.");
                break;
        }
    }

    /// <summary>
    /// Reloads the current active scene. Note that for this to work there can only be one active scene present. args parameter can take settings for reloading the scene and sceneVars can hold variables.
    /// Valid settings are:
    /// <list type="bullet">
    /// <item>use_previous_settings - this will use last settings used when previously loading/reloading the scene ignoring the args parameter.</item>
    /// <item>use_previous_variables - this will use last variables used when previously loading/reloading the scene ignoring the sceneVars parameter.</item>
    /// <item>no_settings_override - this will not override last used settings for this scene with args parameter.</item>
    /// <item>no_variables_override - this will not override last used variables for this scene with sceneVars parameter.</item>
    /// </list>
    /// For more settings refer to AddScene documentation (please mind that if you are reloading a PackedScene some settings will be unavailable).
    /// </summary>
    public void ReloadCurrentScene(Dictionary<string,Variant> args = null, Dictionary<string,Variant> sceneVars = null)
    {
        if (IsOneSceneActive())
        {
            foreach (string key in currentScenes.Keys)
            {
                if (currentScenes[key] == true)
                {
                    ReloadScene(key,args,sceneVars);
                    return;
                }
            }
            GD.PushError("Couldn't find current scene.");
        }
        else
            GD.PushError("Couldn't reload current scene. This can happen when there are multiple scenes active or none. Try using ReloadScene instead.");
    }

    /// <summary>
    /// Reloads the given scene. args parameter can take settings for reloading the scene and sceneVars can hold variables.
    /// Valid settings are:
    /// <list type="bullet">
    /// <item>use_previous_settings: Bool - this will use last settings used when previously loading/reloading the scene ignoring the args parameter.</item>
    /// <item>use_previous_variables: Bool - this will use last variables used when previously loading/reloading the scene ignoring the sceneVars parameter.</item>
    /// <item>no_settings_override: Bool - this will not override last used settings for this scene with args parameter.</item>
    /// <item>no_variables_override: Bool - this will not override last used variables for this scene with sceneVars parameter.</item>
    /// </list>
    /// For more settings refer to AddScene documentation (please mind that if you are reloading a PackedScene some settings will be unavailable).
    /// </summary>
    public void ReloadScene(string scene, Dictionary<string,Variant> args = null, Dictionary<string,Variant> sceneVars = null)
    {
        if (currentScenes.ContainsKey(scene))
        {
            if (!_internalScenePaths.ContainsKey(scene))
            {
                GD.PushError($"Could not find scene ", scene);
                return;
            }

            RemoveScene(scene);

            Dictionary<string,Variant> argsToUse;
            Dictionary<string,Variant> varsToUse;

            bool usePreviousArgs = args.ContainsKey("use_previous_settings") && (bool)args["use_previous_settings"];
            if (usePreviousArgs)
                argsToUse = (Dictionary<string,Variant>)_lastSettings[scene];
            else
                argsToUse = args;

            bool usePreviousVars = args.ContainsKey("use_previous_variables") && (bool)args["use_previous_variables"];
            if (usePreviousVars)
                varsToUse = (Dictionary<string,Variant>)_lastVariables[scene];
            else
                varsToUse = sceneVars;

            bool noSettingsOverride = args.ContainsKey("no_settings_override") && (bool)args["no_settings_override"];
            if (!noSettingsOverride)
                _lastSettings[scene] = argsToUse;

            bool noVarsOverride = args.ContainsKey("no_variables_override") && (bool)args["no_variables_override"];
            if (!noVarsOverride)
                _lastVariables[scene] = varsToUse;

            if (_internalScenePaths[scene].VariantType == Variant.Type.String)
                AddScene((string)_internalScenePaths[scene],argsToUse,varsToUse);
            else
                AddScene((PackedScene)_internalScenePaths[scene],argsToUse,varsToUse);
        }
        else
            GD.PushError($"Could not find scene {scene}");
    }

    /// <summary>
    /// Reloads the given scene. args parameter is a key for preset settings in settingsPresets and sceneVars can hold variables.
    /// </summary>
    public void ReloadScene(string scene, string args = "", Dictionary<string,Variant> sceneVars = null)
    {
        if (currentScenes.ContainsKey(scene))
        {
            if (!_internalScenePaths.ContainsKey(scene))
            {
                GD.PushError($"Could not find scene ", scene);
                return;
            }

            RemoveScene(scene);

            Dictionary<string,Variant> argsToUse = new(){};
            Dictionary<string,Variant> varsToUse;

            if (settingsPresets.ContainsKey(args))
                argsToUse = settingsPresets[args];

            bool usePreviousArgs = argsToUse.ContainsKey("use_previous_settings") && (bool)argsToUse["use_previous_settings"];
            if (usePreviousArgs)
                argsToUse = (Dictionary<string,Variant>)_lastSettings[scene];

            bool usePreviousVars = argsToUse.ContainsKey("use_previous_variables") && (bool)argsToUse["use_previous_variables"];
            if (usePreviousVars)
                varsToUse = (Dictionary<string,Variant>)_lastVariables[scene];
            else
                varsToUse = sceneVars;

            bool noSettingsOverride = argsToUse.ContainsKey("no_settings_override") && (bool)argsToUse["no_settings_override"];
            if (!noSettingsOverride)
                _lastSettings[scene] = argsToUse;

            bool noVarsOverride = argsToUse.ContainsKey("no_variables_override") && (bool)argsToUse["no_variables_override"];
            if (!noVarsOverride)
                _lastVariables[scene] = varsToUse;

            if (_internalScenePaths[scene].VariantType == Variant.Type.String)
                AddScene((string)_internalScenePaths[scene],argsToUse,varsToUse);
            else
                AddScene((PackedScene)_internalScenePaths[scene],argsToUse,varsToUse);
        }
        else
            GD.PushError($"Could not find scene {scene}");
    }

    /// <summary>
    /// Loads a new scene to Main using either full path to scene or by taking in path shortcut. args parameter can take settings for reloading the scene and sceneVars can hold variables.
    /// Valid settings are:
    /// <list type="bullet">
    /// <item>parent_node: String - takes a location of the parent node that will have this scene as a child relative from the root.</item>
    /// <item>loading_screen: String - takes a name of the loading screen that is located at root/CanvasLayer. On startup it is empty.</item>
    /// <item>loading_screen_path: String - takes a location of the loading screen relative from the root. On startup it is CanvasLayer/.</item>
    /// <item>loading_screen_args: Dictionary - takes a dictionary with all variables for loading screen. You can get them with GetVariablesForLoadingScreen().</item>
    /// <item>subthreads: Bool - enables/disables support for loading scenes with subthreads. On startup it is false.</item>
    /// <item>cache_mode: int(0,1,2) - this will set a cache mode for loading scenes. Refer to ResourceLoader.CacheMode for details. On startup it is 1.</item>
    /// <item>no_override: Bool - if true, this will not update preexisting variables with new values. Default is false.</item>
    /// <item>clear_all_previous: Bool - if true, this will remove all previously existing scenes. Default is false.</item>
    /// <item>singlethread: Bool - if true, this will load the scene using only one thread, blocking the execution of the program until it is done. Default is false.</item>
    /// <item>activate_on_startup: Bool - if true, this will automatically enable the scene once it's done loading. Default is true. Include this only if you want to disable automatic activation on load.</item>
    /// <item>run_in_background: Bool - if true, this will automatically run the scene in background once it's done loading. Default is false. Enabling this will automatically ignore activate_on_startup.</item>
    /// </list>
    /// </summary>
    public async void AddScene(string path, Dictionary<string,Variant> args = null, Dictionary<string,Variant> sceneVars = null)
    {
        if (_root == null)
            await ToSignal(this,"ready");

        if (_isLoading)
        {
            GD.PushWarning("A scene is already being loaded. Aborting...");
            return;
        }

        IsSafeToContinueLoading = false;
        IsSafeToFinishLoading = false;

        _isLoading = true;
        _loadingPath = path;

        args ??= new Dictionary<string, Variant>();
        sceneVars ??= new Dictionary<string, Variant>();

        if (scenePaths.ContainsKey(path))
            _loadingPath = scenePaths[path];

        args = SetPreparingArgs(args,sceneVars);

        CallDeferred("DeferredAddScene",_loadingPath,args,sceneVars);
    }

    /// <summary>
    /// Loads a new scene to Main using either full path to scene or by taking in path shortcut. args parameter is a key for preset settings in settingsPresets and sceneVars can hold variables.
    /// </summary>
    public async void AddScene(string path, string argskey = "", Dictionary<string,Variant> sceneVars = null)
    {
        if (_root == null)
            await ToSignal(this,"ready");

        if (_isLoading)
        {
            GD.PushWarning("A scene is already being loaded. Aborting...");
            return;
        }

        IsSafeToContinueLoading = false;
        IsSafeToFinishLoading = false;

        _isLoading = true;
        _loadingPath = path;

        Dictionary<string, Variant> args = new(){};

        if (settingsPresets.ContainsKey(argskey))
            args = settingsPresets[argskey];

        sceneVars ??= new Dictionary<string, Variant>();

        if (scenePaths.ContainsKey(path))
            _loadingPath = scenePaths[path];

        args = SetPreparingArgs(args,sceneVars);

        CallDeferred("DeferredAddScene",_loadingPath,args,sceneVars);
    }

    /// <summary>
    /// Loads a new scene to root using a PackedScene. args parameter can take settings for reloading the scene and sceneVars can hold variables.
    /// Valid settings are:
    /// <list type="bullet">
    /// <item>parent_node: String - takes a location of the parent node that will have this scene as a child relative from the root.</item>
    /// <item>loading_screen: String - takes a name of the loading screen that is located at root/CanvasLayer. On startup it is empty.</item>
    /// <item>loading_screen_path: String - takes a location of the loading screen relative from the root. On startup it is CanvasLayer/.</item>
    /// <item>loading_screen_args: Dictionary - takes a dictionary with all variables for loading screen. You can get them with GetVariablesForLoadingScreen().</item>
    /// <item>no_override: Bool - if true, this will not update preexisting variables with new values. Default is false.</item>
    /// <item>clear_all_previous: Bool - if true, this will remove all previously existing scenes. Default is false.</item>
    /// <item>activate_on_startup: Bool - if true, this will automatically enable the scene once it's done loading. Default is true. Include this only if you want to disable automatic activation on load.</item>
    /// <item>run_in_background: Bool - if true, this will automatically run the scene in background once it's done loading. Default is false. Enabling this will automatically ignore activate_on_startup.</item>
    /// </list>
    /// </summary>
    public async void AddScene(PackedScene scene, Dictionary<string,Variant> args = null, Dictionary<string,Variant> sceneVars = null)
    {
        if (_root == null)
            await ToSignal(this,"ready");

        if (_isLoading)
        {
            GD.PushWarning("A scene is already being loaded. Aborting...");
            return;
        }

        IsSafeToContinueLoading = false;
        IsSafeToFinishLoading = false;

        _isLoading = true;
        _loadingScene = scene;

        args = SetPreparingArgs(args,sceneVars);

        CallDeferred("DeferredAddScene",_loadingScene,args,sceneVars);
    }

    /// <summary>
    /// Loads a new scene to Main using a PackedScene. args parameter is a key for preset settings in settingsPresets and sceneVars can hold variables.
    /// </summary>
    public async void AddScene(PackedScene scene, string argskey = "", Dictionary<string,Variant> sceneVars = null)
    {
        if (_root == null)
            await ToSignal(this,"ready");

        if (_isLoading)
        {
            GD.PushWarning("A scene is already being loaded. Aborting...");
            return;
        }

        IsSafeToContinueLoading = false;
        IsSafeToFinishLoading = false;

        _isLoading = true;
        _loadingScene = scene;

        Dictionary<string, Variant> args = new(){};

        if (settingsPresets.ContainsKey(argskey))
            args = settingsPresets[argskey];

        args = SetPreparingArgs(args,sceneVars);

        CallDeferred("DeferredAddScene",_loadingScene,args,sceneVars);
    }

    private async void DeferredAddScene(string path, Dictionary<string,Variant> args, Dictionary<string,Variant> sceneVars)
    {
        PackedScene _loadedScene;

        if (isDebugEnabled)
            GD.Print($"DEBUG: Beggining loading scene {path} with args {args} and variables {sceneVars}");

        EmitSignal("LoadingBegan", path);

        Control _loadingScreen = null;
        if (args["loading_screen"].ToString() != "")
        {
            _loadingScreen = GetLoadingScreen(args["loading_screen"].ToString(),args["loading_screen_path"].ToString());
            if (_loadingScreen == null)
            {
                GD.PushError($"Loading screen has not been found: {args["loading_screen"]}");
                return;
            }

            EmitSignal("LoadingScreenActivated",_loadingScreen.Name);

            if (!IsSafeToContinueLoading)
                await ToSignal(_loadingScreen,"ContinueLoading");
        }

        args = SetPreloadingArgs(args,sceneVars);

        bool _isSinglethread = args.ContainsKey("singlethread") && (bool)args["singlethread"];

        if (_isSinglethread)
            _loadedScene = ResourceLoader.Load<PackedScene>(path,"",_cacheMode);
        else
        {
            Error _loadedState = ResourceLoader.LoadThreadedRequest(path,"",_subthreadsEnabled,_cacheMode);

            if (_loadedState == Error.Ok)
                SetProcess(true);
            else
            {
                if (isDebugEnabled)
                    GD.Print($"DEBUG: Failed to load scene {path}");

                EmitSignal("LoadingDone",path,(int)_loadedState);
                return;
            }

            await ToSignal(this,"_LoadingContinue");

            SetProcess(false);

            if (_internalLoadingVal == InternalLoadStatus.Failed)
            {
                if (isDebugEnabled)
                    GD.Print($"DEBUG: Failed to load scene {path}. Internal status is {_internalLoadingVal}");

                EmitSignal("LoadingDone",path,1);
                return;
            }

            _loadedScene = (PackedScene)ResourceLoader.LoadThreadedGet(path);
        }

        if (isDebugEnabled)
            GD.Print($"DEBUG: Finished to load scene {path}");

        Node _parent;
        if ((string)args["parent_node"] != "")
            _parent = GetParentNode((string)args["parent_node"]);
        else
            _parent = _defaultParentNode;

        _parent.AddChild(_loadedScene.Instantiate());

        Node _scene = _parent.GetChild(_parent.GetChildCount() - 1);
        _scene.ProcessMode = ProcessModeEnum.Disabled;
        currentScenes.Add(_scene.Name,false);
        if (!_internalScenePaths.ContainsKey(_scene.Name))
            _internalScenePaths.Add(_scene.Name,path);
        _lastSettings.Add(_scene.Name,args);
        _lastVariables.Add(_scene.Name,sceneVars);
        _isLoading = false;

        EmitSignal("LoadingDone",_scene.Name,0);

        if (_loadingScreen != null)
        {
            EmitSignal("LoadingScreenDisabled",_loadingScreen.Name);
            if (!IsSafeToFinishLoading)
                await ToSignal(_loadingScreen,"FinishedLoading");
        }

        SetAfterLoadingArgs(_scene.Name,args,sceneVars);
    }

    private async void DeferredAddScene(PackedScene scene, Dictionary<string,Variant> args, Dictionary<string,Variant> sceneVars)
    {
        if (isDebugEnabled)
            GD.Print($"DEBUG: Beggining adding scene {scene} with args {args} and variables {sceneVars}");

        EmitSignal("LoadingBegan",scene.ResourceName);

        Control _loadingScreen = null;
        if (args["loading_screen"].ToString() != "")
        {
            _loadingScreen = GetLoadingScreen(args["loading_screen"].ToString(),args["loading_screen_path"].ToString());
            if (_loadingScreen == null)
            {
                GD.PushError($"Loading screen has not been found: {args["loading_screen"]}");
                return;
            }

            EmitSignal("LoadingScreenActivated",_loadingScreen.Name);
            if (!IsSafeToContinueLoading)
                await ToSignal(_loadingScreen,"ContinueLoading");
        }

        args = SetPreloadingArgs(args,sceneVars);

        if (isDebugEnabled)
            GD.Print($"DEBUG: Finished adding scene {scene}");

        Node _parent;
        if ((string)args["parent_node"] != "")
            _parent = GetParentNode((string)args["parent_node"]);
        else
            _parent = _defaultParentNode;

        _parent.AddChild(scene.Instantiate());

        Node _scene = _parent.GetChild(_parent.GetChildCount() - 1);
        _scene.ProcessMode = ProcessModeEnum.Disabled;
        currentScenes.Add(_scene.Name,false);
        _lastSettings.Add(_scene.Name,args);
        _lastVariables.Add(_scene.Name,sceneVars);

        if (!_internalScenePaths.ContainsKey(_scene.Name))
            _internalScenePaths.Add(_scene.Name,scene);

        _isLoading = false;

        EmitSignal("LoadingDone",_scene.Name,0);

        if (_loadingScreen != null)
        {
            EmitSignal("LoadingScreenDisabled",_loadingScreen.Name);
            if (!IsSafeToFinishLoading)
                await ToSignal(_loadingScreen,"FinishedLoading");
        }

        SetAfterLoadingArgs(_scene.Name,args,sceneVars);
    }

    private Control GetLoadingScreen(string screen, string pathOverride="")
    {
        if (screen == "")
            return null;

        string finalPath = (pathOverride != "" ? pathOverride : loadingScreensDefaultPath) + screen;
        Control _screen = _root.GetNodeOrNull<Control>(finalPath);

        if (_screen == null)
            GD.PushError($"Could not find loading screen {screen} at path {finalPath}");

        return _screen;
    }

    private Node GetParentNode(string path)
    {
        var _node = _root.GetNodeOrNull<Node>(path);
        if (_node == null)
        {
            GD.PushError($"Parent node could not be found in {path}! Using default parent node...");
            return _defaultParentNode;
        }

        return _node;
    }

    private Dictionary<string,Variant> SetPreparingArgs(Dictionary<string,Variant> args, Dictionary<string,Variant> sceneVars)
    {
        if (!args.ContainsKey("parent_node"))
            args.Add("parent_node","");

        if (!args.ContainsKey("loading_screen"))
        {
            string name = _defaultLoadingScreen != null ? _defaultLoadingScreen.Name : "";
            args.Add("loading_screen",name);
        }

        if (!args.ContainsKey("loading_screen_path"))
            args.Add("loading_screen_path",loadingScreensDefaultPath);

        Dictionary<string,Variant> _loadingScreenVars = args.ContainsKey("loading_screen_args") ? (Dictionary<string,Variant>)args["loading_screen_args"] : null;
        if (_loadingScreenVars != null)
            _loadingScreenVariables = _loadingScreenVars;
        else
            _loadingScreenVariables.Clear();

        if (!args.ContainsKey("subthreads"))
            args.Add("subthreads",_subthreadsEnabled);

        if (!args.ContainsKey("cache_mode"))
            args.Add("cache_mode",(int)_cacheMode);

        bool _noOverrideVars = args.ContainsKey("no_override") && (bool)args["no_override"];

        if (_noOverrideVars)
            _variables.Merge(sceneVars,false);
        else
            _variables.Merge(sceneVars,true);

        return args;
    }

    private Dictionary<string,Variant> SetPreloadingArgs(Dictionary<string,Variant> args, Dictionary<string,Variant> sceneVars)
    {
        bool _resetPrevious = args.ContainsKey("clear_all_previous") && (bool)args["clear_all_previous"];

        if (_resetPrevious)
            ClearAllScenes();

        return args;
    }

    private void SetAfterLoadingArgs(string scene, Dictionary<string,Variant> args, Dictionary<string,Variant> sceneVars)
    {
        bool _runInBackground = args.ContainsKey("run_in_background") && (bool)args["run_in_background"];
        bool _activateOnStartup = !args.ContainsKey("activate_on_startup") || (bool)args["activate_on_startup"];

        if (_runInBackground)
            RunInBackground(scene);
        else if (_activateOnStartup)
            ActivateScene(scene);
        else
            DisableScene(scene);
    }

    private bool IsOneSceneActive()
    {
        if (currentScenes.Values.ToString().Count("true") != 1)
            return false;

        return true;
    }
}
