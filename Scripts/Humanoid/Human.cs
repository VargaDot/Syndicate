using Godot;
using System;

public partial class Human : Node2D
{
	public override void _Ready()
	{
		LoadValues();
	}

	void LoadValues()
	{
		
	}

	int previousDice;
	bool Fine;
	public void RollDice()
	{
		int x = GD.RandRange(2,12);
		if (x == previousDice % 2 )
		{
			DoublesCheck();
		}
		else if(Fine)
		{
			DoubleTimes = 0;
			EmitSignal("MovePiece", x);
		}
			
		previousDice = x;
	}

	int DoubleTimes;
	void DoublesCheck()
	{
		DoubleTimes++;

		if(DoubleTimes == 3)
		{
			Fine = false;
			EmitSignal("BrokeTheLaw");
			previousDice = 0;
			DoubleTimes = 0;
		}
		else
		{
			Fine = true;
		}
	}

	[Signal] public delegate void MovePieceEventHandler(int Dice);
}
