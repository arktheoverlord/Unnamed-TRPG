using Godot;
using System.Collections.Generic;
using System.Linq;

namespace TRPG.Combat.Mapping {
    public class Pathfinder {
        private TileScore current;
        private Vector3 target;
        private List<TileScore> open;
        private List<TileScore> closed;

        public Pathfinder(Vector3 start, Vector3 target){
            open = new List<TileScore>();
            closed = new List<TileScore>();
            current = new TileScore(start);
            closed.Add(current);
            this.target = target;
        }

        public List<Vector3> FindPath(List<Vector3> area, float maxAllowedHeight) {
            if(current.Tile == target) {
                return GetPath();
            }
            else {
                var neighbors = FindNeighbors(current.Tile, area, maxAllowedHeight);
                
                if(neighbors.Count == 0)
                    return null;

                foreach (var vec in neighbors) {
                    var tScore = new TileScore(vec);
                    tScore.HScore = Mathf.Sqrt(Mathf.Abs(vec.x) + (vec.z * vec.z));
                    tScore.Parent = current;

                    if(!IsTileInClosed(vec))
                        open.Add(tScore);
                }

                var lowest = open[open.Count - 1];

                foreach(var tScore in open){
                    if(tScore.HScore < lowest.HScore)
                        lowest = tScore;
                }
                
                open.Remove(lowest);

                current = lowest;
                closed.Add(lowest);
                return null;
            }
        }

        private List<Vector3> GetPath(){
            var head = closed.Where(t => t.Tile == target).First();
            List<Vector3> vectors = new List<Vector3>();

            while(head.Parent != null){
                vectors.Add(head.Tile);
                head = head.Parent;
            }

            vectors.Add(head.Tile);
            return vectors;
        }

        private List<Vector3> FindNeighbors(Vector3 start, List<Vector3> area, float maxHeightDifference) {
            List<Vector3> neighbors = new List<Vector3>();
            var xPos = FindPositiveXNeighbor(start, area);
            var xNeg = FindNegativeXNeighbor(start, area);
            var zPos = FindPositiveZNeighbor(start, area);
            var zNeg = FindNegativeZNeighbor(start, area);
            var filter = new Vector3(0.1f, 0.1f, 0.1f);

            if ((start.y - xPos.y) / 2 <= maxHeightDifference && xPos != filter)
                neighbors.Add(xPos);
            if ((start.y - xNeg.y) / 2 <= maxHeightDifference && xNeg != filter)
                neighbors.Add(xNeg);
            if ((start.y - zPos.y) / 2 <= maxHeightDifference && zPos != filter)
                neighbors.Add(zPos);
            if ((start.y - zNeg.y) / 2 <= maxHeightDifference && zNeg != filter)
                neighbors.Add(zNeg);

            return neighbors;
        }

        private Vector3 FindPositiveXNeighbor(Vector3 start, List<Vector3> area){
            foreach(var v in area){
                if(v.x == start.x + 2 && v.z == start.z){
                    return v;
                }
            }
            return new Vector3(0.1f, 0.1f, 0.1f);
        }

        private Vector3 FindNegativeXNeighbor(Vector3 start, List<Vector3> area){
            foreach(var v in area){
                if(v.x == start.x - 2 && v.z == start.z){
                    return v;
                }
            }
            return new Vector3(0.1f, 0.1f, 0.1f);
        }

        private Vector3 FindPositiveZNeighbor(Vector3 start, List<Vector3> area){
            foreach(var v in area){
                if(v.x == start.x && v.z == start.z + 2){
                    return v;
                }
            }
            return new Vector3(0.1f, 0.1f, 0.1f);
        }

        private Vector3 FindNegativeZNeighbor(Vector3 start, List<Vector3> area){
            foreach(var v in area){
                if(v.x == start.x && v.z == start.z - 2){
                    return v;
                }
            }
            return new Vector3(0.1f, 0.1f, 0.1f);
        }

        private bool IsTileInClosed(Vector3 tile){
            foreach(var tScore in closed){
                if(tScore.Tile == tile)
                    return true;
            }
            return false;
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