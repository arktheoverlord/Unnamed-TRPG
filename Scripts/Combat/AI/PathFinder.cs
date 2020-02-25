using Scripts.Combat.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Combat.AI {
    public class PathFinder {
        private List<TileScore> openList;
        private List<TileScore> closedList;
        private TileScore current;
        private Tile target;
        private Map map;

        public void Init(Tile start, Tile target, Map map) {
            openList = new List<TileScore>();
            closedList = new List<TileScore>();
            current = new TileScore(start);
            this.target = target;
            this.map = map;
        }

        public List<Tile> FindPath() {
            if(current.Tile.Position != target.Position){
                var walkables = GetAdjacentWalkableTiles();

                foreach(TileScore tileScore in walkables){
                    tileScore.HScore = GetTileHScore(tileScore.Tile);
                    tileScore.GScore = GetTileGScore(tileScore);
                    openList.Add(tileScore);
                }

                TileScore lowest = openList[openList.Count - 1];

                for(var i = openList.Count - 1; i >= 0; i--){
                    if(openList[i].FScore < lowest.FScore){
                        lowest = openList[i];
                    }
                }

                closedList.Add(lowest);
                current = lowest;
            } else if(current.Tile.Position == target.Position){
                var tiles = new List<Tile>();

                foreach(var tile in closedList){
                    tiles.Add(tile.Tile);
                }

                return tiles;
            }

            return null;
        }

        private float GetTileHScore(Tile tile){
            float x = tile.Position.x - target.Position.x;
            float y = (float) Math.Pow(tile.Position.y - target.Position.y, 2);
            return (float) Math.Sqrt(x + y);
        }

        private float GetTileGScore(TileScore tileScore){
            return tileScore.Parent.GScore + 1;
        }

        private List<TileScore> GetAdjacentWalkableTiles(){
            var walkables = new List<TileScore>();
            List<Tile> tiles = map.GetAdjacentTiles(current.Tile);
            
            foreach(Tile tile in tiles){
                if(tile.Walkable){
                    walkables.Add(new TileScore(tile));
                }
            }

            return walkables;
        }

        private class TileScore {
            public Tile Tile { get; set; }
            public TileScore Parent { get; set; }

            public float HScore { get; set; }
            public float GScore { get; set; }
            public float FScore {
                get { 
                    return HScore + GScore;
                }
            }

            public TileScore(Tile tile){
                HScore = 0;
                GScore = 0;
                Tile = tile;
            }
        }
    }
}
