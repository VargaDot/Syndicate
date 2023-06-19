using Godot;
using Godot.Collections;
using System;

public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadValues();
		LoadBoard();
	}

	void LoadValues()
	{
		
	}

	void LoadBoard()
	{
		
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

	public void TurnManager()
	{
		
	}

	public void MovePiece(int Dice)
	{

	}

	public void BrokeTheLaw()
	{
		EmitSignal("");
	}

	public Node2D H1, H2, H3, H4;
}
