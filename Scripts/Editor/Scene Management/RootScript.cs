using Godot;
using System;

public partial class RootScript : Node
{
    public override async void _Ready()
    {
        base._Ready();

        Globals.composer = GetNode<ComposerLib>("Composer");

        Globals.composer.AddScene("MainMenu","preset1");

        await ToSignal(Globals.composer,"LoadingDone");

        Globals.composer.AddScene("Game",new Godot.Collections.Dictionary<string, Variant>(){
            {"activate_on_startup",false},
        },new Godot.Collections.Dictionary<string, Variant>());
    }
}
