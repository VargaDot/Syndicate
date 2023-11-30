using Godot;
using Godot.Collections;

public partial class Game : Node2D
{
	public override void _Process(double delta)
	{
		//Quits the game
		if(Input.IsActionPressed("Quit"))
		{
			EmitSignal("RequestUI", 0);
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
		EmitSignal("RequestUI", 5, roll);
		
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
		else if(inPrison) { EmitSignal("RequestUI", 6); }
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

		byte x = 1;
		switch (x)
		{
			case (byte)TileTypes.GO:
				
				break;
			case (byte)TileTypes.PROPERTY:
				EmitSignal("RequestUI", 1, agentPos);
				break;
			case (byte)TileTypes.CHEST:
				EmitSignal("RequestUI", 2);
				break;
			case (byte)TileTypes.CHANCE:
				EmitSignal("RequestUI", 3);
				break;
			case (byte)TileTypes.ITAX:
				
				break;
			case (byte)TileTypes.LTAX:
				
				break;
			case (byte)TileTypes.JAIL:

				break;
			case (byte)TileTypes.GOJAIL:
				
				break;
			case (byte)TileTypes.PARKING:
				EmitSignal("RequestUI", 4);
				break;
			default:
				GD.PrintErr("Property type not valid/Invalid dice roll/Invalid agent position");
				break;
		}
	}

	enum TileTypes { GO, PROPERTY, CHEST, CHANCE, ITAX, LTAX, JAIL, GOJAIL, PARKING }
	
	[Export] Camera2D cam;

	[Signal] public delegate void RequestUIEventHandler(byte functionID, Variant secondOption);

	/// <summary>
	/// Contains AgentID and Current Position.
	/// </summary>
	Dictionary<byte,byte> AgentList = new();
}