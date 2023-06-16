using Godot;
using Godot.Collections;
using System;

public partial class Globals : Node
{
	public static ComposerLib composer;

	///<summary>
	///Dictionary containing propertyIDs and their assigned color
	///</summary>
	///<returns>
	///Integers and Colors
	///</returns>
	public static Dictionary<int,Color> PropertyColors = new Dictionary<int, Color>()
	{
		{0, Colors.Brown},
		{1, Colors.Cyan},
		{2, Colors.Pink},
		{3, Colors.Orange},
		{4, Colors.Red},
		{5, Colors.Yellow},
		{6, Colors.Green},
		{7, Colors.Blue},
	};

	///<summary>
	///Dictionary containing propertyIDs and their assigned sprite based on their filesystem path
	///</summary>
	///<returns>
	///Integers and strings
	///</returns>
	public static Dictionary<int,String> PropertySprites = new Dictionary<int, string>()
	{
		{8,"res://Assets/Sprites/PropertyCards/TrainCard.png"},
		{9,"res://Assets/Sprites/PropertyCards/ElectricCard.png"},
		{10,"res://Assets/Sprites/PropertyCards/WaterCard.png"},
	};

}
