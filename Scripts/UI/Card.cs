using Godot;
using Godot.Collections;
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

	public void CardManager(bool isCardNeeded)
	{
		if(isCardNeeded)
		{
			TextRelated();
			CosmeticsRelated();

			Show();
		}
		else
		{
			Hide();
		}
	}

	void TextRelated()
	{
		//Gets the property name
		Globals.dataManager.DataRequester(_cardTitle, InternalCardName, 1);

		//To change the text as we speak
		for (byte i = 0; i < 5; i++)
		{
			Globals.dataManager.DataRequester(AllLabels[i].Text, InternalCardName, 6, i);
		}
	}

	void CosmeticsRelated()
	{

	}

	/// <summary>
	/// Demands information for Card.cs
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
	public void GetPropertyInformation(string _internalcardName, int CardSpriteID)
	{
		InternalCardName = _internalcardName;
	}

	//Exportables
	[ExportGroup("Card Asthetics")]
	[Export] ColorRect _colorRect;
	[Export] Sprite2D _cardSprite;
	[Export] Label _cardTitle;

	[ExportGroup("Card Labels")]
	[Export] static Label rent, house, house2, house3, house4, hotel, mortgage, buildcost;
	Array<Label> AllLabels = new Array<Label>()
	{
		rent,
		house,
		house2,
		house3,
		house4,
		hotel,
		mortgage,
		buildcost,
	};
	public string InternalCardName;
	
	[Signal] public delegate void GetInternalNameEventHandler();
}
