extends Control

var die1
var die2
func _on_game_request_dice(dice1, dice2, flagged):
	die1 = dice1 - 1
	die2 = dice2 - 1
	_launchTimer()
	
	show()
	%Dice1.play("rolling")
	%Dice2.play("rolling")
	%Dice1.frame = randi_range(0,5)
	%Dice2.frame = randi_range(0,5)

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
		%Dice1.frame = die1
		%Dice2.frame = die2
		ranOnce = false
	
