namespace Scripts.Items {
    public class Equipment : Item {
        public EquipmentType EquipmentType { get; set; } = EquipmentType.Empty;
        //Resources
        public float Health { get; set; } = 0;
        public float Mana { get; set; } = 0;
        public float HealthRegen { get; set; } = 0;
        public float ManaRegen { get; set; } = 0;
        //Attack Stats
        public float Strength { get; set; } = 0;
        public float Dexterity { get; set; } = 0;
        public float Intellect { get; set; } = 0;
        public float PhysicalCritChance { get; set; } = 0;
        public float MagicalCritChance { get; set; } = 0;
        //Defense Stats
        public float Constitution { get; set; } = 0;
        public float Wisdom { get; set; } = 0;
        public float Evade { get; set; } = 0;
        public float Block { get; set; } = 0;
        public float FireResistence { get; set; } = 0;
        public float LightningResistence { get; set; } = 0;
        public float ColdResistence { get; set; } = 0;
        //Movement Stats
        public float Move { get; set; } = 0;
        public float Speed { get; set; } = 0;
        public float Jump { get; set; } = 0;
        //Weapon Stuff
        public int Range { get; set; } = 1;
        public AttackRangeType AttackRangeType { get; set; } = AttackRangeType.Line;
    }

    public enum EquipmentType {
        Empty, GreatSword, LongSword, ShortSword, Dagger,
        Katana, GreatHammer, BattleHammer, ShortSpear, LongSpear,
        GreatAxe, BattleAxe, ShortBow, LongBow, GreatBow,
        Buckler, KiteShield, TowerShield, HandWraps, LightHeadArmor,
        LightBodyArmor, LightLegArmor, HeavyHeadArmor, HeavyBodyArmor, HeavyLegArmor
    }

    public enum AttackRangeType {
        Line, Sphere, Square, Disconnected
    }
}