using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Combat.Mapping {
    public class Map {
        public List<List<Tile>> Tiles { get; private set; }

        public Map(int height, int width) {
            Tiles = new List<List<Tile>>();
            for(int x = 0; x < height; x++) {
                List<Tile> temp = new List<Tile>();
                for(int y = 0; y < width; y++) {
                    temp.Add(new Tile() {
                        Height = 1,
                        TerrainType = 1
                    });
                }
                Tiles.Add(temp);
            }
        }

        public Tile GetTile(int x, int y){
            return Tiles[y][x];
        }

        public List<Tile> GetAdjacentTiles(Tile tile){
            var tiles = new List<Tile>();
            tiles.Add(GetTile((int) tile.Position.x + 1, (int) tile.Position.y));
            tiles.Add(GetTile((int) tile.Position.x - 1, (int) tile.Position.y));
            tiles.Add(GetTile((int) tile.Position.x, (int) tile.Position.y + 1));
            tiles.Add(GetTile((int) tile.Position.x, (int) tile.Position.y - 1));
            return tiles;
        }
    }
}
