extends HBoxContainer

var isAgent1Player:bool = true
func _on_check_box_toggled(button_pressed):
	isAgent1Player = button_pressed

var isAgent2Player:bool
func _on_check_box_2_toggled(button_pressed):
	isAgent2Player = button_pressed

var isAgent3Player:bool
func _on_check_box_3_toggled(button_pressed):
	isAgent3Player = button_pressed

var isAgent4Player:bool
func _on_check_box_4_toggled(button_pressed):
	isAgent4Player = button_pressed

signal onCompletion()
func _on_request_agent_type():
	emit_signal("onCompletion")
