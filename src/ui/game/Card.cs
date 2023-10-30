using Godot;
using Godot.Collections;

public partial class Card : Control
{
	/// <summary> Demands information for Card.cs, please count from 0 on this specific one </summary>
	/// <returns> Card text? what else? </returns>
	public void CardManager(byte boardID)
	{
		//string internalPropName = "";
		int propType = 0;

		// This decides when to change the texture of the card
		if(propType > 7)
		{
			//_cardSprite.Texture = ResourceLoader.Load(Globals.PropertySprites[propType]);
		}
		else
			_colorRect.Color = Globals.PropertyColors[0];

		// This changes the text
		for (byte i = 1; i < AllLabels.Count; i++)
		{

		}
		
		timer.Start();
		Show();
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
