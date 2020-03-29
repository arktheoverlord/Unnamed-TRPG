using Godot;

namespace TRPG {
    public class GameManager : Node {
        [Export]
        public PackedScene CombatManager;

        public override void _Ready(){
            CallDeferred(nameof(StartCombat));
        }

        public void StartCombat(){
            var scene = (PackedScene) GD.Load(CombatScenePaths.Debug);
            var sceneInstance = (GridMap) scene.Instance();
            var cursorStart = sceneInstance.GetNode<Position3D>("CursorStartPosition").Translation;
            GetTree().GetRoot().AddChild(sceneInstance);
            AddChild(CombatManager.Instance());
        }

    }

    public class CombatScenePaths{
        public const string Debug = "res://Scenes/Debug/DebugWorld.tscn";
    }
}