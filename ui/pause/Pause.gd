extends Control

func _on_game_request_pause():
	show()

func _on_save_pressed():
	pass # Replace with function body.

func _on_load_pressed():
	pass # Replace with function body.

func _on_soft_quit_pressed():
	Khana.WipeMemory()
	ComposerGD.ReplaceScene("Game", "MainMenu", null)

func _on_hard_quit_pressed():
	get_tree().quit()
