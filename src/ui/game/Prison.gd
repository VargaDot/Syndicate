extends Control

signal sendReleaseStatus()

var currentPrisoner:int
func _on_game_request_prison(agentID):
	currentPrisoner = agentID
	show()
	$AnimationPlayer.play("Show")

func _on_bail_pressed():
	Khana.ConductTransaction(currentPrisoner, -50)
	emit_signal("sendReleaseStatus", true)

func _on_roll_pressed():
	var roll = randi_range(2, 12)
	emit_signal("RequestDice", roll)
	
	if roll % 2 != 0: emit_signal("sendReleaseStatus", false)
	else: emit_signal("sendReleaseStatus", true)
