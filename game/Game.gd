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
	$Timer.start()

func _on_timer_timeout():
	_turnManager()

enum GAME_STATES {IDLE, ROLL, PRISON, INSPECT, BANKRUPT, DONE}
var currentGame = GAME_STATES.IDLE

func _process(_delta):
	if Input.is_action_pressed("Quit"):
		emit_signal("RequestPause")
	elif currentGame == GAME_STATES.ROLL:
		if Input.is_action_pressed("Confirm") or Input.is_action_just_pressed("Roll"):
			_diceManager()
			print(activePlayer, " rolled")
		else: pass
	else: pass

enum DISTRICT_TYPE {GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING}

var firstRound:bool = true
var activePlayer:int = 0 # Current player in turn
var agentList:PackedByteArray = Khana.GetAgentIDs()

func _turnManager():
	if !firstRound:
		activePlayer += 1 % (agentList.size() - 1)
		activePlayer = agentList[activePlayer]
		firstRound = false
	else:
		if agentList.size() > 0:
			var randomIndex = randi_range(0, agentList.size() - 1)
			activePlayer = agentList[randomIndex]
		else:
			print("agentList is empty")
	print(activePlayer, " is playing")
	
#region Prison Check
	if Khana.GetAgentStatus(activePlayer) == true:
		currentGame = GAME_STATES.PRISON
		emit_signal("RequestPrison")
		print(activePlayer, " is in prison")
	print(activePlayer, " is not jailed")
	
	currentGame = GAME_STATES.ROLL
#endregion

func _diceManager():
	currentGame = GAME_STATES.IDLE
	var dice1 = randi_range(1, 6)
	var dice2 = randi_range(1, 6)
	var roll = dice1 + dice2
	var flagged:bool = false
	
	if dice1 != dice2: Khana.ModifyDoubleCount(activePlayer, false)
	else:
		flagged = true
		Khana.ModifyDoubleCount(activePlayer, true)
		
		if Khana.GetAgentDoublesCount(activePlayer) == 3: 
			Khana.ToggleAgentFreedom(activePlayer)
			Khana.ModifyDoubleCount(activePlayer, false)
			
			_on_next_turn_pressed()
			print(activePlayer, " has been imprisoned")
		else:
			print(activePlayer, " is in risk of going to jail")
	
	emit_signal("RequestDice", dice1, dice2, flagged)
	print("dice rolled! ", roll)
	
	Khana.MoveAgent(activePlayer, roll)
	print(activePlayer, " advanced to ", Khana.GetAgentPosition(activePlayer))
	
func _tileInspector():
	var activePlayerPos:int = Khana.GetAgentPosition(activePlayer)
	print(activePlayerPos)
	var tileType:int = EstateCourt._fetch_tile_type(activePlayerPos)
	print(tileType)
	var debtor:int = 257
	currentGame = GAME_STATES.INSPECT
	
	match tileType:
		DISTRICT_TYPE.GO:
			Khana.ConductTransaction(activePlayer, 200)
			print(activePlayer, " has collected 200")
		DISTRICT_TYPE.PROPERTY:
			var propOwner:int = Khana.CheckForOwnership(activePlayerPos)
			if propOwner == 69:
				emit_signal("RequestCard", "PROP", activePlayerPos, activePlayer)
			elif propOwner == activePlayer:
				pass
			else:
				if Khana.GetMortgageStatus(propOwner, activePlayerPos) == true:
					pass
				else:
					var propPrice:int = EstateCourt._fetch_property_rent(
						activePlayerPos, Khana.GetUpgradeLevel(activePlayer, activePlayerPos)
					)
					Khana.ConductTransaction(activePlayer, -propPrice)
					Khana.ConductTransaction(propOwner, propPrice)
					debtor = propOwner
		DISTRICT_TYPE.CHEST:
			emit_signal("RequestCard", "CHEST")
		DISTRICT_TYPE.CHANCE:
			emit_signal("RequestCard", "CHANCE")
		DISTRICT_TYPE.ITAX:
			Khana.ConductTransaction(activePlayer, -roundi(Khana.GetAgentCash() * 0.1))
		DISTRICT_TYPE.LTAX:
			Khana.ConductTransaction(activePlayer, -100)
		DISTRICT_TYPE.JAIL:
			pass
		DISTRICT_TYPE.GOJAIL:
			Khana.ToggleAgentFreedom(activePlayer)
		DISTRICT_TYPE.PARKING:
			pass
		_:
			print("not found")
#endregion
	
#region Bankruptcy check
	if Khana.GetAgentCash(activePlayer) < 0:
		currentGame = GAME_STATES.BANKRUPT
		emit_signal("Bankrupt", activePlayer, debtor)
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
	Khana.RemoveAgent(activePlayer)
	agentList.remove_at(activePlayer)
	if agentList.size() == 1:
		emit_signal("RequestWin", activePlayer)
	currentGame = GAME_STATES.IDLE

func _on_release_status_sent(released):
	if released == true: currentGame = GAME_STATES.INSPECT
	if released == false: currentGame = GAME_STATES.IDLE
