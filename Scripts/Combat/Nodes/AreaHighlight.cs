using Godot;
using Godot.Collections;
using TRPG.Combat.Mapping;
using TRPG.Characters;
using TRPG.Helper;

namespace TRPG.Combat.Nodes {
    public class AreaHighlight : Area {

        private PhysicsDirectSpaceState spaceState;

        public override void _Ready() {
            spaceState = GetWorld().GetDirectSpaceState();
            
            if(!WithinMap()){
                QueueFree();
            }
        }


        private bool WithinMap() {
            var cast = spaceState.IntersectRay(Translation + Vector3.Up * 2, Translation - (Vector3.Up * 100), new Array() { this });
            //GD.Print("Translation: " + Translation + ", Start: " + (Translation + Vector3.Up) + ", Target: " + (Translation - (Vector3.Up * 100)));

            if (cast.Count == 0) {
                GD.Print("Translation: " + Translation + ", Start: " + (Translation + Vector3.Up * 2) + ", Target: " + (Translation - (Vector3.Up * 100)));
                return false;
            }

            return true;
        }

        private void OnAreaEntered(Area area) {

        }

        private void OnBodyEntered(Node body) {
            QueueFree();
        }
    }
}