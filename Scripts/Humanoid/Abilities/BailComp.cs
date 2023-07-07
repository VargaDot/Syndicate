using Godot;
using System;

public partial class BailComp : Node2D
{
    [Export] public Node2D Agent;

    public override void _Ready()
    {
    }

	/*
    public void Bail(bool boughtBail, int bail)
	{
		if(boughtBail)
		{
			Cash =- bail;
		}

		EmitSignal("Released");
		inPrison = false;
	}
	*/
}
