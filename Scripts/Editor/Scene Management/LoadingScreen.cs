using Godot;
using System;

public partial class LoadingScreen : Control
{

    [Signal]
    public delegate void ContinueLoadingEventHandler();

    [Signal]
    public delegate void FinishedLoadingEventHandler();

    public bool isActive = false;

    public bool hasInitialized = false;
    public bool hasFinished = false;

    public override async void _Ready()
    {
        base._Ready();

        if (Globals.composer == null)
            await ToSignal(GetTree(),"process_frame");

        Globals.composer.LoadingBegan += OnLoadingBegan;
        Globals.composer.LoadingValueUpdated += OnLoadingValueUpdated;
        Globals.composer.LoadingDone += OnLoadingDone;

        Hide();
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public virtual void Activate()
    {
        hasInitialized = false;
        hasFinished = false;
        isActive = true;

        Show();
        ProcessMode = ProcessModeEnum.Inherit;
        hasInitialized = true;
    }

    public virtual void Disable()
    {
        isActive = false;
        hasFinished = true;

        Hide();
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public virtual void OnLoadingBegan(string sceneName)
    {
        if (!isActive)
            return;

        Activate();
    }

    public virtual void OnLoadingValueUpdated(float value)
    {
        if (!isActive)
            return;

        GD.Print($"{value*100}%");
    }

    public virtual void OnLoadingDone(string sceneName,int errorCode)
    {
        if (!isActive)
            return;

        if (errorCode == 0)
            Disable();
        else
            GD.Print($"Error when loading scene {sceneName} with error {errorCode}");
    }
}
