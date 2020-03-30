using Godot;

namespace TRPG {
    public class GameManager : Node {
        [Export]
        public PackedScene CombatManager;
        
        public static GridMap currentBattlefield { get; private set;}

        public override void _Ready(){
            CallDeferred(nameof(StartCombatDeffered), CombatScenePaths.Debug);
        }

        private void StartCombatDeffered(string path){
            var scene = (PackedScene) GD.Load(CombatScenePaths.Debug);
            currentBattlefield = (GridMap) scene.Instance();
            var cursorStart = currentBattlefield.GetNode<Position3D>("CursorStartPosition").Translation;
            GetTree().GetRoot().AddChild(currentBattlefield);

            var combatManagerInstance = CombatManager.Instance();
            combatManagerInstance.GetNode<Area>("CombatCursor").Translation = cursorStart;
            GetTree().GetRoot().AddChild(combatManagerInstance);
        }

        public void EndCombat(){
            CallDeferred(nameof(EndCombatDeffered));
        }

        private void EndCombatDeffered(){
            currentBattlefield.Free();
        }
    }

    public class CombatScenePaths{
        public const string Debug = "res://Scenes/Debug/DebugWorld.tscn";
    }
}