using Godot;
using Godot.Collections;
using Scripts.Combat.States;
using System.Collections.Generic;

namespace Scripts.Combat.Nodes {
    public class CharacterBody : KinematicBody {
        public CharacterState State { get; set; }

        [Export]
        private PackedScene BlueAreaHighlight;

        private Sprite3D sprite;
        private Spatial movementArea;
        private PhysicsDirectSpaceState spaceState;

        private float CharacterOffset = -1.01f;

        private List<Vector3> movementVectors;

        public override void _Ready() {
            sprite = GetNode<Sprite3D>("Sprite");
            movementArea = GetNode<Spatial>("MovementArea");
            spaceState = GetWorld().DirectSpaceState;
            AddToGroup("Units");
        }

        public void MoveTo(Vector3 target) {
            MoveAndSlide(target);
        }

        public void DisplayMovementArea() {
            GetMoveArea();
            foreach (var vec in movementVectors) {
                var instance = (Area) BlueAreaHighlight.Instance();
                instance.Translation = vec;
                instance.Scale *= 2;
                movementArea.AddChild(instance);
            }
            movementArea.Visible = true;
        }

        private void GetMoveArea() {
            List<Vector3> area = new List<Vector3>();
            for (int x = (int)State.BaseCharacter.Move; x > 0; x--) {
                for (int z = ((int)State.BaseCharacter.Move) - x; z >= 0; z--) {
                    area.Add(new Vector3(x * 2, CharacterOffset, z * 2));
                    area.Add(new Vector3(-x * 2, CharacterOffset, z * 2));
                    if (z > 0) {
                        area.Add(new Vector3(x * 2, CharacterOffset, -z * 2));
                        area.Add(new Vector3(-x * 2, CharacterOffset, -z * 2));
                    }
                }
            }

            for (int z = (int)State.BaseCharacter.Move; z > (int)-State.BaseCharacter.Move - 1; z--) {
                area.Add(new Vector3(0, CharacterOffset, z * 2));
            }

            movementVectors = area;
        }

        public void RemoveMovementArea() {
            GD.Print("Removing area");
            movementArea.Visible = false;
            foreach (var child in movementArea.GetChildren()) {
                ((Node)child).QueueFree();
            }
        }

        public bool IsMovementSelectionValid(Vector3 selection) {
            return movementVectors.Contains(selection);
        }
    }
}