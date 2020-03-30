using Godot;
using Godot.Collections;
using TRPG.Combat.States;
using System.Collections.Generic;
using TRPG.Characters;

namespace TRPG.Combat.Nodes {
    public class CharacterBody : KinematicBody {
        public CharacterState State { get; set; }

        [Export]
        private PackedScene BlueAreaHighlight;

        private Sprite3D sprite;
        private Spatial movementArea;
        private PhysicsDirectSpaceState spaceState;

        private float CharacterOffset = -1f;

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
            movementArea.Visible = true;
            foreach (var vec in movementVectors) {
                var instance = (Area)BlueAreaHighlight.Instance();
                instance.Translation = vec;
                instance.Visible = false;
                movementArea.AddChild(instance);
            }
        }

        private void GetMoveArea() {
            movementVectors = new List<Vector3>();
            for (int x = (int)State.BaseCharacter.Move; x > 0; x--) {
                for (int z = ((int)State.BaseCharacter.Move) - x; z >= 0; z--) {
                    var vec = new Vector3(x * 2, CharacterOffset, z * 2);
                    int offset = GetYOffset(vec + Translation);
                    vec += new Vector3(0, offset, 0);
                    movementVectors.Add(vec);

                    vec = new Vector3(-x * 2, CharacterOffset, z * 2);
                    offset = GetYOffset(vec + Translation);
                    vec += new Vector3(0, offset, 0);
                    movementVectors.Add(vec);
                    if (z > 0) {
                        vec = new Vector3(x * 2, CharacterOffset, -z * 2);
                        offset = GetYOffset(vec + Translation);
                        vec += new Vector3(0, offset, 0);
                        movementVectors.Add(vec);

                        vec = new Vector3(-x * 2, CharacterOffset, -z * 2);
                        offset = GetYOffset(vec + Translation);
                        vec += new Vector3(0, offset, 0);
                        movementVectors.Add(vec);
                    }
                }
            }

            for (int z = (int)State.BaseCharacter.Move; z > (int)-State.BaseCharacter.Move - 1; z--) {
                var vec = new Vector3(0, CharacterOffset, z * 2);
                int offset = GetYOffset(vec + Translation);
                vec += new Vector3(0, offset, 0);
                movementVectors.Add(vec);
            }

            movementVectors.Remove(new Vector3(0, CharacterOffset, 0));
        }

        private int GetYOffset(Vector3 target) {
            int x = ((((int)target.x) - 1) / 2);
            int y = ((int)target.y) / 2;
            int z = ((((int)target.z) - 1) / 2);
            //GD.Print(x + " " + y + " " + z);
            int offset = 0;
            int cell = GameManager.currentBattlefield.GetCellItem(x, y, z);
            GD.Print(cell + " " + offset);
            GD.Print(State.GetStatTotal(Stat.Jump));
            while (cell != -1 && offset < State.GetStatTotal(Stat.Jump)) {
                offset += 1;
                cell = GameManager.currentBattlefield.GetCellItem(x, y + offset, z);
            }
            GD.Print(cell + " " + offset);

            if (offset != 0)
                return offset * 2;
            GD.Print(cell + " " + offset);

            for (int i = 0; i < State.GetStatTotal(Stat.Jump); i++) {
                if (GameManager.currentBattlefield.GetCellItem(x, y - i, z) == -1)
                    offset -= 2;
                else
                    break;
            }
            return offset;
        }

        public void RemoveMovementArea() {
            movementArea.Visible = false;
            foreach (var child in movementArea.GetChildren()) {
                ((Node)child).QueueFree();
            }
        }

        public bool IsMovementSelectionValid(Vector3 selection) {
            foreach (var vec in movementVectors) {
                if (vec + Translation == selection)
                    return true;
            }
            return false;
        }
    }
}