using Assets.Scripts.Combat.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Combat.AI {
    public class PathFinding {
        private struct TileScore {
            public float G;
            public float H;
        }

        private List<Tile> openList;
        private List<Tile> closedList;
        private Tile current;
        private Tile target;
        private Map map;

        public void Init(Tile start, Tile target, Map map) {

        }

        public void Step() {

        }
    }
}
