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

	int TotalPlayers;
	string Player1, Player2, Player3, Player4;
	void LoadValues()
	{
		Globals.PlayerUsernames[0] = Player1;
		Globals.PlayerUsernames[1] = Player2;
		TotalPlayers = 2;

		if(Globals.PlayerUsernames[2] != null)
		{
			Globals.PlayerUsernames[2] = Player3;
			TotalPlayers++;
		}
		else if(Globals.PlayerUsernames[3] != null)
		{
			Globals.PlayerUsernames[3] = Player4;
			TotalPlayers++;
		}
	}

	void LoadBoard()
	{
		EmitSignal("BoardLaunched", TotalPlayers);
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

	public void MovePiece(int Dice)
	{

	}

	public void BrokeTheLaw()
	{
		EmitSignal("");
	}

	public Node2D H1, H2, H3, H4;

	[Export] Camera2D cam;

	[Signal] public delegate void BoardLaunchedEventHandler(int n);
}
