extends Control

func _onRequest(funcID, pos):
	match(funcID):
		1: # BUYING PROPERTIES
			EstateCourt.FetchDistrictData(pos, 4)
		2: pass
		3: pass
		4: pass
	
	show()
