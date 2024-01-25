extends Node

func _ready():
	Composer.add_scene_from_string("MainMenu","preset1")

	await Composer.loading_done

	Composer.add_scene_from_string("Game",{
		"activate_on_startup":false,
	})
