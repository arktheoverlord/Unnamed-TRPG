using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Items.Equipment {
    public class Accessory {
        public AccessoryAffix Prefix { get; set; }
        public AccessoryAffix Sufix { get; set; }
        public float HP { get; set; }
        public float MP { get; set; }
        public float Strength { get; set; }
        public float Dexterity { get; set; }
        public float Intellect { get; set; }
        public float Constitution { get; set; }
        public float Wisdom { get; set; }
        public float Evade { get; set; }
        public float Block { get; set; }
        public float PhysicalCritChance { get; set; }
        public float MagicalCritChance { get; set; }
        public float Speed { get; set; }
        public float Move { get; set; }
        public float Jump { get; set; }
    }
}
