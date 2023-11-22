extends Node2D

signal SendUIRequest

func _process(_delta): if Input.is_action_pressed("Quit"): emit_signal("SendUIRequest", 0)

