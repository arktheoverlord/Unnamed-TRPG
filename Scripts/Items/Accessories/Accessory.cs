using TRPG.Items;

namespace TRPG.Items.Accessories {
    public class Accessory : Equipment {
        public AccessoryAffix Prefix { get; set; } = null;
        public AccessoryAffix Sufix { get; set; } = null;
        public float HealthPercent { get; set; } = 0;
        public float ManaPercent { get; set; } = 0;
        public float StrengthPercent { get; set; } = 0;
        public float DexterityPercent { get; set; } = 0;
        public float IntellectPercent { get; set; } = 0;
        public float PhysicalCritChancePercent { get; set; } = 0;
        public float MagicalCritChancePercent { get; set; } = 0;
        public float ConstitutionPercent { get; set; } = 0;
        public float WisdomPercent { get; set; } = 0;
    }
}
