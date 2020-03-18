using Godot;
using Scripts.Combat.States;

namespace Scripts.Combat {
    public class CharacterBody : KinematicBody {
        public CharacterState State {get; set;}

        public Sprite3D Sprite;

        public override void _Ready(){
            Sprite = GetNode<Sprite3D>("Sprite");
        }

        public void MoveTo(Vector3 target){
            MoveAndSlide(target);
        }
    }
}