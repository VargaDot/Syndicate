extends Node

func _ready():
	ComposerGD.AddScene("MainMenu", "res://src/MainMenu.tscn", {
		"instant_load":true,
		"instant_create":true,
		"scene_parent":self
	})
	
	await ComposerGD.SceneCreated
	var mainmenu:Node = get_child(0)
	mainmenu.PlayPressed.connect(_on_play_pressed)
	
	ComposerGD.AddScene("Game", "res://src/Game.tscn", {
		"scene_parent":self,
		"disable_processing":true,
		"instant_create":true
	})

func _on_play_pressed():
	ComposerGD.ReplaceScene("MainMenu", "Game", self)
