using Godot;
using Godot.Collections;
using DataManager;

public partial class Card : Control
{
    public override void _Ready()
    {
        LoadValues();
    }

	void LoadValues()
	{

	}

	/// <summary> Demands information for Card.cs, please count from 0 on this specific one </summary>
	/// <returns> Card text? what else? </returns>
	public void CardManager(string internalPropName)
	{
		bool needsSpriteChange = false;
		byte cardID = 0;

		TextRelated(internalPropName);
		if(needsSpriteChange)
		{
			CosmeticsRelated(cardID);
		}
		
		Show();
		timer.Start();
	}
	
	void TextRelated(string InternalCardName)
	{
		//BoardLoader.LoadTileStrData(InternalCardName, 1);
		
		//Changes all of the text inside the card
		for (byte i = 1; i < AllLabels.Count; i++)
		{
			AllLabels[i].Text = PropertyLoader.GetTextForCard(InternalCardName, i);
		}
	}

	public void OnTimeout()
	{
		Hide();
	}

	void CosmeticsRelated(byte internalCardID)
	{
		if(!Globals.PropertyColors.ContainsKey(internalCardID))
			GD.PushError("Invalid CardColor ID");

		_colorRect.Color = Globals.PropertyColors[internalCardID];
	}

	public void OnEventFired(string internalPropName)
	{
		CardManager(internalPropName);
	}

	[Export] ColorRect _colorRect;
	[Export] Sprite2D _cardSprite;
	[Export] Timer timer;

	[Export] Array<Label> AllLabels = new Array<Label>();
}
