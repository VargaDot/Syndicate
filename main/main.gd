extends Node

func _ready():
	ComposerGD.AddScene("MainMenu", "res://main_menu/MainMenu.tscn", {
		"instant_create":true,
		"scene_parent":self
	})
	
	await Composer.SceneCreated
	var mainmenu:Node = get_child(0)
	mainmenu.PlayPressed.connect(_on_play_pressed)
	
	ComposerGD.AddScene("Game", "res://game/Game.tscn", {
		"scene_parent":self,
	})

func _on_play_pressed():
	Composer.ReplaceScene("MainMenu", "Game", self)
