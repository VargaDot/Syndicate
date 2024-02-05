extends Node

func _ready():
	$LoadingScreen.show()
	$LoadingScreen/ProgressBar.value = 5
	ComposerGD.AddScene("MainMenu", "res://main_menu/MainMenu.tscn", {
		"instant_create":true,
		"scene_parent":self
	})
	$LoadingScreen/ProgressBar.value = 30
	await Composer.SceneCreated
	$LoadingScreen/ProgressBar.value = 35
	var mainmenu:Node = get_child(1)
	$LoadingScreen/ProgressBar.value = 45
	mainmenu.PlayPressed.connect(_on_play_pressed)
	$LoadingScreen/ProgressBar.value = 50
	ComposerGD.AddScene("Game", "res://game/Game.tscn", {
		"scene_parent":self,
	})
	$LoadingScreen/ProgressBar.value = 100
	$LoadingScreen.hide()
	$LoadingScreen/ProgressBar.value = 0

func _on_play_pressed():
	$LoadingScreen.show()
	$LoadingScreen/ProgressBar.value = 50
	Composer.ReplaceScene("MainMenu", "Game", self)
	$LoadingScreen/ProgressBar.value = 100
	$LoadingScreen.hide()
	$LoadingScreen/ProgressBar.value = 0
