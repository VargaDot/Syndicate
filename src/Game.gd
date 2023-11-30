extends Node2D

signal SendUIRequest

func _ready(): _turnManager()
func _process(_delta): if Input.is_action_pressed("Quit"): emit_signal("SendUIRequest", 0)

var firstRound:bool = true
var count:int = 0
func _turnManager():
	if firstRound: 
		count = randi_range(1, Khana.Agentcount)
		firstRound = false
	else: 
		count = count + 1
		if count > Khana.Agentcount: count = 1
	
	
