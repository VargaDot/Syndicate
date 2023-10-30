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


        // UNIT TEST - DISTRICT
            GDScript estateGD = (GDScript)GD.Load("res://Data//EstateCourt.gd");
            GodotObject estateND = (GodotObject)estateGD.New();
            estateND.Call("FetchDistrictData", 0, 1);
            estateND.Call("FetchDistrictData", 1, 4);
            estateND.Call("FetchDistrictData", 1, 3);
            estateND.Call("FetchDistrictData", 1, 2);
            estateND.Call("FetchDistrictData", 1, 1);
            estateND.Call("FetchDistrictData", 1, 5);
        // UNIT TEST - DISTRICT

        // UNIT TEST - ASSET
            estateND.Call("FetchAssetData", "Brown1", 1);
            estateND.Call("FetchAssetData", "Brown1", 2);
            estateND.Call("FetchAssetData", "Brown1", 3);
            estateND.Call("FetchAssetData", "Brown1", 4);
            estateND.Call("FetchAssetData", "Brown1", 4, 1);
            estateND.Call("FetchAssetData", "Brown1", 4, 2);
            estateND.Call("FetchAssetData", "Brown1", 4, 3);
            estateND.Call("FetchAssetData", "Brown1", 4, 4);
            estateND.Call("FetchAssetData", "Brown1", 4, 5);
        // UNIT TEST - ASSET
    }
}
