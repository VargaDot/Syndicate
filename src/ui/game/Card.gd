extends Control

func _onRequest(funcID, secondOp):
	match(funcID):
		"PROP": # BUYING PROPERTIES
			EstateCourt.FetchDistrictData(secondOp, "ID")
		"CHEST": pass
		"CHANCE": pass
	
	show()
