using Godot;
using Godot.Collections;
using DataManager;

//This is strictly for UI, player turns and visual stuff.
public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadGame();
	}

	//Loads and instantiates players into the game scene.
	void LoadGame()
	{
		/*
		UsernamesManager.LoadUsername();
		TheRegistry.AddPlayers();
		*/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Quits the game
		if(Input.IsActionPressed("Quit"))
		{
			GD.PushWarning("No Pause Menu has been made yet.");
			Globals.composer.AddScene("MainMenu","preset1");
		}
	}

	public void MovePiece(byte diceRoll, byte agentPos)
	{
		if(diceRoll + agentPos > 40)
			agentPos = 0;

		agentPos += diceRoll;
	}

	public void BrokeTheLaw()
	{
		EmitSignal("");
	}

	[Export] Camera2D cam;
	[Export] Registry registry;

	byte agentPos1, agentPos2, agentPos3, agentPos4;
}
