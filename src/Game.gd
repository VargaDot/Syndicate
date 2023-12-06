extends Node2D

signal SendUIRequest

func _ready(): _turnManager()
func _process(_delta): if Input.is_action_pressed("Quit"): emit_signal("SendUIRequest", "PAUSE")

var firstRound:bool = true
var currentPlayer:int = 0
var agentCount:int
func _turnManager():
	agentCount = Khana.AgentCount()
	
	if firstRound: 
		currentPlayer = randi_range(1, agentCount)
		firstRound = false
	else: currentPlayer = (currentPlayer + 1) % agentCount
	
	if agentCount == 1: emit_signal("SendUIRequest", "WON", currentPlayer)
	else: pass
	
	if Khana.GetAgentStatus(currentPlayer) == true: emit_signal("SendUIRequest", "PRISON")
	else: pass
	
	var roll = randi_range(2, 12)
	if roll % 2 == 0: Khana.ModifyDoubleCount(currentPlayer, false)
	else: 
		Khana.ModifyDoubleCount(currentPlayer, true)
		if Khana.GetAgentDoublesCount() == 3: 
			Khana.ToggleAgentFreedom(currentPlayer)
			Khana.ModifyDoubleCount(currentPlayer, false)
		else: pass
	
	emit_signal("SendUIRequest", "DICE", roll)
	Khana.MoveAgent(currentPlayer, roll)
	_tileInspector()

enum districtTypes { GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING }

func _tileInspector(): 
	var pos:int = Khana.GetAgentPosition(currentPlayer)
	var tiletype = EstateCourt.FetchDistrictData(pos, "TYPE")
	
	match tiletype:
		districtTypes.GO: Khana.ConductTransaction(currentPlayer, 200)
		districtTypes.PROPERTY:
			var propowner:int = Khana.CheckForOwnership(pos)
			if propowner == 69: emit_signal("SendUIRequest", "PROP", pos, currentPlayer)
			else: 
				if Khana.GetMortgageStatus(owner, pos) == true: pass
				else: 
					var proplevel:int = Khana.GetUpgradeLevel(currentPlayer, pos)
					var internalName:String = EstateCourt.FetchDistrictData(pos, "NAME")
					var propPrice:int = EstateCourt.FetchAssetData(internalName, "RENT", proplevel)
					Khana.ConductTransaction(currentPlayer, -propPrice)
					Khana.ConductTransaction(propowner, proplevel)
		districtTypes.CHEST: emit_signal("SendUIRequest", "CHEST")
		districtTypes.CHANCE: emit_signal("SendUIRequest", "CHANCE")
		districtTypes.ITAX: Khana.ConductTransaction(currentPlayer, -roundi(Khana.GetAgentCash() * 0.1))
		districtTypes.LTAX: Khana.ConductTransaction(currentPlayer, -100)
		districtTypes.JAIL: pass
		districtTypes.GOJAIL: Khana.ToggleAgentFreedom(currentPlayer)
		districtTypes.PARKING: pass
		_: print("not found")
	
