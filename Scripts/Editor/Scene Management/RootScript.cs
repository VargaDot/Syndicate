using Godot;
using System;
using DataManager;

public partial class RootScript : Node
{
    [Signal]
    public delegate void ComposerLoadedEventHandler();

    public override void _Ready()
    {
        base._Ready();

        Globals.composer = GetNode<ComposerLib>("/root/Composer");
        EmitSignal("ComposerLoaded");

        Globals.composer.AddScene("MainMenu","preset1");

        //GD.Print(PropertyLoader.GetPropertyPrice("Brown1"));
    }
}
