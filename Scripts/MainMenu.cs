using Godot;
using System;

///<Summary>
///The script that's responsible for running the main menu button functions.
///</Summary>
public partial class MainMenu : Control
{
	public override void _Process(double delta)
	{
		//Quitting always works
		if(Input.IsActionPressed("Quit"))
			GetTree().Quit();
	}

	//When the play button (singleplayer) gets pressed
	public void OnSinglePlayerPressed()
	{
		Globals.composer.GotoScene("Game", new Godot.Collections.Dictionary<string, Variant>()
		{
			{"IsAnimated",true},
			{"Animation",1}
		});
	}

	//When the multiplayer (coop) button gets pressed
	public void OnMultiPressed()
	{
		GD.PushError("No multiplayer available... Yet!");
	}
}
