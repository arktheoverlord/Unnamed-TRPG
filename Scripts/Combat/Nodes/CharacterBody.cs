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

        private Dictionary<Vector3, float> movementVectors;

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
            movementArea.Visible = true;
            foreach (var vec in movementVectors.Keys) {
                var instance = (Area)BlueAreaHighlight.Instance();
                instance.Translation = vec;
                movementArea.AddChild(instance);
            }
        }

        private void GetMoveArea() {
            movementVectors = new Dictionary<Vector3, float>();
            for (int x = 0; x < (int)State.GetStatTotal(Stat.Move) + 1; x++) {
                int zStart = ((int)State.GetStatTotal(Stat.Move)) - x;
                for (int z = zStart; z > -zStart - 1; z--) {
                    var vec = new Vector3(x * 2, characterOffset, z * 2);
                    var offsect = new Vector3(0, GetYOffset(vec + Translation), 0);

                    if (!movementVectors.ContainsKey(vec + offsect))
                        movementVectors.Add(vec + offsect, (Mathf.Abs(vec.x) + Mathf.Abs(vec.z)) / 2);

                    vec *= new Vector3(-1, 1, -1);
                    offsect = new Vector3(0, GetYOffset(vec + Translation), 0);

                    if (!movementVectors.ContainsKey(vec + offsect))
                        movementVectors.Add(vec + offsect, (Mathf.Abs(vec.x) + Mathf.Abs(vec.z)) / 2);
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

            if (offset != 0)
                return offset * 2;

            for (int i = 1; i < State.GetStatTotal(Stat.Jump); i++) {
                if (GameManager.currentBattlefield.GetCellItem(x, y - i, z) == -1)
                    offset -= 2;
                else
                    break;
            }
            return offset;
        }

        private void ValidateMovementVectors() {
            var invalid = new List<Vector3>();
            bool changed = true;
            int iterations = 0;
            while (changed) {
                for (int i = 0; i < movementVectors.Keys.Count; i++) {

                    var vec = movementVectors.Keys.ToList()[i];
                    var vecFixed = vec - new Vector3(0, characterOffset, 0);
                    var xPosN = movementVectors.Where(k => k.Key.x - 2 == vecFixed.x && k.Key.z == vecFixed.z).FirstOrDefault().Key;
                    var xPosNFixed = xPosN - new Vector3(0, characterOffset, 0);
                    var xNegN = movementVectors.Where(k => k.Key.x + 2 == vecFixed.x && k.Key.z == vecFixed.z).FirstOrDefault().Key;
                    var xNegNFixed = xNegN - new Vector3(0, characterOffset, 0);
                    var zPosN = movementVectors.Where(k => k.Key.x == vecFixed.x && k.Key.z - 2 == vecFixed.z).FirstOrDefault().Key;
                    var zPosNFixed = zPosN - new Vector3(0, characterOffset, 0);
                    var zNegN = movementVectors.Where(k => k.Key.x == vecFixed.x && k.Key.z + 2 == vecFixed.z).FirstOrDefault().Key;
                    var zNegNFixed = zNegN - new Vector3(0, characterOffset, 0);
                    if (!movementVectors.ContainsKey(xPosN) && !movementVectors.ContainsKey(xNegN) && !movementVectors.ContainsKey(zNegN) && !movementVectors.ContainsKey(zPosN)) {
                        invalid.Add(vec);
                    }
                    else {
                        float yDiff = Mathf.Abs(vecFixed.y + characterOffset);
                        float jump = State.GetStatTotal(Stat.Jump);
                        if (yDiff > jump) {
                            if (Mathf.Abs(vecFixed.y - xPosNFixed.y) / 2 > jump && Mathf.Abs(vecFixed.y - xNegNFixed.y) / 2 > jump && Mathf.Abs(vecFixed.y - zPosNFixed.y) / 2 > jump && Mathf.Abs(vecFixed.y - zNegNFixed.y) / 2 > jump) {
                                invalid.Add(vec);
                            }
                            else {
                                var pathFinder = new Pathfinder();
                                int cost = pathFinder.FindPath(vec, new Vector3(0, characterOffset, 0), movementVectors.Keys.ToList(), jump).Count;

                                movementVectors[vec] = cost;
                                if (cost > State.GetStatTotal(Stat.Move)) {
                                    invalid.Add(vec);
                                }
                            }
                        }
                    }
                }

                if (invalid.Count > 0) {
                    changed = true;
                }
                else {
                    changed = false;
                }

                foreach (var vec in invalid) {
                    if (movementVectors.ContainsKey(vec)) {
                        movementVectors.Remove(vec);
                    }
                }
                invalid.Clear();
                iterations++;
            }
        }

        public void RemoveMovementArea() {
            movementArea.Visible = false;
            foreach (var child in movementArea.GetChildren()) {
                ((Node)child).QueueFree();
            }
        }

        public bool IsMovementSelectionValid(Vector3 selection) {
            foreach (var vec in movementVectors.Keys) {
                if (vec + Translation == selection)
                    return true;
            }
            return false;
        }

        private class MovementVector {
            public int moveCost;
            public Vector3 vector;
        }
    }
}