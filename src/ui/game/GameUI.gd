extends Node

signal RequestDice
signal RequestCard
signal RequestPause
signal RequestTrade
signal RequestPrison

func _onRequest(functionID):
	match functionID:
		0: emit_signal("RequestPause")
		1: emit_signal("RequestCard", 1)
		2: emit_signal("RequestCard", 2)
		3: emit_signal("RequestCard", 3)
		4: emit_signal("RequestCard", 4)
		5: emit_signal("RequestDice")
		6: emit_signal("RequestPrison")
		7: emit_signal("RequestTrade")
