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
	var id = EstateCourt.FetchDistrictData(boardID, "CARDID")
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
		internalName = EstateCourt.FetchDistrictData(boardID, "ID")
		$CardSprite/Title.text = EstateCourt.FetchDistrictData(boardID, "NAME")
		$CardSprite/Price.text = EstateCourt.FetchAssetData(internalName, "PP")
		$CardSprite/ValuesContainer/Mortgage.text = EstateCourt.FetchAssetData(internalName, "MORTGAGE")
		$CardSprite/ValuesContainer/Build.text = EstateCourt.FetchAssetData(internalName, "BC")
		for element in values: element.text = EstateCourt.FetchAssetData(internalName, "RENT", element)
	else: pass
	
	show()

func _displayChestCard(boardID, agentID): pass

func _displayChanceCard(boardID, agentID): pass

var internalName
func _on_purchase_button_pressed():
	var price = EstateCourt.FetchAssetData(internalName, "PP")
	Khana.ConductTransaction(agent, price)
	Khana.AddProperty(agent, propID)
	hide()

signal startAuction()
func _on_auction_button_pressed():
	emit_signal("startAuction", agent, internalName)
