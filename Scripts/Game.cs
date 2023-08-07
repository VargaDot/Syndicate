using Godot;
using Godot.Collections;
using DataManager;

//This is strictly for UI, player turns and visual stuff.
public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadValues();
		LoadBoard();
	}

	//Loads and instantiates players into the game scene.
	void LoadValues()
	{
		/*
		UsernamesManager.LoadUsername();
		TheRegistry.AddPlayers();
		*/
	}

	//Starts the turn-based game
	void LoadBoard()
	{
		EmitSignal("BoardLaunched", PlayerList.Count);

		byte x = (byte)GD.RandRange(0, PlayerList.Count - 1);
		PlayerList[x].GetNode<Player>("").RollDice();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Quits the game
		if(Input.IsActionPressed("Quit"))
		{
			GD.PushWarning("No Pause Menu has been made yet.");
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
	Node2D Player1, Player2, Player3, Player4;

	[Export] Camera2D cam;
	[Export] Registry registry;
	[Signal] public delegate void BoardLaunchedEventHandler(int n);
}
