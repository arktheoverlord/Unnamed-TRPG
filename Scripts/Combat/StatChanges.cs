using Godot;
using TRPG.Characters;
using System.Collections.Generic;

namespace TRPG.Combat {
    public class StatChanges {
        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public Dictionary<Stat, int> StatModifers { get; set; }

        //Resources
        public float MaxHealthModifier { get; set; } = 0;
        public float CurrentHealthModifier { get; set; } = 0;
        public float MaxManaModifier { get; set; } = 0;
        public float CurrentManaModifier { get; set; } = 0;
        public float HealthRegen { get; set; } = 0;
        public float ManaRegen { get; set; } = 0;
        //Attack Stats
        public float StrengthModifier { get; set; } = 0;
        public float DexterityModifier { get; set; } = 0;
        public float IntellectModifier { get; set; } = 0;
        public float PhysicalCritChanceModifier { get; set; } = 0;
        public float MagicalCritChanceModifier { get; set; } = 0;
        //Defense Stats
        public float ConstitutionModifier { get; set; } = 0;
        public float WisdomModifier { get; set; } = 0;
        public float EvadeModifier { get; set; } = 0;
        public float BlockModifier { get; set; } = 0;
        public float FireResistenceModifier { get; set; } = 0;
        public float LightningResistenceModifier { get; set; } = 0;
        public float ColdResistenceModifier { get; set; } = 0;
        //MovementStats
        public float MoveModifier { get; set; } = 0;
        public float SpeedModifier { get; set; } = 0;
        public float JumpModifier { get; set; } = 0;
    }
}