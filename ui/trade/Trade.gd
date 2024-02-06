extends Control

enum tradeState {IDLE, TRADE, RESTRUCTURE, DEFAULT}
var currentState = tradeState.IDLE

func _on_game_request_trade(buyerID, sellerID):
	show()

signal restructured()
signal defaulted()
func _on_game_bankrupt(agentID, debtor):
	currentState = tradeState.RESTRUCTURE
