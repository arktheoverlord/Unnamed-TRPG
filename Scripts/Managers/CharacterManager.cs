using Godot;
using System.Collections.Generic;
using TRPG.Characters;

namespace TRPG.Managers {
    public class CharacterManager : Node {
        private List<Character> characters;

        public override void _Ready() {
            characters = new List<Character>();
        }
    }
}
