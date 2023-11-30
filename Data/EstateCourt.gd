extends Node

func _ready():
	var BoardFILE = FileAccess.open("data/Board.JSON", FileAccess.READ)
	var PropertyFILE = FileAccess.open("data/Properties.JSON", FileAccess.READ)
	BoardDATA = JSON.parse_string(BoardFILE.get_as_text())
	PropertyDATA = JSON.parse_string(PropertyFILE.get_as_text())

var BoardDATA
func FetchDistrictData(BoardID, functionID):
	match functionID:
		"TYPE": return BoardDATA[BoardID]["Type"]
		"CARDID": return BoardDATA[BoardID]["CardID"]
		"NAME": return BoardDATA[BoardID]["Name"]
		"ID": return BoardDATA[BoardID]["ID"]
		_: printerr("unrelated")

var PropertyDATA
func FetchAssetData(internalPropName, functionID, level = 0):
	match functionID:
		"PP": return PropertyDATA[internalPropName]["Costs"]["PurchasePrice"]
		"BC": return PropertyDATA[internalPropName]["Costs"]["BuildCost"]
		"MORTGAGE": return PropertyDATA[internalPropName]["Costs"]["Mortgage"]
		"RENT": return PropertyDATA[internalPropName]["Rent"][level]
		_: return PropertyDATA["Default"]
