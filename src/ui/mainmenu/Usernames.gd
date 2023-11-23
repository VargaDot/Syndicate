extends HFlowContainer

var namesEntered:int = 0

func _onChange(new_text):
	$popup.text = "Confirm " + new_text + " by pressing ENTER on the field"
	_checkTimer()

func _onTextRejected(rejected_substring):
	$popup.text = rejected_substring + ", is an invalid name"
	_checkTimer()

func _onSubmit(new_text):
	namesEntered = namesEntered + 1
	$popup.text = new_text + ", has been submitted"
	_checkTimer()

# function to restart the timer if its already working
func _checkTimer():
	if $Timer.is_stopped():
		$popup.show()
		$Timer.start()
	else: 
		$Timer.stop()
		_checkTimer()

func _onTimeout(): $popup.hide()

signal sendCharacterRequest(agentCount)
var username:String = ""
func _on_play_button_pressed():
	if namesEntered >= 2:
		username = $AgentName1.text
		Khana.AddAgent(1, 2500, username)
		username = $AgentName2.text
		Khana.AddAgent(2, 2500, username)
	elif namesEntered >= 3: 
		username = $AgentName3.text
		Khana.AddAgent(3, 2500, username)
	elif namesEntered >= 4: 
		username = $AgentName4.text
		Khana.AddAgent(4, 2500, username)
	else: 
		$popup.text = "You need to submit atleast 2 names" 
		_checkTimer()
	
	emit_signal("sendCharacterRequest", namesEntered)
