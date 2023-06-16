using Godot;
using Godot.Collections;
using System;

public partial class Card : Control
{
	public void DistrictProtocol()
	{
		/*
		DisplayColor();
		DisplayTitle();
		DisplayValues();

		Show();
		*/
	}

	public void OnCardUIExit()
	{
		Hide();
	}

	public void RailProtocol()
	{

	}

	public void UtilityProtocol()
	{

	}

	void DisplayColor(Color newColor)
	{
		_colorRect.Color = newColor;
	}

	void ChangeSprite()
	{
		//_cardSprite.Texture = ;
	}

	void DisplayTitle(string newTitle)
	{
		_cardTitle.Text = newTitle;
	}

	void DisplayValues()
	{
		Label _rent = (Label)AllValues.GetChild(0);
		Label _house = (Label)AllValues.GetChild(1);
		Label _house2 = (Label)AllValues.GetChild(2);
		Label _house3 = (Label)AllValues.GetChild(3);
		Label _house4 = (Label)AllValues.GetChild(4);
		Label _hotel = (Label)AllValues.GetChild(5);
		Label _mortgage = (Label)AllValues.GetChild(6);
		Label _buildcost = (Label)AllValues.GetChild(7);

		_rent.Text = "";
		_house.Text = "";
		_house2.Text = "";
		_house3.Text = "";
		_house4.Text = "";
		_hotel.Text = "";
		_mortgage.Text = "";
		_buildcost.Text = "";
	}

	//Exportables
	[ExportGroup("Card Asthetics")]
	[Export] ColorRect _colorRect;
	[Export] Sprite2D _cardSprite;
	[Export] Label _cardTitle;

	[ExportGroup("Card Stats")]
	[Export] HBoxContainer AllValues;
}
