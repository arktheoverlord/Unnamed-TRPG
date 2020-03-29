using Godot;
using Godot.Collections;
using TRPG.Combat.Mapping;

namespace TRPG.Helper {
    public static class PhysicsHelper {
        public static int GetYOffset(Spatial caster, Vector3 dir, PhysicsDirectSpaceState space) {
            return CastUp(caster, dir, space) + CastDown(caster, dir, space);
        }

        private static int CastUp(Spatial caster, Vector3 dir, PhysicsDirectSpaceState space) {
            int level = 0;
            var start = caster.Translation + new Vector3(0, 0.5f, 0);
            var target = new Vector3(start.x + dir.x, start.y + 1, start.z + dir.z);
            var cast = space.IntersectRay(start, target, new Array { caster });
            while (cast.Count > 0) {
                level++;
                start += Vector3.Up;
                target += Vector3.Up;
                cast = space.IntersectRay(start, target);
            }
            return level;
        }

        private static int CastDown(Spatial caster, Vector3 dir, PhysicsDirectSpaceState space) {
            int level = 0;
            var start = caster.Translation + new Vector3(0, 0.5f, 0) + dir;
            var target = start + new Vector3(0, -1f, 0);
            var cast = space.IntersectRay(start, target, new Array { caster });

            while (cast.Count == 0) {
                level--;
                start -= Vector3.Up;
                target -= Vector3.Up;
                cast = space.IntersectRay(start, target);
            }
            return level;
        }

        public static bool IsTargetWithinMap(Spatial caster, Vector3 target, PhysicsDirectSpaceState space) {
            //Raycast from the center of the cursor to the target
            var cast = space.IntersectRay(caster.Translation + new Vector3(0, 0.5F, 0), target, new Array() { caster }, collideWithAreas: true);
            //Did the raycast hit a collider?
            if (cast == null || cast.Count == 0) {
                //The raycast didn't hit a collider, so raycast from the center of the targt to a point 100 units below the target
                cast = space.IntersectRay(target, target - new Vector3(0, 100, 0), new Array() { caster }, collideWithAreas: true);
                if (cast != null && cast.Count > 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                //The raycast hit a collider, so the collider must be in the map
                return true;
            }

        }
    }
}