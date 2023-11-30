extends Node2D

signal SendUIRequest

func _ready(): _turnManager()
func _process(_delta): if Input.is_action_pressed("Quit"): emit_signal("SendUIRequest", 0)

var firstRound:bool = true
var currentPlayer:int = 0
func _turnManager():
	if firstRound: 
		currentPlayer = randi_range(1, Khana.Agentcount)
		firstRound = false
	else: 
		currentPlayer = currentPlayer + 1
		if currentPlayer > Khana.Agentcount: currentPlayer = 1
	
	
	emit_signal("SendUIRequest", 5, roll)
	Khana.MoveAgent(currentPlayer, roll)
