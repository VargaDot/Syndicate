extends Control

@export var values:Array[Label]
enum tileColors { BROWN, CYAN, PINK, ORANGE, RED, YELLOW, GREEN, BLUE, STATION, UTILITY1, UTILITY2 }

var actualID
func _onRequest(funcID, secondOp):
	actualID = secondOp
	match(funcID):
		"PROP": # BUYING PROPERTIES
			var id = EstateCourt.FetchDistrictData(secondOp, "CARDID")
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
				var internalName = EstateCourt.FetchDistrictData(secondOp, "ID")
				$CardSprite/Title.text = EstateCourt.FetchDistrictData(secondOp, "NAME")
				$CardSprite/Price.text = EstateCourt.FetchAssetData(internalName, "PP")
				$CardSprite/ValuesContainer/Mortgage.text = EstateCourt.FetchAssetData(internalName, "MORTGAGE")
				$CardSprite/ValuesContainer/Build.text = EstateCourt.FetchAssetData(internalName, "BC")
				for element in values: element.text = EstateCourt.FetchAssetData(internalName, "RENT", element)
		"CHEST": pass
		"CHANCE": pass
	
	show()

func _on_purchase_button_pressed():
	Khana.AddProperty()

func _on_auction_button_pressed():
	pass # Replace with function body.
