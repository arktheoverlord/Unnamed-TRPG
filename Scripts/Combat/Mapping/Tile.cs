using Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Scripts.Combat.Mapping {
    public class Tile {
        public Vector2 Position { get; set; }
        public int Height { get; set; }
        public int TerrainType { get; set; }
        public int TerrainObjectID { get; set; }
        public bool Walkable { get; set;}

        public bool IsSlope { get; set; }
        public int SlopeDirection { get; set; }

        public bool HasTrap { get; set; }
        public int TrapID { get; set; }

        public bool HasTreasure { get; set; }
        public List<BaseItem> TreasureChest { get; set; }
    }
}
