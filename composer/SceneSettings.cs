using Godot;

namespace ComposerLib
{
    [GlobalClass]
    public partial class SceneSettings: Resource
    {
        public Node SceneParent {
            get => _sceneParent;
            set
            {
                if (IsInstanceValid(value))
                    _sceneParent = value;
                else
                {
                    GD.PrintErr($"Node {value} is invalid parent. SceneParent is now Root.");
                    _sceneParent = ((SceneTree)Engine.GetMainLoop()).Root;
                }
            }
        }
        private Node _sceneParent = ((SceneTree)Engine.GetMainLoop()).Root;

        [Export]
        public bool InstantLoad {get; set;} = true;

        [Export]
        public bool InstantCreate {get; set;} = false;

        [Export]
        public bool DisableProcessing {get; set;} = false;

        [Export]
        public bool UseSubthreads {get; set;} = false;

        [Export]
        public ResourceLoader.CacheMode CacheMode {get; set;} = ResourceLoader.CacheMode.Reuse;
    }
}
