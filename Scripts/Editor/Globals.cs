using Godot;
using Godot.Collections;
using System;

public partial class Globals : Node
{
	public static ComposerLib composer;

	public static Array<string> PlayerUsernames = new Array<string>();

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
	public static Dictionary<int,Texture2D> PropertySprites = new Dictionary<int, Texture2D>()
	{
		/*
		{8, },
		{9, },
		{10, },
		*/
	};
}
