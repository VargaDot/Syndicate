using Godot;
using System;

public partial class LoadingScreen : Control
{

    [Signal]
    public delegate void ContinueLoadingEventHandler();

    [Signal]
    public delegate void FinishedLoadingEventHandler();

    public override async void _Ready()
    {
        base._Ready();

        if (Globals.composer == null)
            await ToSignal(GetTree().CurrentScene,"ComposerLoaded");

        Globals.composer.LoadingBegan += OnLoadingBegan;
        Globals.composer.LoadingValueUpdated += OnLoadingValueUpdated;
        Globals.composer.LoadingDone += OnLoadingDone;

        Hide();
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public void Activate(string sceneName)
    {
        if (sceneName != Name)
            return;

        ProcessMode = ProcessModeEnum.Inherit;
        Show();
        Globals.composer.IsSafeToContinueLoading = true;
        EmitSignal("ContinueLoading");
    }

    public void Disable(string sceneName)
    {
        if (sceneName != Name)
            return;

        Hide();
        Globals.composer.IsSafeToFinishLoading = true;
        EmitSignal("FinishedLoading");
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public virtual void OnLoadingBegan(string sceneName)
    {
        Activate(Name);
    }

    public virtual void OnLoadingValueUpdated(float value)
    {
        GD.Print($"{value*100}%");
    }

    public virtual void OnLoadingDone(string sceneName,int errorCode)
    {
        if (errorCode == 0)
            Disable(Name);
        else
            GD.Print($"Error when loading scene {sceneName} with error {errorCode}");
    }
}
