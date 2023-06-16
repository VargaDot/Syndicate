using Godot;
using Godot.Collections;
using System;

public partial class MusicPlayer : AudioStreamPlayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MusicSelector();
	}

	int previousSong;
	void MusicSelector()
	{
		int x = GD.RandRange(0, MusicList.Count);
		if(x == previousSong)
		{
			for (int i = 0; x != previousSong; i++)
			{
				x = GD.RandRange(0, MusicList.Count);
			}
		}

		Stream = MusicList[x];
		previousSong = x;
	}

	void WhenFinished()
	{
		MusicSelector();
	}

	[ExportGroup("Music")]
	[Export] Array<AudioStream> MusicList = new Array<AudioStream>();

	[ExportGroup("Sound effects")]
	[Export] Array<AudioStream> SfxList = new Array<AudioStream>();
}
