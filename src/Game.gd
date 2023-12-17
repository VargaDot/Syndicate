extends Node2D

signal RequestDice()
signal RequestCard()
signal RequestPause()
signal RequestTrade()
signal RequestPrison()
signal RequestWin()
signal RequestLoss()

func _ready(): _turnManager()
func _process(_delta): 
	if Input.is_action_pressed("Quit"): emit_signal("RequestPause")
	match currentGame:
		GAME_STATES.ROLL: pass
		GAME_STATES.BANKRUPT: pass
		GAME_STATES.COMPLETE: pass
		_: pass

enum GAME_STATES {NORMAL, ROLL, BANKRUPT, COMPLETE}
var currentGame = GAME_STATES.NORMAL

enum DISTRICT_TYPE { GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING }

var firstRound:bool = true
var currentPlayer:int = 0
var agentCount:int
func _turnManager():
	agentCount = Khana.AgentCount() # Create some sort of array or list instead
	
	if firstRound:
		currentPlayer = randi_range(1, agentCount)
		firstRound = false
	else: currentPlayer = (currentPlayer + 1) % agentCount
	
	if agentCount == 1: emit_signal("RequestWin", currentPlayer)
	if Khana.GetAgentStatus(currentPlayer) == true: emit_signal("RequestPrison")
	
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
	
	var pos:int = Khana.GetAgentPosition(currentPlayer)
	var tiletype = EstateCourt.FetchDistrictData(pos, "TYPE")
	var bankrupter:int = 257
	
	match tiletype:
		DISTRICT_TYPE.GO: Khana.ConductTransaction(currentPlayer, 200)
		DISTRICT_TYPE.PROPERTY:
			var propowner:int = Khana.CheckForOwnership(pos)
			if propowner == 69: emit_signal("RequestCard", "PROP", pos, currentPlayer)
			elif propowner != 69: if Khana.GetMortgageStatus(propowner, pos) == true: pass
			else:
				var proplevel:int = Khana.GetUpgradeLevel(currentPlayer, pos)
				var internalName:String = EstateCourt.FetchDistrictData(pos, "NAME")
				var propPrice:int = EstateCourt.FetchAssetData(internalName, "RENT", proplevel)
				Khana.ConductTransaction(currentPlayer, -propPrice)
				Khana.ConductTransaction(propowner, proplevel)
				bankrupter = propowner
		DISTRICT_TYPE.CHEST: emit_signal("RequestCard", "CHEST")
		DISTRICT_TYPE.CHANCE: emit_signal("RequestCard", "CHANCE")
		DISTRICT_TYPE.ITAX: Khana.ConductTransaction(currentPlayer, -roundi(Khana.GetAgentCash() * 0.1))
		DISTRICT_TYPE.LTAX: Khana.ConductTransaction(currentPlayer, -100)
		DISTRICT_TYPE.JAIL: pass
		DISTRICT_TYPE.GOJAIL: Khana.ToggleAgentFreedom(currentPlayer)
		DISTRICT_TYPE.PARKING: pass
		_: print("not found")
	
	if Khana.GetAgentCash(currentPlayer) < 0:
		emit_signal("RequestLoss", currentPlayer, bankrupter)
		Khana.RemoveAgent(currentPlayer)
	
	$UI/NextTurn.show()
	if Input.is_action_pressed("Confirm"): _on_next_turn_pressed()
	

func _on_next_turn_pressed():
	_turnManager()
	$UI/NextTurn.hide()
