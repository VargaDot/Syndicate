extends Control

@export var singlePlayer:Panel
@onready var animPlyr:AnimationPlayer = $AnimationPlayer

func _process(_delta):
	if Input.is_action_pressed("Quit"): get_tree().quit()
	if Input.is_action_just_pressed("SkipAnimation") && animPlyr.is_playing(): animPlyr.seek(1, true)

var shown:bool
func _singlePlayerPressed() -> void:
	if shown: animPlyr.play("HideSingleplayer")
	else: animPlyr.play("ShowSingleplayer")
	shown = !shown

func _multiPressed() -> void:
	pass

func _playPressed() -> void:
	Composer.disable_scene("MainMenu")
	Composer.activate_scene("Game")

func _closePressed() -> void:
	animPlyr.play("HideSingleplayer")
	shown = false
