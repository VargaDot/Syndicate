extends Control

@export var values:Array[Label]
enum tileColors { BROWN, CYAN, PINK, ORANGE, RED, YELLOW, GREEN, BLUE, STATION, UTILITY1, UTILITY2 }

var currentTile
var currentAgent
func _on_game_request_card(agentID, tileID):
	currentTile = tileID
	currentAgent = agentID
	
	var id = EstateCourt._fetch_card_id(tileID)
	match id:
		tileColors.BROWN: %CardSprite/CardColor.color = Color.BROWN
		tileColors.CYAN: %CardSprite/CardColor.color = Color.CYAN
		tileColors.PINK: %CardSprite/CardColor.color = Color.PINK
		tileColors.ORANGE: %CardSprite/CardColor.color = Color.ORANGE
		tileColors.RED: %CardSprite/CardColor.color = Color.RED
		tileColors.YELLOW: %CardSprite/CardColor.color = Color.YELLOW
		tileColors.GREEN: %CardSprite/CardColor.color = Color.GREEN
		tileColors.BLUE: %CardSprite/CardColor.color = Color.BLUE
		tileColors.STATION: %CardSprite.texture = "res://art/sprite/Cards/TrainCard.png"
		tileColors.UTILITY1: %CardSprite.texture = "res://art/sprite/Cards/ElectricCard.png"
		tileColors.UTILITY2: %CardSprite.texture = "res://art/sprite/Cards/WaterCard.png"
	if id <= 7:
		$CardSprite/Title.text = EstateCourt._fetch_property_name(tileID)
		print(EstateCourt._fetch_property_price(tileID))
		$CardSprite/Price.text = str(EstateCourt._fetch_property_price(tileID))
		$CardSprite/ValuesContainer/Mortgage.text = str(EstateCourt._fetch_property_mortgage(tileID))
		$CardSprite/ValuesContainer/Build.text = str(EstateCourt._fetch_property_buildcost(tileID))
		for element in values:
			element.text = str(EstateCourt._fetch_property_rent(tileID, element))
	else:
		pass
	show()

signal ChanceTriggered()
func _on_game_request_chance():
	print("HIT CHANCE")
	emit_signal("ChanceTriggered")

signal ChestTriggered()
func _on_game_request_chest():
	emit_signal("ChestTriggered")

func _on_purchase_button_pressed():
	var price = EstateCourt._fetch_property_price(currentTile)
	Khana.BuyProperty(currentAgent, currentTile, price)
	hide()

signal startAuction()
func _on_auction_button_pressed():
	emit_signal("startAuction", currentAgent, currentTile)
