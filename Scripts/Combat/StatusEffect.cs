using Scripts.Combat.States;

namespace Scripts.Combat {
    public class StatusEffect {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public bool DealsDamage { get; set; }
        public bool Heals { get; set; }
        public bool DealsMPDamage { get; set; }
        public bool HealsMP { get; set; }
        public Stat StatModified { get; set; }
        public bool Buffs { get; set; }
        public int StatModifier { get; set; }
    }
}
