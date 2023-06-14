using Godot;
using Godot.Collections;
using System;

public partial class Card : Control
{
	void ChangeSprite()
	{
		//CardSprite.Texture = Cards[i];
	}

	[Export] Array<Label> PropertyValues = new Array<Label>();
	[Export] Sprite2D CardSprite;
	[Export] Array<Texture2D> Cards = new Array<Texture2D>();
}
