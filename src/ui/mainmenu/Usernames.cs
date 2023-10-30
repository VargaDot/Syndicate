using Godot;

public partial class Usernames : HFlowContainer
{
    public override void _EnterTree()
    {
        Enter1 = false;
		Enter2 = false;
    }

	public void OnGoPressed()
	{
		if(Enter1 && Enter2)
			EmitSignal("OnPlayPressed");
		else
			LaunchWarning(1);
	}

	///<Summary>
	///Checks UI Panel interaction and confirms/denies/reacts to them.
	///</Summary>
	public void UsernameManager(byte ID, string name)
    {
		
	}

	public void OnAgent1Registered(string new_text)
	{
		UsernameManager(0, new_text);
		Enter1 = true;
	}

	public void OnAgent2Registered(string new_text)
	{
		UsernameManager(1, new_text);
		Enter2 = true;
	}

	public void OnAgent3Registered(string new_text)
	{
		UsernameManager(2, new_text);
	}

	public void OnAgent4Registered(string new_text)
	{
		UsernameManager(3, new_text);
	}

	static ushort startingCash = 2500;
	public void OnCashChosen()
	{
		//int x = GetNode<>
	}

	public void OnInvalidUsername()
	{
		LaunchWarning(0);
	}

	void LaunchWarning(byte x)
	{
	 	switch (x)
		{
			case 0:
				PopUp.Text = "Invalid username";
				break;
			case 1:
				PopUp.Text = "Please enter usernames for Player 1 and Player 2";
				break;
			default:
				PopUp.Text = "There seems to be a problem";
				GD.PushError("UsernameManager.cs has fired a default error, here's all the information you need");
				break;
		}

		PopUp.Show();
		textTimer.Start();
	}

	void OnTimeout()
	{
		PopUp.Hide();
	}

	bool Enter1, Enter2;

	[Export] public Label PopUp;
	[Export] public Timer textTimer;

	[Signal] public delegate void GoPressedEventHandler();
}
