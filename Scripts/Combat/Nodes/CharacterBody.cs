using Godot;
using TRPG.Combat.States;
using TRPG.Characters;
using TRPG.Combat.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace TRPG.Combat.Nodes {
    public class CharacterBody : KinematicBody {
        public CharacterState State { get; set; }

        [Export]
        private PackedScene BlueAreaHighlight;

        private Sprite3D sprite;
        private Spatial movementArea;
        private PhysicsDirectSpaceState spaceState;

        private float characterOffset = -1f;

        private List<Vector3> movementVectors;

        public override void _Ready() {
            sprite = GetNode<Sprite3D>("Sprite");
            movementArea = GetNode<Spatial>("MovementArea");
            spaceState = GetWorld().DirectSpaceState;
            AddToGroup("Units");
        }

        public void MoveTo(Vector3 target) {
            RemoveMovementArea();
            Translation = target + Vector3.Up;
        }

        public void DisplayMovementArea() {
            GetMoveArea();
            ValidateMovementVectors();
            movementVectors.Remove(new Vector3(0, characterOffset, 0));
            movementArea.Visible = true;
            foreach (var vec in movementVectors) {
                var instance = (Sprite3D)BlueAreaHighlight.Instance();
                instance.Translation = vec;
                movementArea.AddChild(instance);
            }
        }

        private void GetMoveArea() {
            movementVectors = new List<Vector3>();
            for (int x = 0; x < (int)State.GetStatTotal(Stat.Move) + 1; x++) {
                int zStart = ((int)State.GetStatTotal(Stat.Move)) - x;
                for (int z = zStart; z > -zStart - 1; z--) {
                    var vec = new Vector3(x * 2, characterOffset, z * 2);
                    var offsect = new Vector3(0, GetYOffset(vec + Translation), 0);

                    if (!movementVectors.Contains(vec + offsect)) {
                        movementVectors.Add(vec + offsect);
                    }

                    vec *= new Vector3(-1, 1, -1);
                    offsect = new Vector3(0, GetYOffset(vec + Translation), 0);

                    if (!movementVectors.Contains(vec + offsect)) {
                        movementVectors.Add(vec + offsect);
                    }
                }
            }
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

            if (offset != 0) {
                return offset * 2;
            }

            cell = GameManager.currentBattlefield.GetCellItem(x, y - 1, z);

            while (cell == -1) {
                offset += 1;
                cell = GameManager.currentBattlefield.GetCellItem(x, y - offset - 1, z);

                if (offset > 50) {
                    break;
                }
            }

            return offset * -2;
        }

        private void ValidateMovementVectors() {
            var invalid = new List<Vector3>();
            foreach (var vec in movementVectors) {
                if (!IsTargetWithinMap(vec + Translation)) {
                    invalid.Add(vec);
                }
            }

            for (int i = 0; i < movementVectors.Count; i++) {
                var vec = movementVectors[i];

                if (!invalid.Contains(vec)) {
                    var pathfinder = new Pathfinder(vec, new Vector3(0, characterOffset, 0));

                    List<Vector3> path = null;
                    int ii = 0;

                    do {
                        path = pathfinder.FindPath(movementVectors, State.GetStatTotal(Stat.Jump));

                        ii++;
                        if (ii > 10) {
                            break;
                        }

                    } while (path == null);

                    if (path == null) {
                        invalid.Add(vec);
                    }
                    else if (path.Count - 1 > State.GetStatTotal(Stat.Move)) {
                        invalid.Add(vec);
                    }

                    if(!GetParent().GetParent().GetNode<CombatStateMachine>("CombatStateMachine").IsPositionOpen(vec + Translation)){
                        invalid.Add(vec);
                    }
                }
            }

            foreach (var inv in invalid) {
                if (movementVectors.Contains(inv)) {
                    movementVectors.Remove(inv);
                }
            }
        }

        private bool IsTargetWithinMap(Vector3 target) {
            var x = ((((int)target.x) - 1) / 2);
            var y = ((int)target.y) / 2;
            var z = ((((int)target.z) - 1) / 2);

            for (int i = 0; i < 51; i++) {
                if (GameManager.currentBattlefield.GetCellItem(x, y - i, z) != -1) {
                    return true;
                }
            }

            return false;
        }

        public void RemoveMovementArea() {
            movementArea.Visible = false;
            foreach (var child in movementArea.GetChildren()) {
                ((Node)child).QueueFree();
            }
        }

        public bool IsMovementSelectionValid(Vector3 selection) {
            foreach (var vec in movementVectors) {
                if (vec + Translation == selection) {
                    return true;
                }
            }
            return false;
        }

        private class MovementVector {
            public int moveCost;
            public Vector3 vector;
        }
    }
}