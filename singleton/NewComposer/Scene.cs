using Godot;

namespace ComposerLib
{
    [GlobalClass]
    public partial class Scene : Resource
    {
        [Signal]
        public delegate void FinishedLoadingEventHandler(string sceneName);

        [Signal]
        public delegate void FinishedCreatingEventHandler(string sceneName);

        [Export]
        public string InternalName {get; private set;}

        [Export(PropertyHint.File,"*.tscn")]
        public string PathToResource {get; set;}

        [Export]
        public PackedScene Resource {get; set;} = null;

        [Export]
        public SceneSettings Settings {get; set;} = null;

        public Node Instance {get; private set;} = null;

        internal Loader Loader;

        public Scene(string internalName, string path, SceneSettings settings = null)
        {
            InternalName = internalName;
            PathToResource = path;
            Settings = settings ?? new();

            GetLoader();

            if (settings.InstantLoad)
                Load();
        }

        public Scene(string internalName, PackedScene resource, string path = "", SceneSettings settings = null)
        {
            InternalName = internalName;
            Resource = resource;
            PathToResource = path;
            Settings = settings ?? new();

            GetLoader();

            if (settings.InstantLoad)
                Load();
        }

        public async void Load()
        {
            if (Resource != null || PathToResource == "" || Loader == null) return;

            Loader.AddToQueue(new LoaderScene(){
                Scene = this,
                UseSubthreads = Settings.UseSubthreads,
                CacheMode = Settings.CacheMode,
            });

            Loader.Enable();

            if (Settings.InstantCreate)
            {
                await ToSignal(this, SignalName.FinishedLoading);
                Create();
            }
        }

        public void Create()
        {
            if (Resource == null)
            {
                GD.PrintErr($"Create error for scene {InternalName}: Resource doesn't exist in memory.");
                return;
            }

            Instance = Resource.Instantiate();
            Settings.SceneParent.AddChild(Instance);

            if (Settings.DisableProcessing)
                Disable();

            EmitSignal(SignalName.FinishedCreating, InternalName);
        }

        public void Enable()
        {
            if (Instance == null)
            {
                GD.PrintErr($"Enable error for scene {InternalName}: No instance exists.");
                return;
            }

            Instance.ProcessMode = Node.ProcessModeEnum.Inherit;
        }

        public void Disable()
        {
            if (Instance == null)
            {
                GD.PrintErr($"Disable error for scene {InternalName}: No instance exists.");
                return;
            }

            Instance.ProcessMode = Node.ProcessModeEnum.Disabled;
        }

        public void Unload()
        {
            Resource?.Dispose();
            Resource = null;
        }

        public void Remove()
        {
            Instance?.QueueFree();
            Instance = null;
        }

        public new void Dispose()
        {
            Unload();
            Remove();
        }

        internal void OnLoaded(Scene scene, PackedScene resource)
        {
            if (scene.InternalName == InternalName && resource != null)
            {
                Resource = resource;
                EmitSignal(SignalName.FinishedLoading, InternalName);
            }
        }

        private void GetLoader()
        {
            Loader = ((SceneTree)Engine.GetMainLoop()).Root.GetNodeOrNull<Loader>("Composer/Loader");
            if (Loader == null)
            {
                GD.PrintErr($"Couldn't access Loader for scene {InternalName}. Load method will not work.");
            }
        }
    }
}