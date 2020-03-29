using Godot;
using Godot.Collections;
using Scripts.Combat.Mapping;

namespace Scripts.Helper {
    public static class PhysicsHelper {
        public static Vector3 GetYOffset(Spatial caster, Vector3 target, PhysicsDirectSpaceState spaceState) {
            //Raycast from the center of the cursor to the target
            var cast = spaceState.IntersectRay(caster.Translation + new Vector3(0, 0.5F, 0), target, new Array() { caster }, collideWithAreas: true, collideWithBodies: false);
            //Did raycast hit a collider and is it a tile?
            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                var y = ((Tile)cast["collider"]).GetHighestTile();
                return new Vector3(0, y, 0);
            }
            else {
                //Raycast from the center of the orginal target to a point 100 units below the original target
                cast = spaceState.IntersectRay(target + new Vector3(0, 0.5F, 0), target - new Vector3(0, 100, 0), new Array() { caster }, collideWithAreas: true, collideWithBodies: false);
                //Did the raycast hit a collider and is it a tile?
                if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                    //Find the different in height between the translation of the cursor and the translation of the tile
                    int y = (int)(caster.Translation.y - ((Tile)cast["collider"]).Translation.y);
                    return new Vector3(0, -y, 0);
                }
            }
            return new Vector3(-1, -1, -1);
        }

    }
}