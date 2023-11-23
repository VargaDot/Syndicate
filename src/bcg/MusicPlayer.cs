using Godot;
using Godot.Collections;

public partial class MusicPlayer : AudioStreamPlayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { MusicSelector(); }

	byte previousSong = 0;
	void MusicSelector()
	{
		byte x = 0;
		
		if (MusicList.Count <= 0) GD.PushWarning("No music exists");
		while (x == previousSong) x = (byte)GD.RandRange(0, MusicList.Count - 1);

		Stream = MusicList[x];
		previousSong = x;
		
		Play();
	}

	void WhenFinished()
	{
		Stop();
		MusicSelector();
	}

	[ExportGroup("Music")]
	[Export] Array<AudioStream> MusicList = new();

	[ExportGroup("Sound effects")]
	[Export] Array<AudioStream> SfxList = new();
}
