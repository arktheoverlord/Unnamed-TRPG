using System.Collections.Generic;
using Godot;

namespace Scripts.Combat.Helper {
    public class AreaFinder {
        public static List<Vector2> GetArea(Vector2 center, int length) {
            List<Vector2> area = new List<Vector2>();
            for (int x = 0; x < length; x++) {
                for (int y = x == 0 ? 1 : 0; y < length - x; y++) {
                    area.Add(new Vector2(center.x + x, center.y + y));
                    area.Add(new Vector2(center.x + x, center.y - y));
                    area.Add(new Vector2(center.x - x, center.y + y));
                    area.Add(new Vector2(center.x - x, center.y - y));
                }
            }
            return area;
        }
    }
}
