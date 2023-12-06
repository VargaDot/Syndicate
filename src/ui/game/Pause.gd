extends Control

func _onRequest():
	if visible: hide()
	else: show()

func _on_save_pressed():
	pass # Replace with function body.

func _on_load_pressed():
	pass # Replace with function body.

func _on_soft_quit_pressed():
	Composer.disable_scene("Game")
	Composer.activate_scene("MainMenu")

func _on_hard_quit_pressed():
	get_tree().quit()
