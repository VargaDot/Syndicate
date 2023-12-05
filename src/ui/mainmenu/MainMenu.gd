extends Control

@export var singlePlayer:Panel
@onready var animPlyr:AnimationPlayer = $AnimationPlayer

func _process(_delta):
	if Input.is_action_pressed("Quit"): get_tree().quit()
	if Input.is_action_just_pressed("SkipAnimation") && animPlyr.is_playing(): animPlyr.seek(1, true)

var singleShown:bool = false
func _singlePlayerPressed() -> void:
	if singleShown: animPlyr.play("HideSingleplayer")
	else: animPlyr.play("ShowSingleplayer")
	singleShown = !singleShown

func _multiPressed() -> void:
	pass

func _on_completion():
	Composer.disable_scene("MainMenu")
	Composer.activate_scene("Game")

var creditsShown:bool = false
func _on_credits_pressed():
	if creditsShown: animPlyr.play("ShowCredits")
	else: animPlyr.play("HideCredits")
	creditsShown = !creditsShown
