extends Control
 
var dice1
var dice2
func _on_game_request_dice(diceRoll):
	if diceRoll > 12: printerr("Invalid dice roll outcome")
	
	# X represents the frames for AnimatedSprite2D
	dice1 = diceRoll - 2
	dice1 /= 2
	
	if dice1 % 1 != 0: 
		if dice1 == 6: dice2 = dice1 - 1 
		else: dice2 = dice1 + 1
	else: dice2 = dice1
	
	_launchTimer()
	
	show()
	%Dice1.play("rolling")
	%Dice2.play("rolling")

func _launchTimer():
	var t:float = randf_range(1.25,3.25)
	%Timer.wait_time = t;
	%Timer.start()

var ranOnce:bool = false
func _onTimeout():
	if ranOnce == false:
		var s:float = randf_range(0.25,0.75)
		%Dice1.speed_scale = s
		%Dice2.speed_scale = s
		
		_launchTimer()
		ranOnce = true
	else:
		%Dice1.stop()
		%Dice2.stop()
		%Dice1.frame = dice1
		%Dice2.frame = dice2
		ranOnce = false
	
