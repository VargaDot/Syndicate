extends Node

func _ready():
	var BoardFILE = FileAccess.open("data_manager/Board.JSON", FileAccess.READ)
	BoardDATA = JSON.parse_string(BoardFILE.get_as_text())

var BoardDATA
func _fetch_tile_type(BoardID):
	return BoardDATA[BoardID]["Type"]

func _fetch_card_id(BoardID):
	return BoardDATA[BoardID]["CardID"]

func _fetch_property_name(BoardID):
	return BoardDATA[BoardID]["Name"]

func _fetch_property_price(BoardID):
	return BoardDATA[BoardID]["Costs"]["PurchasePrice"]

func _fetch_property_mortgage(BoardID):
	return BoardDATA[BoardID]["Costs"]["Mortgage"]

func _fetch_property_buildcost(BoardID):
	return BoardDATA[BoardID]["Costs"]["BuildCost"]

func _fetch_property_rent(BoardID, level = 0):
	return BoardDATA[BoardID]["Rent"][level]
