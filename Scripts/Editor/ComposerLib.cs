using Godot;
using System;
using Godot.Collections;

public partial class ComposerLib : Node
{

    public Dictionary<string,string> ScenePaths = new Dictionary<string, string>()
    {
        {"MainMenu","res://Scenes/MainMenu.tscn"},
        {"Game","res://Scenes/Game.tscn"},
    };

    public Node2D CurrentScene {get; private set;} = null;
    public enum ANIMATIONS {
        NONE = 0,
        FADE = 1,
    }


    const string CONTROL_PATH = "CanvasLayer/TransitionUI";

    Dictionary<string,Variant> Variables = new Dictionary<string, Variant>();
    bool _isEnteringScene = false;
    Node2D root = null;

    string _scenePath = "";
    PackedScene _packedScene = null;
    Dictionary<string,Variant> _currentArgs = null;
    Dictionary<string,Variant> _currentSceneVars = null;


    /// <summary>
    /// Returns the variables passed with <strong>sceneVars</strong> argument in GotoScene.
    /// </summary>
    /// <returns>
    /// A Dictionary with string keys and Variant values.
    /// </returns>
    public Dictionary<string, Variant> GetSceneVariables() => Variables;

    /// <summary>
    /// Gets the current scene's name if there is one present.
    /// </summary>
    /// <returns>
    /// A string with the current scene's name or empty string if there is no scene present.
    /// </returns>
    public string GetSceneName()
    {
        if (CurrentScene == null)
            return "";

        return CurrentScene.Name;
    }


    /// <summary>
    /// Reloads the current scene. Does nothing if there is no scene present.
    /// </summary>
    public void ReloadCurrentScene()
    {
        if (CurrentScene == null)
            return;

        if (_scenePath != "")
            GotoScene(_scenePath, _currentArgs, _currentSceneVars);
        else if (_packedScene != null)
            GotoScene(_packedScene, _currentArgs, _currentSceneVars);
    }

    // public void RemoveScene(string scene)
    // {
    //     using var _sceneToRemove = root.GetNodeOrNull<Node2D>((string)scene);

    //     if (_sceneToRemove == null)
    //         return;

    //     _sceneToRemove.QueueFree();
    // }

    /// <summary>
    /// Removes the current scene from Main scene (root).
    /// </summary>
    public void RemoveCurrentScene()
    {
        if (CurrentScene == null)
            return;

        root.GetNode<Node2D>((string)CurrentScene.Name).QueueFree();
        CurrentScene = null;
    }

    /// <summary>
    /// Changes the current scene to a new one. args is responsible for customization of the process.
    /// <list type="bullet">
    /// <item>scene - The scene's name or full path. A short name can be used if the full path exists in ScenePaths variable.</item>
    /// <item>args - A dictionary with different parameters for the changing scene process. Valid ones are:</item>
    /// <list type="number">
    /// <item>IsAnimated - bool. If true, will play a transition based on the Animation value</item>
    /// <item>Animation - int. A value of 1 will play Fade in/Fade out transition. Value of 0 will not play any transition</item>
    /// </list>
    /// <item>sceneVars - Variables to pass to the next scene. Can be accessed by calling GetSceneVariables() in the new scene.</item>
    /// </list>
    /// </summary>
    public void GotoScene(string scene, Dictionary<string,Variant> args = null, Dictionary<string,Variant> sceneVars = null)
    {
        if (_isEnteringScene)
            return;

        _isEnteringScene = true;

        if (!scene.Contains("res://"))
        {
            if (ScenePaths.ContainsKey(scene))
                _scenePath = ScenePaths[scene];
            else
            {
                GD.PushError($"Invalid scene name {scene}. Have you made a typo?");
                throw new ArgumentException($"Invalid scene name {scene}. Have you made a typo?");
            }
        }
        else
            _scenePath = scene;

        if (root == null)
            root = GetTree().Root.GetNode<Node2D>("Main");

        if (args == null)
            args = new Dictionary<string, Variant>();

        if (sceneVars != null)
            Variables = sceneVars;

        _currentArgs = args;
        _currentSceneVars = Variables;

        CallDeferred("DeferredGotoScene",_scenePath, args);
    }

    /// <summary>
    /// Changes the current scene to a new one. args is responsible for customization of the process.
    /// <list type="bullet">
    /// <item>scene - PackedScene containing the new scene.</item>
    /// <item>args - A dictionary with different parameters for the changing scene process. Valid ones are:</item>
    /// <list type="number">
    /// <item>IsAnimated - bool. If true, will play a transition based on the Animation value</item>
    /// <item>Animation - int. A value of 1 will play Fade in/Fade out transition. Value of 0 will not play any transition</item>
    /// </list>
    /// <item>sceneVars - Variables to pass to the next scene. Can be accessed by calling GetSceneVariables() in the new scene.</item>
    /// </list>
    /// </summary>
    public void GotoScene(PackedScene scene, Dictionary<string,Variant> args = null, Dictionary<string,Variant> sceneVars = null)
    {
        if (_isEnteringScene)
            return;

        _isEnteringScene = true;

        if (root == null)
            root = GetTree().Root.GetNode<Node2D>("Main");

        if (args == null)
            args = new Dictionary<string, Variant>();

        if (sceneVars == null)
            Variables = new Dictionary<string, Variant>();

        _packedScene = scene;
        _currentArgs = args;
        _currentSceneVars = Variables;

        CallDeferred("DeferredGotoScene",scene, args);
    }

    void DeferredGotoScene(string scene, Dictionary<string,Variant> args = null)
    {
        bool _isAnimated = args.ContainsKey("IsAnimated") ? (bool)args["IsAnimated"] : false;
        ANIMATIONS _animation = args.ContainsKey("Animation") ? (ANIMATIONS)((int)args["Animation"]) : ANIMATIONS.NONE;

        if (_isAnimated && _animation != ANIMATIONS.NONE)
        {
            switch (_animation)
            {
                case ANIMATIONS.FADE:
                {
                    FadeTransition(scene, args);
                    return;
                }
                default:
                    break;
            }
        }

        AddScene(scene);

        _isEnteringScene = false;
    }

    void DeferredGotoScene(PackedScene scene, Dictionary<string,Variant> args = null)
    {
        bool _isAnimated = args.ContainsKey("IsAnimated") ? (bool)args["IsAnimated"] : false;
        ANIMATIONS _animation = args.ContainsKey("Animation") ? (ANIMATIONS)((int)args["Animation"]) : ANIMATIONS.NONE;

        if (_isAnimated && _animation != ANIMATIONS.NONE)
        {
            switch (_animation)
            {
                case ANIMATIONS.FADE:
                {
                    FadeTransition(scene, args);
                    return;
                }
                default:
                    break;
            }
        }

        AddScene(scene);

        _isEnteringScene = false;
    }

    async void FadeTransition(string scene, Dictionary<string,Variant> args)
    {
        Control _control = root.GetNode<Control>(CONTROL_PATH);
        _control.Show();

        ColorRect _transitionRect = _control.GetNode<ColorRect>("FadeRect");
        _transitionRect.Show();

        if (CurrentScene != null)
        {
            Tween _fadeInTween = GetTree().CreateTween();
            double _fadeInDuration = args.ContainsKey("FadeInDuration") ? (double)args["FadeInDuration"] : 0.5;

            _transitionRect.Color = new Color(0,0,0,0);

            _fadeInTween.TweenProperty(_transitionRect,"color",new Color(0,0,0,1),_fadeInDuration).SetTrans(Tween.TransitionType.Sine);

            await ToSignal(_fadeInTween,"finished");

            _fadeInTween.Kill();
        }

        AddScene(scene);

        Tween _fadeOutTween = GetTree().CreateTween();
        double _fadeOutDuration = args.ContainsKey("FadeOutDuration") ? (double)args["FadeOutDuration"] : 0.5;

        _fadeOutTween.TweenProperty(_transitionRect,"color",new Color(0,0,0,0),_fadeOutDuration).SetTrans(Tween.TransitionType.Sine);

        await ToSignal(_fadeOutTween,"finished");

        _isEnteringScene = false;

        _fadeOutTween.Kill();
        _transitionRect.Hide();
        _control.Hide();
    }

    async void FadeTransition(PackedScene scene, Dictionary<string,Variant> args)
    {
        Control _control = root.GetNode<Control>(CONTROL_PATH);
        _control.Show();

        ColorRect _transitionRect = _control.GetNode<ColorRect>("FadeRect");
        _transitionRect.Show();


        if (CurrentScene != null)
        {
            Tween _fadeInTween = GetTree().CreateTween();
            double _fadeInDuration = args.ContainsKey("FadeInDuration") ? (double)args["FadeInDuration"] : 0.5;

            _transitionRect.Color = new Color(0,0,0,0);

            _fadeInTween.TweenProperty(_transitionRect,"color",new Color(0,0,0,1),_fadeInDuration).SetTrans(Tween.TransitionType.Sine);

            await ToSignal(_fadeInTween,"finished");

            _fadeInTween.Kill();
        }

        AddScene(scene);

        Tween _fadeOutTween = GetTree().CreateTween();
        double _fadeOutDuration = args.ContainsKey("FadeOutDuration") ? (double)args["FadeOutDuration"] : 0.5;

        _fadeOutTween.TweenProperty(_transitionRect,"color",new Color(0,0,0,0),_fadeOutDuration).SetTrans(Tween.TransitionType.Sine);

        await ToSignal(_fadeOutTween,"finished");

        _isEnteringScene = false;

        _fadeOutTween.Kill();
        _transitionRect.Hide();
        _control.Hide();
    }

    void AddScene(string scene)
    {
        if (CurrentScene != null)
            root.GetNode<Node2D>((string)CurrentScene.Name).QueueFree();

        root.AddChild(ResourceLoader.Load<PackedScene>(scene).Instantiate());

        CurrentScene = root.GetChild<Node2D>(root.GetChildCount() - 1);
    }

    void AddScene(PackedScene scene)
    {
        if (CurrentScene != null)
            root.GetNode<Node2D>((string)CurrentScene.Name).QueueFree();

        root.AddChild(scene.Instantiate());

        CurrentScene = root.GetChild<Node2D>(root.GetChildCount() - 1);
    }
}
