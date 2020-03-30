using Godot;
using Godot.Collections;
using TRPG.Combat.Mapping;
using TRPG.Characters;
using TRPG.Helper;

namespace TRPG.Combat.Nodes {
    public class AreaHighlight : Area {
        public override void _Ready() {
        }

        private void OnBodyEntered(Node body) {
            if(body.GetType() != typeof(GridMap)){
                QueueFree();
            }
        }
    }
}