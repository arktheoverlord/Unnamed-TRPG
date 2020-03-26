using Godot;
using Scripts.Combat.States;
using System.Collections.Generic;

namespace Scripts.Combat.Nodes {
    public class CharacterBody : KinematicBody {
        public CharacterState State { get; set; }

        [Export]
        private PackedScene BlueAreaHighlight;

        private Sprite3D sprite;
        private Spatial movementArea;

        private List<Vector3> movementVectors;

        public override void _Ready() {
            sprite = GetNode<Sprite3D>("Sprite");
            movementArea = GetNode<Spatial>("MovementArea");
        }

        public void MoveTo(Vector3 target) {
            MoveAndSlide(target);
        }

        public void DisplayMovementArea() {
            GetMoveArea();
            foreach (var vec in movementVectors) {
                var instance = BlueAreaHighlight.Instance();
                ((Sprite3D)instance).Translation = vec;
                ((Sprite3D)instance).Scale *= new Vector3(2, 2, 2);
                movementArea.AddChild(instance);
            }
            movementArea.Visible = true;
        }

        private void GetMoveArea() {
            List<Vector3> area = new List<Vector3>();
            for (int x = (int)State.BaseCharacter.Move; x > 0; x--) {
                for (int z = ((int)State.BaseCharacter.Move) - x; z >= 0; z--) {
                    area.Add(new Vector3(x, -0.5f, z));
                    area.Add(new Vector3(-x, -0.5f, z));
                    if (z > 0) {
                        area.Add(new Vector3(x, -0.5f, -z));
                        area.Add(new Vector3(-x, -0.5f, -z));
                    }
                }
            }

            for (int z = (int)State.BaseCharacter.Move; z > (int)-State.BaseCharacter.Move - 1; z--) {
                area.Add(new Vector3(0, -0.5f, z));
            }

            area.Remove(new Vector3(0, -0.5f, 0));

            //Because the character scene is downscaled by a half, the area vectors need to be upscaled by 2, 
            //otherwise they'er downscaled by a half when added to the scene
            for(int i = 0; i < area.Count; i++){
                area[i] *= 2;
            }
            movementVectors = area;
        }

        public void RemoveMovementArea() {
            GD.Print("Removing area");
            movementArea.Visible = false;
            foreach(var child in movementArea.GetChildren()){
                ((Node) child).QueueFree();
            }
        }

        public bool IsMovementSelectionValid(Vector3 selection){
            return movementVectors.Contains(selection);
        }
    }
}