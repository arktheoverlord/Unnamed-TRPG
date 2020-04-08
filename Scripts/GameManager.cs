using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRPG.UI.Combat;
using TRPG.Combat.Mapping;

namespace TRPG {
    public class GameManager : Node {
        [Export]
        public PackedScene CombatManager;

        public static GridMap currentBattlefield { get; private set; }
        public static Node CMI { get; private set; }

        public override void _Ready() {
            CallDeferred(nameof(StartCombatDeffered), CombatScenePaths.Debug);
        }

        public override void _Process(float delta) {
            if (Input.IsActionJustPressed("Debug")) {
            }
        }

        private void StartCombatDeffered(string path) {
            var scene = (PackedScene)GD.Load(path);
            currentBattlefield = (GridMap)scene.Instance();
            var cursorStart = currentBattlefield.GetNode<Position3D>("CursorStartPosition").Translation;
            GetTree().GetRoot().AddChild(currentBattlefield);

            CMI = CombatManager.Instance();
            CMI.GetNode<Area>("CombatCursor").Translation = cursorStart;
            GetTree().GetRoot().AddChild(CMI);
        }

        public void EndCombat() {
            CallDeferred(nameof(EndCombatDeffered));
        }

        private void EndCombatDeffered() {
            currentBattlefield.Free();
        }
    }

    public class CombatScenePaths {
        public const string Debug = "res://Scenes/Debug/DebugWorld.tscn";
        public const string Debug2 = "res://Scenes/Debug/DebugWorld2.tscn";
    }
}