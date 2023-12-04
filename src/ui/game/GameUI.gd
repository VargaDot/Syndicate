extends Node

signal RequestDice
signal RequestCard
signal RequestPause
signal RequestTrade
signal RequestPrison

func _onRequest(functionID, secondOption, thirdOption):
	match functionID:
		"PAUSE": emit_signal("RequestPause")
		"PROP": emit_signal("RequestCard", "PROP", secondOption, thirdOption)
		"CHEST": emit_signal("RequestCard", "CHEST", secondOption)
		"CHANCE": emit_signal("RequestCard", "CHANCE", secondOption)
		"DICE": emit_signal("RequestDice", secondOption)
		"PRISON": emit_signal("RequestPrison", secondOption)
		"TRADE": emit_signal("RequestTrade", secondOption, thirdOption)
