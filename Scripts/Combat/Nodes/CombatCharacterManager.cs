using Godot;
using System;
using TRPG.Combat.States;
using TRPG.Characters;

namespace TRPG.Combat.Nodes {
    public class CombatCharacterManager : Node {
        [Export]
        public PackedScene Character;

        private bool isCharacterMoving;
        private CharacterBody movingCharacter;

        #region Signals
        [Signal]
        public delegate void CharacterAdded(Character character, CombatCharacterManager ccm, Team team);

        [Signal]
        public delegate void ValidMovementLocationSelected(CharacterBody body);
        #endregion
        public override void _Ready() {

        }

        public override void _Process(float delta) {
            if (Input.IsActionJustPressed("Debug")) {
                //CreateDebugNPC();
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
            EmitSignal(nameof(CharacterAdded), character, this, Team.PC);
        }

        public void OnMoveButtonPressed(CharacterBody body) {
            isCharacterMoving = true;
            movingCharacter = body;
            movingCharacter.DisplayMovementArea();
        }

        public void OnMovementLocationSelected(Vector3 location){
            if(movingCharacter.IsMovementSelectionValid(location)){
                EmitSignal(nameof(ValidMovementLocationSelected), movingCharacter);
                movingCharacter.MoveTo(location);
            }
        }
    }
}
