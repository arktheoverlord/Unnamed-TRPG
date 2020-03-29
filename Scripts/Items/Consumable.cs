using TRPG.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPG.Items {
    public class Consumable : Item {
        public bool RestoresHealth { get; set; }
        public float AmountRestored { get; set; }
        public bool RemovesStatusEffect { get; set; }
        public StatusEffect StatusEffect { get; set; }
    }
}
