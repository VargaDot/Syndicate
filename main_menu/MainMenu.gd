extends Control

@export var singlePlayer:Panel
@onready var animPlyr:AnimationPlayer = $AnimationPlayer

func _process(_delta):
	if Input.is_action_pressed("Quit"):
		get_tree().quit()
	if Input.is_action_just_pressed("SkipAnimation") && animPlyr.is_playing():
		animPlyr.seek(1, true)

func _singlePlayerPressed() -> void:
	if $SingleplayerMenu.visible: 
		animPlyr.play("HideSingleplayer")
		while animPlyr.is_playing():
			pass
		$SingleplayerMenu.hide()
	else:
		$SingleplayerMenu.show()
		animPlyr.play("ShowSingleplayer")

func _multiPressed() -> void:
	pass

signal PlayPressed
func _on_completion():
	emit_signal("PlayPressed")

func _on_credits_pressed():
	if $Credits.visible:
		animPlyr.play("HideCredits")
		while animPlyr.is_playing():
			pass
		$Credits.hide()
	else: 
		animPlyr.play("ShowCredits")
		$Credits.show()
