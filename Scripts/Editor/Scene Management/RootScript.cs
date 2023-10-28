using Godot;

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

        GDScript MyGDScript = (GDScript)GD.Load("res://Data//EstateCourt.gd");
        GodotObject myGDScriptNode = (GodotObject)MyGDScript.New(); // This is a GodotObject
        myGDScriptNode.Call("FetchDistrictData", 2, 1);

    }
}
