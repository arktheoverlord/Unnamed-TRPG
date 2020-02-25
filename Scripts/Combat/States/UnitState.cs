using Scripts.Actors;
using Scripts.Actors.Jobs;
using Scripts.Combat.Mapping;
using Scripts.Interfaces.Combat;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Combat.States {
    public class UnitState : IUnitState {
        protected int ID;
        protected Character Base;

        protected Dictionary<Stat, float> stats;
        protected Dictionary<StatusEffect, int> statuses;
        protected Tile position;
        protected float speedTotal;
        protected int facing;
        protected bool isPC;
        protected bool isGuest;

        public UnitState(Character character, int id) {
            ID = id;
            Base = character;

            stats = new Dictionary<Stat, float> {
                { Stat.MaxHP, Base.HP},
                { Stat.CurrentHP, Base.HP },
                { Stat.MaxMP, Base.MP },
                { Stat.CurrentMP, Base.MP },
                { Stat.Strength, Base.Strength },
                { Stat.Dexterity, Base.Dexterity },
                { Stat.Intellect, Base.Intellect },
                { Stat.Constitution, Base.Constitution },
                { Stat.Wisdom, Base.Wisdom },
                { Stat.HPRegen, Base.HPRegen },
                { Stat.MPRegen, Base.MPRegen },
                { Stat.PhysicalCritChance, Base.PhysicalCritChance },
                { Stat.MagicalCritChance, Base.MagicalCritChance },
                { Stat.Block, Base.Block },
                { Stat.Evade, Base.Evade },
                { Stat.Speed, Base.Speed },
                { Stat.Move, Base.Move },
                { Stat.Jump, Base.Jump },
            };

            statuses = new Dictionary<StatusEffect, int>();
        }

        public int GetID() {
            return ID;
        }

        public Character GetBase() {
            return Base;
        }

        public float GetStat(Stat stat) {
            return stats[stat];
        }

        public Tile GetPosition() {
            return position;
        }

        public void SetPosition(Tile position) {
            this.position = position;
        }

        public float GetSpeedTotal() {
            return speedTotal;
        }

        public void IncreaseSpeedTotal() {
            speedTotal += stats[Stat.Speed];
        }

        public void ResetSpeedTotal() {
            speedTotal = 0;
        }

        public int GetFacing() {
            return facing;
        }

        public void SetFacing(int facing) {
            this.facing = facing;
        }

        public bool IsPC() {
            return isPC;
        }

        public bool IsGuest() {
            return isGuest;
        }

        public void DamageHP(float amount) {
            stats[Stat.CurrentHP] -= amount;
            if (stats[Stat.CurrentHP] < 0) {
                stats[Stat.CurrentHP] = 0;
            }
        }

        public void RestoreHP(float amount) {
            stats[Stat.CurrentHP] += amount;
            if (stats[Stat.CurrentHP] > stats[Stat.MaxHP]) {
                stats[Stat.CurrentHP] = stats[Stat.MaxHP];
            }
        }

        public void DamageHP(float amount, IUnitState attacker) {
            DamageHP(amount);
        }

        public void RestoreHP(float amount, IUnitState healer) {
            RestoreHP(amount);
        }

        public void DamageMP(float amount) {
            stats[Stat.CurrentMP] -= amount;
            if (stats[Stat.CurrentMP] < 0) {
                stats[Stat.CurrentMP] = 0;
            }
        }

        public void RestoreMP(float amount) {
            stats[Stat.CurrentMP] += amount;
            if (stats[Stat.CurrentMP] > stats[Stat.MaxMP]) {
                stats[Stat.CurrentMP] = stats[Stat.MaxMP];
            }
        }

        public void DamageMP(float amount, IUnitState attacker) {
            DamageMP(amount);
        }

        public void RestoreMP(float amount, IUnitState healer) {
            RestoreMP(amount);
        }

        public void ModifieStat(Stat stat, int amount) {
            stats[stat] += amount;
        }

        public void ModifieStat(Stat stat, float percent) {
            stats[stat] *= percent;
        }

        public List<StatusEffect> GetStatusEffects() {
            return statuses.Keys.ToList();
        }

        public bool DecreaseStatusEffectDuration(StatusEffect effect, int time = 1) {
            if (statuses.ContainsKey(effect)) {
                statuses[effect] -= time;
                return true;
            }
            return false;
        }

        public void DecreaseAllStatusEffectDuration(int time = 1) {
            foreach(var s in statuses.Keys) {
                DecreaseStatusEffectDuration(s, time);
            }
        }

        public void CheckStatusEffectsForRemoval() {
            List<StatusEffect> needsRemovel = new List<StatusEffect>();

            foreach(var s in statuses.Keys) {
                if(statuses[s] == 0) {
                    needsRemovel.Add(s);
                }
            }

            for(int i = 0; i < needsRemovel.Count; i++) {
                statuses.Remove(needsRemovel[i]);
            }
        }

        public void AddStatusEffect(StatusEffect effect) {
            if (statuses.Keys.ToList().Contains(effect)) {
                statuses[effect] = effect.Duration;
            } else {
                statuses.Add(effect, effect.Duration);
            }
        }

        public void RemoveStatusEffect(StatusEffect effect) {
            if (statuses.Keys.Contains(effect)) {
                statuses.Remove(effect);
            }
        }

        public bool CanUseAbility(Job job, JobAbility ability) {
            var abilities = Base.JobMeta[job].UnlockedAbilities;
            int index = abilities.FindIndex(a => a == ability);
            float mpCost = abilities[index].MPCostAmount;

            if(mpCost > stats[Stat.CurrentMP]) {
                return false;
            }

            return true;
        }
    }
}
