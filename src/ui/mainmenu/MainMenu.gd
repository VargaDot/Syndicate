extends Control

@export var singlePlayer:Panel
var animPlyr:AnimationPlayer

func _ready():
	animPlyr = $AnimationPlayer

func _process(_delta):
	if Input.is_action_pressed("Quit"): get_tree().quit()
	if Input.is_action_just_pressed("SkipAnimation") && animPlyr.is_playing(): animPlyr.seek(1, true)
	pass

var shown:bool
func _singlePlayerPressed() -> void:
	if shown: animPlyr.play("HideSingleplayer")
	else: animPlyr.play("ShowSingleplayer")
	shown = !shown

func _multiPressed() -> void:
	pass

func _playPressed() -> void:
	ComposerLib.AddScene("Game", "preset1")

func _closePressed() -> void:
	animPlyr.play("HideSingleplayer")
	shown = false
