using Godot;
using System;
using Scripts.Combat.States;
using Scripts.Characters;

namespace Scripts.Combat.Nodes {
    public class CombatCharacterManager : Node {
        [Export]
        public PackedScene Character;

        public override void _Ready() {

        }

        public void AddNewCharacter() {

        }

        public void RemoveCharacter() {

        }

        public void CreateDebugNPC() {
            var baseChar = new Character();
            var state = new CharacterState(baseChar, CharacterState.Type.PC);
            var character = Character.Instance();
            ((CharacterBody)character).State = state;
            ((KinematicBody)character).Scale = new Vector3(0.5f, 0.5f, 0.5f);
            ((KinematicBody)character).Translation = new Vector3(1.5f, 0.51f, 1.5f);
            AddChild(character);
        }
    }
}
