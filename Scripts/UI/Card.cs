using Godot;
using Godot.Collections;
using DataManager;
using System;

public partial class Card : Control
{
    public override void _Ready()
    {
        LoadValues();
    }

	void LoadValues()
	{

	}

	/// <summary>
	/// Demands information for Card.cs, please count from 0 on this specific one
	/// <list type = "number">
    /// <item><description><para><em> Normal </em></para></description></item>
	///	<item><description><para><em> Train </em></para></description></item>
	/// <item><description><para><em> Utility </em></para></description></item>
	/// <item><description><para><em> Chance </em></para></description></item>
	/// <item><description><para><em> Community Chest </em></para></description></item>
	/// <item><description><para><em> Police* </em></para></description></item>
	/// <item><description><para><em> Parking* </em></para></description></item>
	/// <item><description><para><em> Tax* </em></para></description></item>
	/// </list>
	/// * Not used currently
	/// </summary>
	/// <returns>
	///	Card text? what else?
	/// </returns>
	public void CardManager(bool isCardNeeded, string internalPropName, byte cardID)
	{
		if(isCardNeeded)
		{
			TextRelated(internalPropName);
			CosmeticsRelated(cardID);

			Show();
		}
		else
		{
			Hide();
		}
	}

	void TextRelated(string InternalCardName)
	{
		//Changes all of the text inside the card
		for (byte i = 1; i < AllLabels.Count; i++)
		{
			AllLabels[i].Text = PropertyLoader.GetTextForCard(InternalCardName, i);
		}
	}

	enum CardIDs
	{
		NORMAL,
		TRAIN,
		UTILITY,
		CHANCE,
		COMMUNITYCHEST,
		POLICE,
		PARKING,
		TAX,
	}

	void CosmeticsRelated(byte internalCardID)
	{
		if(!Globals.PropertyColors.ContainsKey(internalCardID))
			GD.PushError("Invalid CardColor ID");

		_colorRect.Color = Globals.PropertyColors[internalCardID];
	}

	[Export] ColorRect _colorRect;
	[Export] Sprite2D _cardSprite;

	[Export] Array<Label> AllLabels = new Array<Label>();
}
