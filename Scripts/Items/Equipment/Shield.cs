using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Items.Equipment {
    public class Shield : BaseItem {
        public float Constitution { get; private set; }
        public float Wisdom { get; private set; }
        public float BlockChance { get; private set; }
        public float EvadeChance { get; private set; }
    }
}
