extends Node2D

signal SendUIRequest

func _process(_delta): if Input.is_action_pressed("Quit"): emit_signal("SendUIRequest", 0)
func _ready(): _turnManager()

var firstRound:bool = true
var count:int = 0
func _turnManager():
	if firstRound: 
		count = randi_range(1, Khana.Agentcount)
		firstRound = false
	
	
	
	if count > Khana.Agentcount: count = 1
