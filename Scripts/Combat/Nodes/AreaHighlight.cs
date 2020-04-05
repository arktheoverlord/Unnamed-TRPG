using Godot;
using Godot.Collections;
using TRPG.Combat.Mapping;
using TRPG.Characters;

namespace TRPG.Combat.Nodes {
    public class AreaHighlight : Area {
        public override void _Ready() {
            Translation += new Vector3(0, 0.01f, 0);
        }

        public override void _Process(float delta) {
            if (!Visible)
                Visible = true;
        }
    }
}