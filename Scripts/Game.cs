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

	public void TurnManager()
	{
		MoveAgent(1, 1);
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

	public void BrokeTheLaw()
	{
		EmitSignal("");
	}

	[Export] Camera2D cam;
	[Export] Registry registry;

	byte agentPos1, agentPos2, agentPos3, agentPos4;
}
