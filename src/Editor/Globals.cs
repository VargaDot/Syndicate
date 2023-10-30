using Godot;
using Godot.Collections;

public partial class Globals : Node
{
	public static ComposerLib composer;

	///<summary>
	///Dictionary containing propertyIDs and their assigned color
	///</summary>
	///<returns>
	///Integers and Colors
	///</returns>
	public static Dictionary<byte,Color> PropertyColors = new()
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

	public enum TileTypes
    {
        GO,
        PROPERTY,
    	CHEST,
        CHANCE,
        ITAX,
        LTAX,
        JAIL,
        GOJAIL,
        PARKING,
    }

	///<summary>
	///Dictionary containing propertyIDs and their assigned sprite based on their filesystem path
	///</summary>
	///<returns>
	///Integers and strings
	///</returns>
	public static Dictionary<byte,string> PropertySprites = new()
	{
		/*
		{8, },
		{9, },
		{10, },
		*/
	};
}
