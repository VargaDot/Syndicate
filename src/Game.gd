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
var agentList:Array = Khana.GetAgentIDs()
func _turnManager():
	if firstRound:
		currentPlayer = agentList.pick_random()
		firstRound = false
	else: 
		currentPlayer += 1 % (agentList.size() - 1)
		currentPlayer = agentList[currentPlayer]
	
	if agentList.size() == 1: emit_signal("RequestWin", currentPlayer)
	
	if Khana.GetAgentStatus(currentPlayer) == true: emit_signal("RequestPrison")
	
	var roll = randi_range(2, 12)
	if roll % 2 != 0: Khana.ModifyDoubleCount(currentPlayer, false)
	else:
		Khana.ModifyDoubleCount(currentPlayer, true)
		if Khana.GetAgentDoublesCount(currentPlayer) == 3: 
			Khana.ToggleAgentFreedom(currentPlayer)
			Khana.ModifyDoubleCount(currentPlayer, false)
		else: pass
	
	emit_signal("RequestDice", roll)
	Khana.MoveAgent(currentPlayer, roll)
	
	var boardPos:int = Khana.GetAgentPosition(currentPlayer)
	var tileType = EstateCourt.FetchDistrictData(boardPos, "TYPE")
	var debtor:int = 257
	
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
		emit_signal("RequestLoss", currentPlayer, debtor)
		Khana.RemoveAgent(currentPlayer)
		agentList.remove_at(currentPlayer)
	
	$UI/NextTurn.show()
	if Input.is_action_pressed("Confirm"): _on_next_turn_pressed()
	

func _on_next_turn_pressed():
	_turnManager()
	$UI/NextTurn.hide()
