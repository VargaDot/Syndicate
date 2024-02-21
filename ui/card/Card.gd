extends Control

@export var values:Array[Label]
enum tileColors { BROWN, CYAN, PINK, ORANGE, RED, YELLOW, GREEN, BLUE, STATION, UTILITY1, UTILITY2 }

var propID
var agent
func _on_game_request_card(functionID, boardID, agentID):
	propID = boardID
	agent = agentID
	
	match functionID:
		"PROP": $Card._displayPropCard(boardID, agentID)
		"CHEST": $Card._displayChestCard(boardID, agentID)
		"CHANCE": $Card._displayChanceCard(boardID, agentID)

func _displayPropCard(boardID, agentID):
	var id = EstateCourt._fetch_card_id(boardID)
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
		$CardSprite/Title.text = EstateCourt._fetch_property_name(boardID)
		$CardSprite/Price.text = EstateCourt._fetch_property_price(boardID)
		$CardSprite/ValuesContainer/Mortgage.text = EstateCourt._fetch_property_mortgage(boardID)
		$CardSprite/ValuesContainer/Build.text = EstateCourt._fetch_property_buildcost(boardID)
		for element in values: element.text = EstateCourt._fetch_property_rent(boardID, element)
	else: pass
	
	show()

func _displayChestCard(boardID, agentID): pass

func _displayChanceCard(boardID, agentID): pass

func _on_purchase_button_pressed():
	var price = EstateCourt._fetch_property_price(propID)
	Khana.ConductTransaction(agent, price)
	Khana.AddProperty(agent, propID)
	hide()

signal startAuction()
func _on_auction_button_pressed():
	emit_signal("startAuction", agent, propID)
