using Godot;
using System;
using System.Collections.Generic;

public partial class Human : Node2D
{
	public override void _Ready()
	{
		LoadValues();
	}

	void LoadValues()
	{
		
	}

	protected enum STATES
	{
		ROLLING,
		TRADING,
		BANKRUPT,
	}

	byte previousDice, DoubleTimes;
	bool inPrison;
	public void RollDice()
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
				//Bail(false, 0);
			}
		}
		
		else
		{
			EmitSignal("MovePiece", x);	
			previousDice = x;
		}
	}

	public void GameOver()
	{
		QueueFree();
	}

	[Signal] public delegate void MovePieceEventHandler(int Dice);
	[Signal] public delegate void BrokeTheLawEventHandler();
	[Signal] public delegate void ReleasedEventHandler();

}
