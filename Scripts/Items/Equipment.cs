namespace TRPG.Items {
    ///<summary>The base class for all equipment</summary>
    public class Equipment : Item {
        public EquipmentType EquipmentType { get; set; } = EquipmentType.Empty;
        //Resources
        ///<summary>Additional health granted by this piece of equipment.</summary>
        public float Health { get; set; } = 0;
        ///<summary>Additional mana granted by this piece of equipment.</summary>
        public float Mana { get; set; } = 0;
        ///<summary>Additional health regen granted by this piece of equipment.</summary>
        public float HealthRegen { get; set; } = 0;
        ///<summary>Additional mana regen granted by this piece of equipment.</summary>
        public float ManaRegen { get; set; } = 0;
        //Attack Stats
        ///<summary>Additional strength granted by this piece of equipment.</summary>
        public float Strength { get; set; } = 0;
        ///<summary>Additional dexterity granted by this piece of equipment.</summary>
        public float Dexterity { get; set; } = 0;
        ///<summary>Additional intellect granted by this piece of equipment.</summary>
        public float Intellect { get; set; } = 0;
        ///<summary>Additional physical critical strike chance granted by this piece of equipment.</summary>
        public float PhysicalCritChance { get; set; } = 0;
        ///<summary>Additional magical critical strike chance granted by this piece of equipment.</summary>
        public float MagicalCritChance { get; set; } = 0;
        //Defense Stats
        ///<summary>Additional constitution granted by this piece of equipment.</summary>
        public float Constitution { get; set; } = 0;
        ///<summary>Additional wisdom granted by this piece of equipment.</summary>
        public float Wisdom { get; set; } = 0;
        ///<summary>Additional evade granted by this piece of equipment.</summary>
        public float Evade { get; set; } = 0;
        ///<summary>Additional block granted by this piece of equipment.</summary>
        public float Block { get; set; } = 0;
        ///<summary>Additional fire resistence granted by this piece of equipment.</summary>
        public float FireResistence { get; set; } = 0;
        ///<summary>Additional lightning resistence granted by this piece of equipment.</summary>
        public float LightningResistence { get; set; } = 0;
        ///<summary>Additional cold resistence granted by this piece of equipment.</summary>
        public float ColdResistence { get; set; } = 0;
        //Movement Stats
        ///<summary>Additional movement granted by this piece of equipment.</summary>
        public float Move { get; set; } = 0;
        ///<summary>Additional speed granted by this piece of equipment.</summary>
        public float Speed { get; set; } = 0;
        ///<summary>Additional jump granted by this piece of equipment.</summary>
        public float Jump { get; set; } = 0;
        //Weapon Stuff
        ///<summary>This weapon's range.</summary>
        public int Range { get; set; } = 1;
        ///<summary>This weapons targeting area.</summary>
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