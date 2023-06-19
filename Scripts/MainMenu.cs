using Godot;
using System;

///<Summary>
///The script that's responsible for running the main menu button functions.
///</Summary>
public partial class MainMenu : Control
{
	public override void _Ready()
	{
		LoadValues();
	}

	void LoadValues()
	{
		//LoadUsernames();
	}

	public override void _Process(double delta)
	{
		//Quitting always works
		if(Input.IsActionPressed("Quit"))
			GetTree().Quit();
	}

	bool Shown = false;
	public void OnSinglePlayerPressed()
	{
		if (Shown == false)
		{
			SingleplayerPanel.Show();
			Shown = true;
		}
		else
		{
			SingleplayerPanel.Hide();
		}
	}

	//When the multiplayer (coop) button gets pressed
	public void OnMultiPressed()
	{
		GD.PushError("No multiplayer available... Yet!");
	}

	public void OnPlayPressed()
	{
		Globals.composer.GotoScene("Game", new Godot.Collections.Dictionary<string, Variant>()
		{
			{"IsAnimated",true},
			{"Animation",1}
		});
	}

	/*
	void LoadUsernames()
	{
		for (int i = 0; i < Globals.PlayerUsernames.Count; i++)
		{
			
		}
	}
	*/

	[Export] public Panel SingleplayerPanel;
}
