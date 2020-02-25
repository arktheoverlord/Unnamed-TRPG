using Scripts.Actors;
using Scripts.Actors.Jobs;
using Scripts.Combat;
using Scripts.Combat.Mapping;
using Scripts.Combat.States;
using System.Collections.Generic;

namespace Scripts.Interfaces.Combat {
    public interface IUnitState {
        int GetID();

        Character GetBase();

        float GetStat(Stat stat);

        Tile GetPosition();

        void SetPosition(Tile position);

        float GetSpeedTotal();

        void IncreaseSpeedTotal();

        void ResetSpeedTotal();

        int GetFacing();

        void SetFacing(int facing);

        bool IsPC();

        bool IsGuest();

        void DamageHP(float amount);

        void RestoreHP(float amount);

        void DamageHP(float amount, IUnitState attacker);

        void RestoreHP(float amount, IUnitState healer);

        void DamageMP(float amount);

        void RestoreMP(float amount);

        void DamageMP(float amount, IUnitState attacker);

        void RestoreMP(float amount, IUnitState healer);

        void ModifieStat(Stat stat, int amount);

        void ModifieStat(Stat stat, float percent);

        List<StatusEffect> GetStatusEffects();

        bool DecreaseStatusEffectDuration(StatusEffect effect, int time = 1);

        void DecreaseAllStatusEffectDuration(int time = 1);

        void CheckStatusEffectsForRemoval();

        void AddStatusEffect(StatusEffect effect);

        void RemoveStatusEffect(StatusEffect effect);

        bool CanUseAbility(Job job, JobAbility ability);
    }
}
