extends Control
 
func _onRequest(diceRoll):
	if diceRoll > 12: printerr("Invalid dice roll outcome")
	
	# X represents the frames for AnimatedSprite2D
	var x = diceRoll - 2
	x /= 2
	
	var result
	if x % 1 != 0: result = x + 1
	
	_playAnimation()

func _playAnimation():
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
		
		ranOnce = false
