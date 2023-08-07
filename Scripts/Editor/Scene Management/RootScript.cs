using Godot;
using System;

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
    }
}
