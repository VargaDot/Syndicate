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

	Node2D Player1 = new Player(), Player2 = new Player(), Player3 = new Player(), Player4 = new Player();
	Array<Node2D> PlayerList = new Array<Node2D>();
	void LoadValues()
	{
		PlayerList.Add(Player1);
		PlayerList.Add(Player2);

		AddChild(PlayerList[0]);
		AddChild(PlayerList[1]);

		PlayerList[0].GetNode<Player>("Player");
		PlayerList[1].GetNode<Player>("Player1");

		if(Globals.PlayerUsernames[2] != null)
		{
			PlayerList.Add(Player3);
			AddChild(PlayerList[2]);
			PlayerList[2].GetNode<Player>("Player2");
		}
		else if(Globals.PlayerUsernames[3] != null)
		{
			PlayerList.Add(Player4);
			AddChild(PlayerList[3]);
			PlayerList[3].GetNode<Player>("Player3");
		}

		for (int i = 0; i < PlayerList.Count; i++)
		{
			PlayerList[i].Name = Globals.PlayerUsernames[i];
		}
	}

	void LoadBoard()
	{
		EmitSignal("BoardLaunched");

		int x = GD.RandRange(0, PlayerList.Count - 1);
		//PlayerList[x];
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
