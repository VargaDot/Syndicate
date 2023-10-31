extends Node

func _ready():
	var BoardFILE = FileAccess.open("data/Board.JSON", FileAccess.READ)
	var PropertyFILE = FileAccess.open("data/Properties.JSON", FileAccess.READ)
	BoardDATA = JSON.parse_string(BoardFILE.get_as_text())
	PropertyDATA = JSON.parse_string(PropertyFILE.get_as_text())

static var BoardDATA
static func FetchDistrictData(BoardID, functionID):
	var data
	match functionID:
		1: data = BoardDATA[BoardID]["Type"]
		2: data = BoardDATA[BoardID]["CardID"]
		3: data = BoardDATA[BoardID]["Name"]
		4: data = BoardDATA[BoardID]["ID"]
		_: printerr("unrelated")
	
	print(data)
	return data

static var PropertyDATA
static func FetchAssetData(internalPropName, functionID, level = 0):
	var data
	match functionID:
		1: data = PropertyDATA[internalPropName]["Costs"]["PurchasePrice"]
		2: data = PropertyDATA[internalPropName]["Costs"]["BuildCost"]
		3: data = PropertyDATA[internalPropName]["Costs"]["Mortgage"]
		4: data = PropertyDATA[internalPropName]["Rent"][level]
		_: data = PropertyDATA["Default"]
	
	print(data)
	return data
