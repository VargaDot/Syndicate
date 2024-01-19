extends Node

func _ready():
	ComposerGD.AddScene("MainMenu", "res://src/MainMenu.tscn", {
		"instant_load":true,
		"instant_create":true,
		"scene_parent":self
	})
