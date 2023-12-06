extends Control

@export var singlePlayer:Panel
@onready var animPlyr:AnimationPlayer = $AnimationPlayer

func _process(_delta):
	if Input.is_action_pressed("Quit"): get_tree().quit()
	if Input.is_action_just_pressed("SkipAnimation") && animPlyr.is_playing(): animPlyr.seek(1, true)

func _singlePlayerPressed() -> void:
	if $SingleplayerMenu.visible: 
		animPlyr.play("HideSingleplayer")
		$SingleplayerMenu.hide()
	else:
		$SingleplayerMenu.show()
		animPlyr.play("ShowSingleplayer")

func _multiPressed() -> void:
	pass

func _on_completion():
	Composer.disable_scene("MainMenu")
	Composer.activate_scene("Game")

func _on_credits_pressed():
	if $Credits.visible:
		animPlyr.play("HideCredits")
		$Credits.hide()
	else: 
		animPlyr.play("ShowCredits")
		$Credits.show()
