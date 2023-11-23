extends Container

signal requestAgentType()
func _on_character_request(agentCount):
	if agentCount >= 2: 
		$agent1.texture
		$agent2.texture
	elif agentCount > 3: $agent3.texture
	elif agentCount > 4: $agent4.texture
	
	emit_signal("requestAgentType")
