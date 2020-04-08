using Godot;
using Godot.Collections;
using TRPG.Combat.Mapping;
using TRPG.Characters;

namespace TRPG.Combat.Nodes {
    public class AreaHighlight : Sprite3D {
        public override void _Ready() {
            Translation += new Vector3(0, 0.01f, 0);
        }
    }
}