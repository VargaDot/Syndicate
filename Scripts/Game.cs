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
		TheRegistry.LoadAgents(AgentList);
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

	bool firstRound = true;
	public void TurnManager()
	{
		byte x = 0;
		if(firstRound)
		{
			x = (byte)GD.RandRange(0,AgentList.Keys.Count - 1);
			firstRound = false;
		}

		x++; 
		if(x > AgentList.Keys.Count - 1)
			x = 0;

		RollDice(x);
	}

	byte previousDice, DoubleTimes;
	bool inPrison;
	public void RollDice(byte agentID)
	{
		byte roll = (byte)GD.RandRange(2,12);
		
		if(!inPrison)
		{
			if (roll == previousDice % 2 )
			{
				DoubleTimes++;
				if(DoubleTimes == 3)
				{
					roll = 0;
					previousDice = 0;
					DoubleTimes = 0;
				}
			}
		}
		else if(inPrison)
		{
			EmitSignal("UIMessenger", 2);
		}
		else
		{
			MoveAgent(roll, AgentList[agentID], agentID);
			previousDice = roll;
		}
	}

	void MoveAgent(byte diceRoll, byte agentPos, byte agentID)
	{
		if(diceRoll + agentPos > 40)
			agentPos = 0;

		agentPos += diceRoll;

		byte x = BoardLoader.LoadTileNumData(agentPos, 1);
		switch (x)
		{
			case (byte)Globals.TileTypes.GO:
				TheRegistry.ConductTransaction(agentPos, 200);
				break;
			case (byte)Globals.TileTypes.PROPERTY:
				EmitSignal("UIMessenger", 1);
				break;
			case (byte)Globals.TileTypes.CHEST:
				EmitSignal("UIMessenger", 4);
				break;
			case (byte)Globals.TileTypes.CHANCE:
				EmitSignal("UIMessenger", 5);
				break;
			case (byte)Globals.TileTypes.ITAX:
				TheRegistry.ConductTransaction(agentPos, -100);
				break;
			case (byte)Globals.TileTypes.LTAX:
				TheRegistry.ConductTransaction(agentPos, -250);
				break;
			case (byte)Globals.TileTypes.GOJAIL:
				
				break;
			case (byte)Globals.TileTypes.PARKING:
				EmitSignal("UIMessenger", 6);
				break;
			default:
				GD.PrintErr("Property type not valid/Invalid dice roll/Invalid agent position");
				break;
		}
	}

	[Export] Camera2D cam;

	[Signal] public delegate void UIMessengerEventHandler(byte functionID, Variant secondOption);

	/// <summary>
	/// Contains AgentID and Current Position.
	/// </summary>
	Dictionary<byte,byte> AgentList = new();
}
