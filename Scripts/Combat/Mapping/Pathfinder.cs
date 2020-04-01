using Godot;
using System.Collections.Generic;
using System.Linq;

namespace TRPG.Combat.Mapping {
    public class Pathfinder {
        private TileScore current;
        private List<TileScore> open;
        private List<TileScore> closed;

        private const float characterOffset = -1.0f;

        public List<Vector3> FindPath(Vector3 start, Vector3 target, List<Vector3> area, float jump) {
            open = new List<TileScore>();
            closed = new List<TileScore>();
            current = new TileScore(start);
            int iter = 0;

            while (current.Tile != target) {
                iter++;
                var neighbors = FindNeighbors(current.Tile, area, jump);

                foreach (var vec in neighbors) {
                    var tScore = new TileScore(vec);
                    tScore.HScore = Mathf.Sqrt(vec.x + (vec.z * vec.z));
                    tScore.Parent = current;
                    open.Add(tScore);
                }

                var lowest = open[open.Count - 1];
                open.Remove(lowest);

                if (start == new Vector3(-2, 3, -2)) {
                    System.Console.WriteLine(lowest.Tile);
                }

                current = lowest;
                closed.Add(lowest);
                if (iter > 20) {
                    break;
                }
            }

            var tiles = new List<Vector3>();

            foreach (var tScore in closed) {
                tiles.Add(tScore.Tile);
            }

            return tiles;
        }

        private List<Vector3> FindNeighbors(Vector3 start, List<Vector3> area, float jump) {
            List<Vector3> neighbors = new List<Vector3>();
            var xPos = area.Where(v => v.x - 2 == start.x && v.z == start.z).FirstOrDefault();
            var xNeg = area.Where(v => v.x + 2 == start.x && v.z == start.z).FirstOrDefault();
            var zPos = area.Where(v => v.x == start.x && v.z - 2 == start.z).FirstOrDefault();
            var zNeg = area.Where(v => v.x == start.x && v.z + 2 == start.z).FirstOrDefault();

            if ((start.y - xPos.y) / 2 <= jump)
                neighbors.Add(xPos);
            if ((start.y - xNeg.y) / 2 <= jump)
                neighbors.Add(xNeg);
            if ((start.y - zPos.y) / 2 <= jump)
                neighbors.Add(zPos);
            if ((start.y - zNeg.y) / 2 <= jump)
                neighbors.Add(zNeg);

            /*if (start == new Vector3(0, 3, -2) || start == new Vector3(-2, 3, -2)) {
                System.Console.WriteLine(start);
                foreach (var vec in neighbors) {
                    System.Console.WriteLine(vec);
                }
                System.Console.WriteLine();
            }*/

            return neighbors;
        }

        private class TileScore {
            public Vector3 Tile;
            public TileScore Parent;

            public float HScore = 0;
            public float GScore {
                get {
                    return Parent == null ? 1 : Parent.GScore + 1;
                }
            }
            public float FScore {
                get {
                    return HScore + GScore;
                }
            }

            public TileScore(Vector3 tile) {
                Tile = tile;
            }
        }
    }
}