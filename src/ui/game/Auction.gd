extends Control

var bidders
var auctionedProp
var currentAuctionner:int = 1
func _on_start_auction(internalName):
	bidders = Khana.AgentCount()
	auctionedProp = EstateCourt.FetchAssetData(internalName, "PP")
	_turnManager()

func _turnManager():
	
	
	currentAuctionner = (currentAuctionner + 1) % bidders 

func _on_bid_1_pressed():
	pass # Replace with function body.

func _on_bid_10_pressed():
	pass # Replace with function body.

func _on_bid_100_pressed():
	pass # Replace with function body.

func _on_fold_pressed():
	pass # Replace with function body.
