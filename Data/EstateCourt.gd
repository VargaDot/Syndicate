extends Node

func _ready():
	var BoardFILE = FileAccess.open("data/Board.JSON", FileAccess.READ)
	var PropertyFILE = FileAccess.open("data/Properties.JSON", FileAccess.READ)
	BoardDATA = JSON.parse_string(BoardFILE.get_as_text())
	PropertyDATA = JSON.parse_string(PropertyFILE.get_as_text())

var BoardDATA
func FetchDistrictData(BoardID, functionID):
	match functionID:
		1: return BoardDATA[BoardID]["Type"]
		2: return BoardDATA[BoardID]["CardID"]
		3: return BoardDATA[BoardID]["Name"]
		4: return BoardDATA[BoardID]["ID"]
		_: printerr("unrelated")

var PropertyDATA
func FetchAssetData(internalPropName, functionID, level = 0):
	match functionID:
		1: return PropertyDATA[internalPropName]["Costs"]["PurchasePrice"]
		2: return PropertyDATA[internalPropName]["Costs"]["BuildCost"]
		3: return PropertyDATA[internalPropName]["Costs"]["Mortgage"]
		4: return PropertyDATA[internalPropName]["Rent"][level]
		_: return PropertyDATA["Default"]
