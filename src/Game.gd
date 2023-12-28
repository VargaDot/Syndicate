extends Node2D

signal RequestDice()
signal RequestCard()
signal RequestPause()
signal RequestTrade()
signal RequestPrison()
signal RequestWin()
signal RequestLoss()
signal Bankrupt()

func _ready(): _turnManager()
func _process(_delta): 
	if Input.is_action_pressed("Quit"): emit_signal("RequestPause")

enum GAME_STATES {IDLE, ROLL, PRISON, INSPECT, BANKRUPT, DONE}
var currentGame = GAME_STATES.IDLE

enum DISTRICT_TYPE { GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING }

var firstRound:bool = true
var currentPlayer:int = 0
var agentList:Array = Khana.GetAgentIDs()
func _turnManager():
	if firstRound:
		currentPlayer = agentList.pick_random()
		firstRound = false
	else:
		currentPlayer += 1 % (agentList.size() - 1)
		currentPlayer = agentList[currentPlayer]
	
	if Khana.GetAgentStatus(currentPlayer) == true: emit_signal("RequestPrison")
	
	while currentGame == GAME_STATES.IDLE:
		if Input.is_action_pressed("Confirm"): currentGame = GAME_STATES.ROLL
		else: pass
	
	var roll = randi_range(2, 12)
	if roll % 2 != 0: Khana.ModifyDoubleCount(currentPlayer, false)
	else:
		Khana.ModifyDoubleCount(currentPlayer, true)
		if Khana.GetAgentDoublesCount(currentPlayer) == 3: 
			Khana.ToggleAgentFreedom(currentPlayer)
			Khana.ModifyDoubleCount(currentPlayer, false)
			_on_next_turn_pressed()
		else: pass
	
	emit_signal("RequestDice", roll)
	Khana.MoveAgent(currentPlayer, roll)
	
	var boardPos:int = Khana.GetAgentPosition(currentPlayer)
	var tileType = EstateCourt.FetchDistrictData(boardPos, "TYPE")
	var debtor:int = 257
	
	currentGame = GAME_STATES.INSPECT
	match tileType:
		DISTRICT_TYPE.GO: Khana.ConductTransaction(currentPlayer, 200)
		DISTRICT_TYPE.PROPERTY:
			var propOwner:int = Khana.CheckForOwnership(boardPos)
			if propOwner == 69: emit_signal("RequestCard", "PROP", boardPos, currentPlayer)
			else: if Khana.GetMortgageStatus(propOwner, boardPos) == true: pass
			else:
				var proplevel:int = Khana.GetUpgradeLevel(currentPlayer, boardPos)
				var internalName:String = EstateCourt.FetchDistrictData(boardPos, "NAME")
				var propPrice:int = EstateCourt.FetchAssetData(internalName, "RENT", proplevel)
				Khana.ConductTransaction(currentPlayer, -propPrice)
				Khana.ConductTransaction(propOwner, proplevel)
				debtor = propOwner
		DISTRICT_TYPE.CHEST: emit_signal("RequestCard", "CHEST")
		DISTRICT_TYPE.CHANCE: emit_signal("RequestCard", "CHANCE")
		DISTRICT_TYPE.ITAX: Khana.ConductTransaction(currentPlayer, -roundi(Khana.GetAgentCash() * 0.1))
		DISTRICT_TYPE.LTAX: Khana.ConductTransaction(currentPlayer, -100)
		DISTRICT_TYPE.JAIL: pass
		DISTRICT_TYPE.GOJAIL: Khana.ToggleAgentFreedom(currentPlayer)
		DISTRICT_TYPE.PARKING: pass
		_: print("not found")
	
	if Khana.GetAgentCash(currentPlayer) < 0:
		currentGame = GAME_STATES.BANKRUPT
		emit_signal("Bankrupt", currentPlayer, debtor)
		while currentGame == GAME_STATES.BANKRUPT: pass
	
	currentGame = GAME_STATES.DONE
	$UI/NextTurn.show()
	while currentGame == GAME_STATES.DONE:
		if Input.is_action_pressed("Confirm"):
			_on_next_turn_pressed()
		else: pass
	

func _on_next_turn_pressed():
	currentGame = GAME_STATES.IDLE
	_turnManager()
	$UI/NextTurn.hide()

func _on_restructured():
	currentGame = GAME_STATES.IDLE

func _on_defaulted():
	Khana.RemoveAgent(currentPlayer)
	agentList.remove_at(currentPlayer)
	if agentList.size() == 1: emit_signal("RequestWin", currentPlayer)
	currentGame = GAME_STATES.IDLE
