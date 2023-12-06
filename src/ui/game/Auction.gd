extends Control

var bidders = []
func _on_start_auction(firstAgent, internalName):
	bidders = Khana.GetAgentIDs()
	bidders.remove(firstAgent)

func _on_bid_pressed():
	pass # Replace with function body.

func _on_fold_pressed():
	pass # Replace with function body.
