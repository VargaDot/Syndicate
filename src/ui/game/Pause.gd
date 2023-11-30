extends Control

func _onRequest():
	if visible: hide()
	else: show()
