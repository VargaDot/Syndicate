extends Control
 
func _onRequest(diceRoll):
	if diceRoll > 12: printerr("Invalid dice roll outcome")
	
	var x = diceRoll - 2
	if x == 0: pass
	else: x / 2
	
	var result
	if x % 1 != 0: result = x + 1
	
	show()
	$Dice1.play("rolling")
	$Dice2.play("rolling")
