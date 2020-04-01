using Godot;
using System;
using TRPG.Characters;
using System.Collections.Generic;

namespace TRPG.Combat.States {
    public class CharacterState : Node {
        public Character BaseCharacter { get; private set; }
        public Team CharacterTeam { get; private set; }
        public int ID { get; private set; }
        public Vector3 Position { get; private set; }
        public Dictionary<Stat, float> StatModifiers { get; private set; }

        public float CurrentHealth { get; private set; } = 0;
        public float CurrentMana { get; private set; } = 0;

        public Dictionary<StatusEffect, List<int>> StatusEffects;

        public CharacterState(Character baseCharacter, Team type, int id) {
            StatusEffects = new Dictionary<StatusEffect, List<int>>();
            BaseCharacter = baseCharacter;
            CharacterTeam = type;
            ID = id;
            InitStatModifers();
            CurrentHealth = GetStatTotal(Stat.Health);
            CurrentMana = GetStatTotal(Stat.Mana);
        }

        private void InitStatModifers() {
            StatModifiers = new Dictionary<Stat, float>();
            foreach (var stat in (Stat[])Enum.GetValues(typeof(Stat))) {
                StatModifiers.Add(stat, 0);
            }
        }

        public void SetStatModifier(Stat stat, float amount){
            StatModifiers[stat] = amount;
        }

        public void AddStatModifier(Stat stat, float amount){
            StatModifiers[stat] += amount;
        }

        public float GetStatTotal(Stat stat) {
            float total = Mathf.Floor(BaseCharacter.StatTotals[stat]() + StatModifiers[stat]);
            if(stat == Stat.Move && total > 20){
                total = 20;
            }
            return total;
        }

        public void TurnStartUpdate() {

        }

        public void TurnEndUpdate(StatChanges changes) {
            UpdateMaxHealth(changes.MaxHealthModifier);
            CurrentHealth += changes.CurrentHealthModifier;
            UpdateMaxMana(changes.MaxManaModifier);
            CurrentMana += changes.CurrentManaModifier;
            Position = changes.Position;
            /*StatModifiers[S] += changes.HealthRegen;
            ManaRegen += changes.ManaRegen;

            StrengthModifier += changes.StrengthModifier;
            DexterityModifier += changes.DexterityModifier;
            IntellectModifier += changes.IntellectModifier;
            PhysicalCritChanceModifier += changes.PhysicalCritChanceModifier;
            MagicalCritChanceModifier += changes.MagicalCritChanceModifier;

            ConstitutionModifier += changes.ConstitutionModifier;
            WisdomModifier += changes.WisdomModifier;
            EvadeModifier += changes.EvadeModifier;
            BlockModifier += changes.BlockModifier;
            FireResistenceModifier += changes.FireResistenceModifier;
            LightningResistenceModifier += changes.LightningResistenceModifier;
            ColdResistenceModifier += changes.ColdResistenceModifier;
            MoveModifier += changes.MoveModifier;
            SpeedModifier += changes.SpeedModifier;
            JumpModifier += changes.JumpModifier;*/
        }

        public bool SetID(int id) {
            if (ID == 0) {
                ID = id;
                return true;
            }
            return false;
        }

        public void UpdateMaxHealth(float magnitude) {
            StatModifiers[Stat.Health] += magnitude;
            if (magnitude > 0) {
                CurrentHealth += magnitude;
            }
            if (CurrentHealth > GetStatTotal(Stat.Health)) {
                CurrentHealth = GetStatTotal(Stat.Health);
            }
        }

        #region Status Effect Methods
        public void UpdateMaxMana(float magnitude) {
            StatModifiers[Stat.Mana] += magnitude;
            if (magnitude > 0) {
                CurrentMana += magnitude;
            }
            if (CurrentMana > GetStatTotal(Stat.Mana)) {
                CurrentMana = GetStatTotal(Stat.Mana);
            }
        }

        public void UpdateStateEffect(StatusEffect effect, int turn = 1) {
            if (turn == -1) {
                StatusEffects[effect][0] = 0;
            }
            else {
                StatusEffects[effect][0] += turn;
                StatusEffects[effect][1] -= turn;
            }
        }

        public void CheckStatusEffectsForRemovel() {
            foreach (var key in StatusEffects.Keys) {
                if (StatusEffects[key][0] == 0) {
                    if (key.HasEndOfTurnEffect) {
                        foreach (var stat in (Stat[])Enum.GetValues(typeof(Stat))) {

                        }
                    }
                }
            }
        }

        private float RemoveStatusEffect(StatusEffect effect, Stat stat, float baseValue, int turnsActive) {
            if (CanEffectBeRemoved(effect, stat)) {
                float percentModiferTotal = (baseValue * effect.StatPercentModifiers[stat]) * turnsActive;
                float modifierTotal = effect.StatModifers[stat] * turnsActive;
                return percentModiferTotal + modifierTotal;
            }
            return 0;
        }

        private bool CanEffectBeRemoved(StatusEffect effect, Stat stat) {
            return effect.EOTEffects.Contains(stat) && !effect.EOTEffects.Contains(stat);
        }
        #endregion
    }

    public enum Team {
        PC, NPC, Guest
    }
}
