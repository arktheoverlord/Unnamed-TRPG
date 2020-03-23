using Godot;
using System;
using Scripts.Characters;
using System.Collections.Generic;

namespace Scripts.Combat.States {
    public class CharacterState {
        public Character BaseCharacter { get; private set; }
        public Team CharacterTeam { get; private set; }
        public int ID { get; set; }
        public Vector3 Position { get; set; }

        //Resources
        public float MaxHealth { get; set; } = 0;
        public float CurrentHealth { get; set; } = 0;
        public float MaxMana { get; set; } = 0;
        public float CurrentMana { get; set; } = 0;
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
        public float IceResistenceModifier { get; set; } = 0;
        //MovementStats
        public float MoveModifier { get; set; } = 0;
        public float SpeedModifier { get; set; } = 0;
        public float JumpModifier { get; set; } = 0;

        public Dictionary<StatusEffect, int> StatusEffects;

        public CharacterState(Character baseCharacter, Team type, int id) {
            StatusEffects = new Dictionary<StatusEffect, int>();
            BaseCharacter = baseCharacter;
            CharacterTeam = type;
            ID = id;
            MaxHealth = BaseCharacter.Health;
            CurrentHealth = BaseCharacter.Health;
            MaxMana = BaseCharacter.Mana;
            CurrentMana = BaseCharacter.Mana;
        }
    }

    public enum Team {
        PC, NPC, Guest
    }
}
