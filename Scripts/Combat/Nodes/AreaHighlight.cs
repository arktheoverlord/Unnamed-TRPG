using Godot;
using Scripts.Combat.Mapping;
using Scripts.Characters;
using Scripts.Helper;

namespace Scripts.Combat.Nodes {
    public class AreaHighlight : Area {
        private PhysicsDirectSpaceState spaceState;

        public override void _Ready() {
            spaceState = GetWorld().GetDirectSpaceState();
        }

        private void OnAreaEntered(Area area){
            if(area.GetType() == typeof(Tile)){
                int tileHeight = ((Tile) area).GetHighestTile();
            }
        }

        private void OnBodyEntered(Node body) {
            QueueFree();
            if (body.GetType() == typeof(CharacterBody)) {
                QueueFree();
            }
            if (body.GetType() == typeof(Tile)) {
                float maxHeight = ((CharacterBody)GetParent().GetParent()).State.GetStatTotal(Stat.Jump);

                var yOffSet = PhysicsHelper.GetYOffset(this, Translation, spaceState);

                if (yOffSet.z == -1) {
                    QueueFree();
                    return;
                }

                if (yOffSet.y > maxHeight || yOffSet.y < -maxHeight) {
                    QueueFree();
                    return;
                }

                Translation += yOffSet;
            }
        }
    }
}