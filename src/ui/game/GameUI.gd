extends Node

func _on_game_request_card(functionID, boardID, agentID):
	match functionID:
		"PROP": $Card._displayPropCard(boardID, agentID)
		"CHEST": $Card._displayChestCard(boardID, agentID)
		"CHANCE": $Card._displayChanceCard(boardID, agentID)

func _on_game_request_dice(diceroll): $Dice._onRequest(diceroll)

func _on_game_request_loss(loserID): $Lose._on_request_loss(loserID)

func _on_game_request_pause(): $Pause._onRequest()

func _on_game_request_prison(agentID): $Prison._onRequest(agentID)

func _on_game_request_trade(buyerID, sellerID): $Trade._onRequest(buyerID, sellerID)

func _on_game_request_win(winnerID): $Win._on_request_win(winnerID)
