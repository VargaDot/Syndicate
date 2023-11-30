extends Control

signal continue_loading
signal finished_loading

var loading_failed: bool = false

func _ready():
	Composer.connect("loading_value_updated",_on_loading_value_updated)
	Composer.connect("loading_done",_on_loading_done)
	Composer.connect("loading_screen_activated",activate)

	hide()
	process_mode = Node.PROCESS_MODE_DISABLED

func activate(scene_name):
	if name != scene_name:
		return

	process_mode = Node.PROCESS_MODE_INHERIT
	show()
	set_process_input(false)
	Composer.is_safe_to_continue = true
	continue_loading.emit()

func disable(scene_name):
	if name != scene_name:
		return

	hide()
	set_process_input(false)
	finished_loading.emit()
	process_mode = Node.PROCESS_MODE_DISABLED

func _on_loading_value_updated(value: float):
	$VBoxContainer/ProgressBar.value = value * 100

func _on_loading_done(_scene_name: String, error_code):
	set_process_input(true)

	if error_code != 0:
		loading_failed = true
		$VBoxContainer/Result.text = "Loading failed! Press 'Enter' to continue."
		return

	$VBoxContainer/Result.text = "Loading Done! Press 'Enter' to continue."

func _input(event):
	if event.is_action_pressed("ui_accept"):
		if loading_failed:
			get_tree().quit()
		else:
			disable(name)
