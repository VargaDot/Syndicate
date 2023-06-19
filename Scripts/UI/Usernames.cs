using Godot;
using Godot.Collections;
using System;

public partial class Usernames : HFlowContainer
{
	public override void _Ready()
	{
		for (int i = 0; i < 4; i++)
		{
			InputFields [i] = (LineEdit)GetChild(i);
		}
	}

	public void UsernameManager(int x)
	{
		switch (x)
		{
			case(0):
				Globals.PlayerUsernames[0] = InputFields[0].Text;
				PopUp.Text = "Username " + InputFields[0] + " Has been submitted";
				break;
			case(1):
				Globals.PlayerUsernames[1] = InputFields[1].Text;
				PopUp.Text = "Username " + InputFields[1] + " Has been submitted";
				break;
			case(2):
				Globals.PlayerUsernames[2] = InputFields[2].Text;
				PopUp.Text = "Username " + InputFields[2] + " Has been submitted";
				break;
			case(3):
				Globals.PlayerUsernames[3] = InputFields[3].Text;
				PopUp.Text = "Username " + InputFields[3] + " Has been submitted";
				break;
			case(4):
				PopUp.Text = "Invalid username";
				break;
			default:
				PopUp.Text = "There seems to be a problem";
				break;
		}

		PopUp.Show();
		textTimer.Start();
	}


	public void OnName1Registered()
	{
		UsernameManager(0);
	}

	public void OnName2Registered()
	{
		UsernameManager(1);
	}

	public void OnName3Registered()
	{
		UsernameManager(2);
	}

	public void OnName4Registered()
	{
		UsernameManager(3);
	}

	public void OnInvalidUsername()
	{
		UsernameManager(4);
	}

	void OnTimeout()
	{
		PopUp.Hide();
	}

	Array<LineEdit> InputFields = new Array<LineEdit>();
	[Export] public Label PopUp;
	[Export] public Timer textTimer;
}
