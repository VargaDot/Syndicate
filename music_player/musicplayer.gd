extends AudioStreamPlayer

@export var musicList:Array[AudioStream] = []

func _ready():
	_musicSelector()

var lastsong:int = 0
func _musicSelector():
	var x:int
	
	if musicList.size() == 0: print("No music exists")
	while x == lastsong: randi_range(0, musicList.size())
	
	stream = musicList[x]
	lastsong = x
	play(0)

func _on_finished():
	_musicSelector()
