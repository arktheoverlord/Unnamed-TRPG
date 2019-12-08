using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Combat.Mapping {
    public class Tile {
        public int Height { get; set; }
        public int TerrainType { get; set; }
        public bool IsSlope { get; set; }
        public int SlopeDirection { get; set; }
        public bool HasTreasure { get; set; }
        public List<BaseItem> TreasureChest { get; set; }
        public bool HasTrap { get; set; }
        public int TrapID { get; set; }
        public int TerrainObjectID { get; set; }
    }
}
