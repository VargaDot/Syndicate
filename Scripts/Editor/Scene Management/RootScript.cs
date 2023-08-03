using Godot;
using System;

public partial class RootScript : Node
{
    public override async void _Ready()
    {
        base._Ready();

        Globals.composer = GetNode<ComposerLib>("Composer");
        Globals.composer.AddScene("MainMenu",new Godot.Collections.Dictionary<string, Variant>());
    }
}
