using Godot;
using Godot.Collections;
using System;

public partial class Card : Control
{
	void CardManager(int cardUI)
	{
		if(cardUI <= 7)
			_colorRect.Color = Globals.PropertyColors[cardUI];
		else if(cardUI > 7 && cardUI <= 10)
		{
			//_cardSprite.Texture = Globals.PropertySprites[cardUI];
		}
		else
		{
			
		}
	}

	void DisplayTitle(string newTitle)
	{
		_cardTitle.Text = newTitle;
	}

	void DisplayValues()
	{
		rent.Text = _rent.ToString();
		house.Text = _house.ToString();
		house2.Text = _house2.ToString();
		house3.Text = _house3.ToString();
		house4.Text = _house4.ToString();
		hotel.Text = _hotel.ToString();
		mortgage.Text = _mortgage.ToString();
		buildcost.Text = _buildcost.ToString();
	}

	public int _rent, _house, _house2, _house3, _house4, _hotel, _mortgage, _buildcost;

	//Exportables
	[ExportGroup("Card Asthetics")]
	[Export] ColorRect _colorRect;
	[Export] Sprite2D _cardSprite;
	[Export] Label _cardTitle;

	[ExportGroup("Card Labels")]
	[Export] Label rent, house, house2, house3, house4, hotel, mortgage, buildcost;
	
}
