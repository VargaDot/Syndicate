extends Node2D

signal RequestDice()
signal RequestCard()
signal RequestPause()
signal RequestTrade()
signal RequestPrison()
signal RequestWin()
signal RequestLoss()
signal Bankrupt()

func _ready():
	print("HI")

func _process(_delta): 
	if Input.is_action_pressed("Quit"):
		emit_signal("RequestPause")

enum GAME_STATES {IDLE, ROLL, PRISON, INSPECT, BANKRUPT, DONE}
var currentGame = GAME_STATES.IDLE

enum DISTRICT_TYPE {GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING}

var firstRound:bool = true
var currentPlayer:int = 0
var agentList:PackedByteArray = Khana.GetAgentIDs()

func _turnManager():
#region TurnManager
	if firstRound:
		# Check if the array is not empty
		if agentList.size() > 0:
			# Generate a random index within the valid range
			var randomIndex = randi_range(0, agentList.size() - 1)
			currentPlayer = agentList[randomIndex]
		else:
			print("PackedByteArray is empty")
		firstRound = false
	else:
		currentPlayer += 1 % (agentList.size() - 1)
		currentPlayer = agentList[currentPlayer]
#endregion
	
#region Prison Check
	if Khana.GetAgentStatus(currentPlayer) == true:
		currentGame = GAME_STATES.PRISON
		emit_signal("RequestPrison")
		while currentGame == GAME_STATES.PRISON: 
			pass
#endregion
	
	while currentGame == GAME_STATES.IDLE:
		if Input.is_action_pressed("Confirm"): 
			currentGame = GAME_STATES.ROLL
		else: 
			pass
	
#region Dice roll & Doubles Check
	var roll = randi_range(2, 12)
	if roll % 2 != 0: Khana.ModifyDoubleCount(currentPlayer, false)
	else:
		Khana.ModifyDoubleCount(currentPlayer, true)
		if Khana.GetAgentDoublesCount(currentPlayer) == 3: 
			Khana.ToggleAgentFreedom(currentPlayer)
			Khana.ModifyDoubleCount(currentPlayer, false)
			_on_next_turn_pressed()
		else:
			pass
#endregion
	
	emit_signal("RequestDice", roll)
	Khana.MoveAgent(currentPlayer, roll)
	
#region TileInspector
	var boardPos:int = Khana.GetAgentPosition(currentPlayer)
	var tileType = EstateCourt._fetch_tile_type(boardPos)
	var debtor:int = 257
	currentGame = GAME_STATES.INSPECT
	
	match tileType:
		DISTRICT_TYPE.GO:
			Khana.ConductTransaction(currentPlayer, 200)
		DISTRICT_TYPE.PROPERTY:
			var propOwner:int = Khana.CheckForOwnership(boardPos)
			if propOwner == 69:
				emit_signal("RequestCard", "PROP", boardPos, currentPlayer)
			else:
				if Khana.GetMortgageStatus(propOwner, boardPos) == true:
					pass
				else:
					var propPrice:int = EstateCourt._fetch_property_rent(
						boardPos, Khana.GetUpgradeLevel(currentPlayer, boardPos)
					)
					Khana.ConductTransaction(currentPlayer, -propPrice)
					Khana.ConductTransaction(propOwner, propPrice)
					debtor = propOwner
		DISTRICT_TYPE.CHEST:
			emit_signal("RequestCard", "CHEST")
		DISTRICT_TYPE.CHANCE:
			emit_signal("RequestCard", "CHANCE")
		DISTRICT_TYPE.ITAX:
			Khana.ConductTransaction(currentPlayer, -roundi(Khana.GetAgentCash() * 0.1))
		DISTRICT_TYPE.LTAX:
			Khana.ConductTransaction(currentPlayer, -100)
		DISTRICT_TYPE.JAIL:
			pass
		DISTRICT_TYPE.GOJAIL:
			Khana.ToggleAgentFreedom(currentPlayer)
		DISTRICT_TYPE.PARKING:
			pass
		_:
			print("not found")
#endregion
	
#region Bankruptcy check
	if Khana.GetAgentCash(currentPlayer) < 0:
		currentGame = GAME_STATES.BANKRUPT
		emit_signal("Bankrupt", currentPlayer, debtor)
		while currentGame == GAME_STATES.BANKRUPT:
			pass
#endregion
	
	currentGame = GAME_STATES.DONE
	$UI/NextTurn.show()
	while currentGame == GAME_STATES.DONE:
		if Input.is_action_pressed("Confirm"):
			_on_next_turn_pressed()
		else:
			pass

func _on_next_turn_pressed():
	currentGame = GAME_STATES.IDLE
	_turnManager()
	$UI/NextTurn.hide()

func _on_restructured():
	currentGame = GAME_STATES.IDLE

func _on_defaulted():
	Khana.RemoveAgent(currentPlayer)
	agentList.remove_at(currentPlayer)
	if agentList.size() == 1:
		emit_signal("RequestWin", currentPlayer)
	currentGame = GAME_STATES.IDLE

func _on_release_status_sent(released):
	if released == true: currentGame = GAME_STATES.INSPECT
	if released == false: currentGame = GAME_STATES.IDLE
