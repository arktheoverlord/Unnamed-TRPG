using Godot;
using Scripts.Combat.States;

namespace Scripts.Combat.Nodes {
    public class CharacterBody : KinematicBody {
        public int CharacterID { get; set; } = 0;
        public Team Team { get; set; }

        public Sprite3D Sprite;

        public override void _Ready(){
            Sprite = GetNode<Sprite3D>("Sprite");
        }

        public void MoveTo(Vector3 target){
            MoveAndSlide(target);
        }
    }
}