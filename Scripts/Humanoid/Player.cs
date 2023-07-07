using Godot;
using System;

public partial class Player : Human
{
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Roll"))
			RollDice();
		
	}

}
