using Godot;
using System;
using Scripts.Combat.States;
using Scripts.Characters;

namespace Scripts.Combat.Nodes {
    public class CombatCharacterManager : Node {
        [Export]
        public PackedScene Character;

        private bool isCharacterMoving;
        private CharacterBody movingCharacter;

        #region Signals
        [Signal]
        public delegate void OnCharacterAdded(Character character, CombatCharacterManager ccm, Team team);
        #endregion
        public override void _Ready() {

        }

        public override void _Process(float delta) {
            if (Input.IsActionJustPressed("Debug")) {
                CreateDebugNPC();
            }

            if(Input.IsActionJustPressed("Back") && isCharacterMoving){
                isCharacterMoving = false;
                movingCharacter.RemoveMovementArea();
            }
        }

        public void AddCharacter(CharacterState state, Vector3 location) {
            var character = Character.Instance();
            ((CharacterBody)character).State = state;
            ((CharacterBody)character).Translation = location;
            AddChild(character);
        }

        public void RemoveCharacter(int characterID) {

        }

        public void CreateDebugNPC() {
            var character = new Character();
            EmitSignal(nameof(OnCharacterAdded), character, this, Team.PC);
        }

        public void OnMoveButtonPressed(CharacterState state) {
            GD.Print("Got here");
            var children = GetChildren();

            foreach (var child in children) {
                if (((CharacterBody)child).State == state) {
                    ((CharacterBody)child).DisplayMovementArea();
                    isCharacterMoving = true;
                    movingCharacter = (CharacterBody)child;
                }
            }
        }

        public void RemoveMovementArea() {
            
        }
    }
}
