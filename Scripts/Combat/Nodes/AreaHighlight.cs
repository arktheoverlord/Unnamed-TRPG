using Godot;
using Godot.Collections;
using TRPG.Combat.Mapping;
using TRPG.Characters;
using TRPG.Helper;

namespace TRPG.Combat.Nodes {
    public class AreaHighlight : Area {
        public override void _Ready() {
            Translation += new Vector3(0, 0.01f, 0);
            if(!IsWithinMap())
                QueueFree();
        }

        public override void _Process(float delta){
            if(!Visible)
                Visible = true;
        }

        private bool IsWithinMap(){
            var origin = GetGlobalTransform().origin;
            
            int x = ((((int)origin.x) - 1) / 2);
            int y = ((int)origin.y) / 2;
            int z = ((((int)origin.z) - 1) / 2);

            for (int i = 0; i < 51; i++) {
                if (GameManager.currentBattlefield.GetCellItem(x, y - i, z) != -1)
                    return true;
            }

            return false;
        }

        private void OnBodyEntered(Node body) {
            if(body.GetType() != typeof(GridMap)){
                //QueueFree();
            }
        }
    }
}