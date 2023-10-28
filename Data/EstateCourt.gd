extends Node

const BOARD_FILEPATH:String = "Data/Board.JSON"
static var BOARDFILE = FileAccess.open(BOARD_FILEPATH, FileAccess.READ)
static var BOARDDATA = JSON.parse_string(BOARDFILE)

static func FetchDistrictData(BoardID, functionID):
	var value
	match functionID:
		1: value = BOARDDATA[BoardID]["Type"]
		2: value = BOARDDATA[BoardID]["CardID"]
		3: value = BOARDDATA[BoardID]["Name"]
		4: value = BOARDDATA[BoardID]["ID"]
		_: printerr("wetard")
	
	print(value)
	return value

const PROPERTY_FILEPATH:String = "Data/Properties.JSON"
static var PROPERTYFILE = FileAccess.open(PROPERTY_FILEPATH, FileAccess.READ)
