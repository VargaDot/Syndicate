extends Node2D

signal SendUIRequest

func _ready(): _turnManager()
func _process(_delta): if Input.is_action_pressed("Quit"): emit_signal("SendUIRequest", 0)

var firstRound:bool = true
var currentPlayer:int = 0
var agentCount = Khana.AgentCount()
func _turnManager():
	if firstRound: 
		currentPlayer = randi_range(1, agentCount)
		firstRound = false
	else: currentPlayer = (currentPlayer + 1) % agentCount
	
	var roll = randi_range(2, 12)
	if roll % 2 == 0: Khana.ModifyDoubleCount(currentPlayer)
	else: 
		Khana.ModifyDoubleCount(currentPlayer, true)
		if Khana.GetAgentDoublesCount() == 3: 
			emit_signal("SendUIRequest", 6)
			Khana.ToggleAgentFreedom(currentPlayer)
		else: pass
	
	emit_signal("SendUIRequest", 5, roll)
	Khana.MoveAgent(currentPlayer, roll)
	_tileInspector()

enum districtTypes { GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING }

func _tileInspector(): 
	var pos:int = Khana.GetAgentPosition(currentPlayer)
	var tiletype = EstateCourt.FetchDistrictData(pos, 1)
	
	match tiletype:
		districtTypes.GO: Khana.ConductTransaction(currentPlayer, 200)
		districtTypes.PROPERTY: emit_signal("SendUIRequest", 1, pos)
		districtTypes.CHEST: emit_signal("SendUIRequest", 2)
		districtTypes.CHANCE: emit_signal("SendUIRequest", 3)
		districtTypes.ITAX: Khana.ConductTransaction(currentPlayer, -roundi(Khana.GetAgentCash() * 0.1))
		districtTypes.LTAX: Khana.ConductTransaction(currentPlayer, -100)
		districtTypes.JAIL: pass
		districtTypes.GOJAIL: Khana.ToggleAgentFreedom()
		districtTypes.PARKING: pass
		_: print("not found")
	
