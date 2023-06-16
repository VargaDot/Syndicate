using Godot;
using System;

public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadBoard();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Quit"))
		{
			GD.PushError("No Pause Menu has been made yet.");
			GetTree().Quit();
		}
	}

	void LoadBoard()
	{
		
	}
}
