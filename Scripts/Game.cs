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
	public void RollDice(byte y)
	{
		byte x = (byte)GD.RandRange(2,12);
		
		if(!inPrison)
		{
			if (x == previousDice % 2 )
			{
				DoubleTimes++;
				if(DoubleTimes == 3)
				{
					EmitSignal("BrokeTheLaw");
					x = 0;
					previousDice = 0;
					DoubleTimes = 0;
				}
			}
		}
		else if(inPrison)
		{
			if(x == x % 2)
			{
				PromptBailOption();
			}
		}
		else
		{
			MoveAgent(AgentList[y], x);
			previousDice = x;
		}
	}

	void MoveAgent(byte diceRoll, byte agentPos)
	{
		if(diceRoll + agentPos > 40)
			agentPos = 0;

		agentPos += diceRoll;

		byte x = BoardLoader.LoadTileNumData(agentPos, 1);
		switch (x)
		{
			case (byte)TileTypes.GO:
				TheRegistry.ConductTransaction(agentPos, 200);
				break;
			case (byte)TileTypes.PROPERTY:
				TheRegistry.AddProperty(agentPos, agentPos);
				break;
			case (byte)TileTypes.CHEST:
				HoldChestEvent();
				break;
			case (byte)TileTypes.CHANCE:
				HoldChanceEvent();
				break;
			case (byte)TileTypes.ITAX:
				TheRegistry.ConductTransaction(agentPos, -100);
				break;
			case (byte)TileTypes.LTAX:
				TheRegistry.ConductTransaction(agentPos, -250);
				break;
			case (byte)TileTypes.GOJAIL:
				BrokeTheLaw();
				break;
			case (byte)TileTypes.PARKING:
				HoldParkingEvent();
				break;
			default:
				GD.PrintErr("Property type not valid/Invalid dice roll/Invalid agent position");
				break;
		}
	}

	enum TileTypes
    {
        GO,
        PROPERTY,
    	CHEST,
        CHANCE,
        ITAX,
        LTAX,
        JAIL,
        GOJAIL,
        PARKING,
    }

	void HoldChestEvent()
	{

	}

	void HoldChanceEvent()
	{
	
	}

	void HoldParkingEvent()
	{

	}

	void PromptBailOption()
	{

	}

	public void BrokeTheLaw()
	{
		EmitSignal("");
	}

	[Export] Camera2D cam;

	/// <summary>
	/// Contains AgentID and Current Position.
	/// </summary>
	Dictionary<byte,byte> AgentList = new();
}
