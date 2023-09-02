using Godot;
using Godot.Collections;
using DataManager;

public partial class Card : Control
{
	/// <summary> Demands information for Card.cs, please count from 0 on this specific one </summary>
	/// <returns> Card text? what else? </returns>
	public void CardManager(byte boardID)
	{
		byte propType = BoardLoader.LoadTileNumData(boardID, 1);
		byte cardID = BoardLoader.LoadTileNumData(boardID, 2);
		string internalPropName = "";

		// This decides when to change the texture of the card
		if(propType > 7)
		{

		}
		else
			_colorRect.Color = Globals.PropertyColors[cardID];

		// This changes the text
		for (byte i = 1; i < AllLabels.Count; i++)
		{
			AllLabels[i].Text = PropertyLoader.GetTextForCard(internalPropName, i);
		}
		
		Show();
		timer.Start();
	}

	public void OnTimeout()
	{
		Hide();
	}

	public void OnEventFired(byte boardID)
	{
		CardManager(boardID);
	}

	[Export] ColorRect _colorRect;
	[Export] Sprite2D _cardSprite;
	[Export] Timer timer;
	[Export] HBoxContainer AcquiringOptions;

	[Export] Array<Label> AllLabels = new();

}
