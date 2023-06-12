using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Quit"))
			GetTree().Quit();
	}

	void OnSinglePlayerPressed()
	{

	}

	void OnMultiPressed()
	{
		
	}
}
