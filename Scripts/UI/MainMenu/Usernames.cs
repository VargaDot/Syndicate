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
		
		DataManager.TheRegistry.ListAgents();
	}

	///<Summary>
	///Checks UI Panel interaction and confirms/denies/reacts to them.
	///</Summary>
	public void UsernameManager(byte ID, string name = null)
    {
        DataManager.TheRegistry.AddAgents(ID, startingCash, name);
	}

	public void OnName1Registered()
	{
		string n = GetNode<LineEdit>("PlayerName").Text;
		UsernameManager(0, n);
		Enter1 = true;
	}

	public void OnName2Registered()
	{
		string n = GetNode<LineEdit>("PlayerName2").Text;
		UsernameManager(1, n);
		Enter2 = true;
	}

	public void OnName3Registered()
	{
		string n = GetNode<LineEdit>("PlayerName3").Text;
		UsernameManager(2, n);
	}

	public void OnName4Registered()
	{
		string n = GetNode<LineEdit>("PlayerName4").Text;
		UsernameManager(3, n);
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
