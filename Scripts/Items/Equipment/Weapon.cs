using Scripts.Actors.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Items.Equipment {
    public class Weapon : Item {
        public float MeleePower { get; set; }
        public float RangedPower { get; set; }
        public float MagicalPower { get; set; }
        public float Strength { get; set; }
        public float Dexterity { get; set; }
        public float Intellect { get; set; }
        public float PhysicalCritCahnce { get; set; }
        public float MagicalCritChance { get; set; }
        public DamageType DamageType { get; set; }
        public bool IsHeavy { get; set; }
    }
}
