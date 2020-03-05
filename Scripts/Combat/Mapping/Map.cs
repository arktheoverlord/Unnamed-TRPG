using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Combat.Mapping {
    public class Map {
        public List<List<MapTile>> Tiles { get; private set; }

        public Map(int height, int width) {
            Tiles = new List<List<MapTile>>();
            for(int x = 0; x < height; x++) {
                List<MapTile> temp = new List<MapTile>();
                for(int y = 0; y < width; y++) {
                    temp.Add(new MapTile() {
                        Height = 1,
                        TerrainType = 1
                    });
                }
                Tiles.Add(temp);
            }
        }

        public MapTile GetTile(int x, int y){
            return Tiles[y][x];
        }

        public List<MapTile> GetAdjacentTiles(MapTile tile){
            var tiles = new List<MapTile>();
            tiles.Add(GetTile((int) tile.Position.x + 1, (int) tile.Position.y));
            tiles.Add(GetTile((int) tile.Position.x - 1, (int) tile.Position.y));
            tiles.Add(GetTile((int) tile.Position.x, (int) tile.Position.y + 1));
            tiles.Add(GetTile((int) tile.Position.x, (int) tile.Position.y - 1));
            return tiles;
        }
    }
}
