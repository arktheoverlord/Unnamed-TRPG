using Godot;
using System.Collections.Generic;
using System.Linq;
using TRPG.UI.Combat;

namespace TRPG {
    public class GameManager : Node {
        [Export]
        public PackedScene CombatManager;

        public static GridMap currentBattlefield { get; private set; }

        public override void _Ready() {
            CallDeferred(nameof(StartCombatDeffered), CombatScenePaths.Debug);
        }

        public override void _Process(float delta) {
            if (Input.IsActionJustPressed("Debug")) {
                Vector3 start = new Vector3(-3, 6, -3);
                Vector3 target = new Vector3(-1, 2, -1);
                var vecs = GetMoveArea();
                var scene = (PackedScene)GD.Load("res://Scenes/UI/Combat/BlueAreaHighlight.tscn");
                foreach (var vec in vecs) {
                    System.Console.WriteLine(vec);
                    var h = (Area)scene.Instance();
                    h.Translation = vec;
                    GetTree().GetRoot().GetNode<Node>("CombatManager").GetNode<Area>("CombatCursor").AddChild(h);
                }
            }
        }

        private void StartCombatDeffered(string path) {
            var scene = (PackedScene)GD.Load(path);
            currentBattlefield = (GridMap)scene.Instance();
            var cursorStart = currentBattlefield.GetNode<Position3D>("CursorStartPosition").Translation;
            GetTree().GetRoot().AddChild(currentBattlefield);

            var combatManagerInstance = CombatManager.Instance();
            combatManagerInstance.GetNode<Area>("CombatCursor").Translation = cursorStart;
            GetTree().GetRoot().AddChild(combatManagerInstance);
        }

        public void EndCombat() {
            CallDeferred(nameof(EndCombatDeffered));
        }

        private void EndCombatDeffered() {
            currentBattlefield.Free();
        }

        #region Debug
        private List<Vector3> GetMoveArea() {
            var movementVectors = new Dictionary<Vector3, float>();
            var cursor = GetTree().GetRoot().GetNode<Node>("CombatManager").GetNode<CombatCursor>("CombatCursor");
            for (int x = 0; x < 3 + 1; x++) {
                int zStart = 3 - x;
                for (int z = zStart; z > -zStart - 1; z--) {
                    var vec = new Vector3(x * 2, 0, z * 2);
                    var offsect = new Vector3(0, GetYOffset(vec + cursor.Translation), 0);

                    if (!movementVectors.ContainsKey(vec + offsect))
                        movementVectors.Add(vec + offsect, (Mathf.Abs(vec.x) + Mathf.Abs(vec.z)) / 2);

                    vec *= new Vector3(-1, 1, -1);
                    offsect = new Vector3(0, GetYOffset(vec + cursor.Translation), 0);

                    if (!movementVectors.ContainsKey(vec + offsect))
                        movementVectors.Add(vec + offsect, (Mathf.Abs(vec.x) + Mathf.Abs(vec.z)) / 2);
                }
            }
            return movementVectors.Keys.ToList();
        }

        private int GetYOffset(Vector3 target) {
            int x = ((((int)target.x) - 1) / 2);
            int y = ((int)target.y) / 2;
            int z = ((((int)target.z) - 1) / 2);

            int offset = 0;
            int cell = GameManager.currentBattlefield.GetCellItem(x, y, z);

            while (cell != -1) {
                offset += 1;
                cell = GameManager.currentBattlefield.GetCellItem(x, y + offset, z);
            }

            if (offset != 0)
                return offset * 2;

            for (int i = 1; i < 3; i++) {
                if (GameManager.currentBattlefield.GetCellItem(x, y - i, z) == -1)
                    offset -= 2;
                else
                    break;
            }
            return offset;
        }
        #endregion
    }

    public class CombatScenePaths {
        public const string Debug = "res://Scenes/Debug/DebugWorld.tscn";
        public const string Debug2 = "res://Scenes/Debug/DebugWorld2.tscn";
    }
}