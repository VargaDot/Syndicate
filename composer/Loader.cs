using Godot;
using System.Collections.Generic;

namespace ComposerLib
{
    internal class LoaderScene
    {
        internal Scene Scene {get; set;}
        internal bool UseSubthreads {get; set;} = false;
        internal ResourceLoader.CacheMode CacheMode = ResourceLoader.CacheMode.Reuse;
    }

    internal partial class Loader : Node
    {
        [Signal]
        internal delegate void LoaderStartedEventHandler(Scene scene);

        [Signal]
        internal delegate void LoaderLoadingUpdatedEventHandler(Scene scene, float progress);

        [Signal]
        internal delegate void LoaderFinishedEventHandler(Scene scene, PackedScene resource=null);

        [Signal]
        internal delegate void LoaderAllFinishedEventHandler();

        internal Composer Composer;
        internal bool IsWorking {
            get
            {
                return ProcessMode == ProcessModeEnum.Inherit;
            }
        }
        private static Queue<LoaderScene> SceneQueue = new();
        private static LoaderScene CurrentLoadedObject = null;

        public override void _EnterTree()
        {
            Disable();
        }

        internal static void AddToQueue(LoaderScene scene)
        {
            SceneQueue.Enqueue(scene);
        }

        public override void _Process(double delta)
        {
            if (CurrentLoadedObject == null)
            {
                if (SceneQueue.Count > 0)
                {
                    BeginNewLoad();
                }
                else return;
            }

            Godot.Collections.Array progress = new();

            switch (ResourceLoader.LoadThreadedGetStatus(CurrentLoadedObject.Scene.PathToResource, progress))
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
                {
                    EmitSignal(SignalName.LoaderLoadingUpdated, (float)progress[0]);
                    break;
                }
                case ResourceLoader.ThreadLoadStatus.Loaded:
                {
                    var resource = (PackedScene)ResourceLoader.LoadThreadedGet(CurrentLoadedObject.Scene.PathToResource);
                    EmitSignal(SignalName.LoaderFinished, CurrentLoadedObject.Scene, resource);
                    EndLoad();
                    break;
                }
                case ResourceLoader.ThreadLoadStatus.Failed: case ResourceLoader.ThreadLoadStatus.InvalidResource:
                {
                    EmitSignal(SignalName.LoaderFinished, CurrentLoadedObject.Scene);
                    EndLoad();
                    break;
                }
            }
        }

        internal void Enable()
        {
            SetDeferred(PropertyName.ProcessMode, (int)ProcessModeEnum.Inherit);
        }

        internal async void Disable()
        {
            if (CurrentLoadedObject != null)
            {
                GD.Print("Loader is loading an object, awaiting process to end...");
                await ToSignal(this, SignalName.LoaderFinished);
                GD.Print("Object Loaded, proceeding to disable...");
            }

            SetDeferred(PropertyName.ProcessMode, (int)ProcessModeEnum.Disabled);
        }

        private void BeginNewLoad()
        {
            CurrentLoadedObject = SceneQueue.Dequeue();
            LoaderFinished += CurrentLoadedObject.Scene.OnLoaded;
            EmitSignal(SignalName.LoaderStarted, CurrentLoadedObject.Scene);
            ResourceLoader.LoadThreadedRequest(CurrentLoadedObject.Scene.PathToResource, "PackedScene", CurrentLoadedObject.UseSubthreads, CurrentLoadedObject.CacheMode);
        }

        private void EndLoad()
        {
            LoaderFinished -= CurrentLoadedObject.Scene.OnLoaded;
            CurrentLoadedObject = null;

            if (SceneQueue.Count == 0)
            {
                EmitSignal(SignalName.LoaderAllFinished);
            }
        }
    }
}