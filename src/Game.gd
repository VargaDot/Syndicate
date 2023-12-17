extends Node2D

signal RequestDice()
signal RequestCard()
signal RequestPause()
signal RequestTrade()
signal RequestPrison()
signal RequestWin()
signal RequestLoss()

func _ready(): _turnManager()
func _process(_delta): if Input.is_action_pressed("Quit"): emit_signal("RequestPause")

var firstRound:bool = true
var currentPlayer:int = 0
var agentCount:int
func _turnManager():
	agentCount = Khana.AgentCount()
	
	if firstRound: 
		currentPlayer = randi_range(1, agentCount)
		firstRound = false
	else: currentPlayer = (currentPlayer + 1) % agentCount
	
	if agentCount == 1: emit_signal("RequestWin", currentPlayer)
	else: pass
	
	if Khana.GetAgentStatus(currentPlayer) == true: emit_signal("RequestPrison")
	else: pass
	
	var roll = randi_range(2, 12)
	if roll % 2 == 0: Khana.ModifyDoubleCount(currentPlayer, false)
	else: 
		Khana.ModifyDoubleCount(currentPlayer, true)
		if Khana.GetAgentDoublesCount(currentPlayer) == 3: 
			Khana.ToggleAgentFreedom(currentPlayer)
			Khana.ModifyDoubleCount(currentPlayer, false)
		else: pass
	
	emit_signal("RequestDice", roll)
	Khana.MoveAgent(currentPlayer, roll)
	_tileInspector()

enum districtTypes { GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING }

func _tileInspector(): 
	var pos:int = Khana.GetAgentPosition(currentPlayer)
	var tiletype = EstateCourt.FetchDistrictData(pos, "TYPE")
	var bankrupter:int = 257
	
	match tiletype:
		districtTypes.GO: Khana.ConductTransaction(currentPlayer, 200)
		districtTypes.PROPERTY:
			var propowner:int = Khana.CheckForOwnership(pos)
			if propowner == 69: emit_signal("RequestCard", "PROP", pos, currentPlayer)
			else:
				if Khana.GetMortgageStatus(propowner, pos) == true: pass
				else: 
					var proplevel:int = Khana.GetUpgradeLevel(currentPlayer, pos)
					var internalName:String = EstateCourt.FetchDistrictData(pos, "NAME")
					var propPrice:int = EstateCourt.FetchAssetData(internalName, "RENT", proplevel)
					Khana.ConductTransaction(currentPlayer, -propPrice)
					Khana.ConductTransaction(propowner, proplevel)
					bankrupter = propowner
		districtTypes.CHEST: emit_signal("RequestCard", "CHEST")
		districtTypes.CHANCE: emit_signal("RequestCard", "CHANCE")
		districtTypes.ITAX: Khana.ConductTransaction(currentPlayer, -roundi(Khana.GetAgentCash() * 0.1))
		districtTypes.LTAX: Khana.ConductTransaction(currentPlayer, -100)
		districtTypes.JAIL: pass
		districtTypes.GOJAIL: Khana.ToggleAgentFreedom(currentPlayer)
		districtTypes.PARKING: pass
		_: print("not found")
	
	_check_for_bankruptcy(bankrupter)

signal file_bankruptcy()
func _check_for_bankruptcy(bankrupter):
	if Khana.GetAgentCash(currentPlayer) < 0:
		emit_signal("file_bankruptcy", currentPlayer, bankrupter)
		Khana.RemoveAgent(currentPlayer)
	else: pass 
