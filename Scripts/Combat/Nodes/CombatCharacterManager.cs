using Godot;
using System;
using Scripts.Combat.States;
using Scripts.Characters;

namespace Scripts.Combat.Nodes {
    public class CombatCharacterManager : Node {
        [Export]
        public PackedScene Character;

        #region Signals
        [Signal]
        public delegate void OnCharacterAdded(Character character, CombatCharacterManager ccm, Team team);
        #endregion
        public override void _Ready() {

        }

        public override void _Process(float delta){
            if(Input.IsActionJustPressed("Debug")){
                CreateDebugNPC();
            }
        }

        public void AddCharacter(int characterID, Vector3 location) {
            var character = Character.Instance();
            ((CharacterBody) character).CharacterID = characterID;
            ((CharacterBody) character).Translation = location;
            AddChild(character);
        }

        public void RemoveCharacter(int characterID) {

        }

        public void CreateDebugNPC() {
            var character = new Character();
            EmitSignal(nameof(OnCharacterAdded), character, this, Team.PC);
        }
    }
}
