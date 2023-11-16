extends HFlowContainer

var namesEntered:int = 0

func _onChange(new_text):
	$popup.text = "Confirm your changes by pressing ENTER on the field"
	_checkTimer()

func _onTextRejected(rejected_substring):
	$popup.text = rejected_substring + ", is an invalid name"
	_checkTimer()

func _onSubmit(new_text):
	namesEntered =+ 1
	$popup.text = new_text + ", has been submitted"
	_checkTimer()

func _checkTimer():
	if $Timer.is_stopped():
		$popup.show()
		$Timer.start()
	else: 
		$Timer.stop()
		_checkTimer()

func _onTimeout(): $popup.hide()
