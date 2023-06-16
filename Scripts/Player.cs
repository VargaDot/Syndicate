using Godot;
using System;

public partial class Player : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	int previousDice;
	bool Fine;
	public void RollDice()
	{
		int x = GD.RandRange(2,12);
		if (x == previousDice && x == previousDice % 2 )
		{
			DoublesCheck();
		}
		else
		{
			DoubleTimes = 0;
		}

		if(Fine)
		{
			EmitSignal("RolledDice", x);
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
}
