extends Control

func _on_game_request_pause():
	show()

func _on_save_pressed():
	pass # Replace with function body.

func _on_load_pressed():
	pass # Replace with function body.

signal MainMenuPressed
func _on_soft_quit_pressed():
	ComposerGD.ReplaceScene("Game", "MainMenu", self)

func _on_hard_quit_pressed():
	get_tree().quit()
