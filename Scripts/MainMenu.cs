using Godot;
using System;

///<Summary>
///The script that's responsible for running the main menu button functions.
///</Summary>
public partial class MainMenu : Control
{

	AnimationPlayer player;

	public override void _Ready()
	{
		player = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public override void _Process(double delta)
	{
		//Quitting always works
		// if(Input.IsActionPressed("Quit"))
		// 	GetTree().Quit();
		if (Input.IsActionJustPressed("SkipAnimation") && player.IsPlaying())
			player.Seek(1,true);
	}

	bool Shown = false;
	public void OnSinglePlayerPressed()
	{
		player.Play("ShowSingleplayer");
	}

	//When the multiplayer (coop) button gets pressed
	public void OnMultiPressed()
	{
		GD.PushError("No multiplayer available... Yet!");
	}

	public void OnPlayPressed()
	{
		Globals.composer.AddScene("Game","preset1");
	}

	public void OnClosePressed()
	{
		player.Play("HideSingleplayer");
	}

	[Export] public Panel SingleplayerPanel;
}
