using Scripts.Actors.Jobs;
using Scripts.Combat.States;
using Scripts.Items;
using System.Collections.Generic;
using Godot;

namespace Scripts.Interfaces.Combat {
    public interface ICombatAI {
        void AddAggression(IUnitState unit, int aggroAmount);

        void AddAggression(IUnitState unit, float aggroPercent);

        void RemoveAggression(IUnitState unit, int aggroAmount);

        void RemoveAggression(IUnitState unit, float aggroPercent);

        Dictionary<IUnitState, float> GetAggroList();

        AIAction GetAction();

        Vector2 GetMoveToPosition();

        List<IUnitState> GetAttackTargets();

        JobAbility GetAbilityUsed();

        List<IUnitState> GetAbilityTargets();

        BaseItem GetItemUsed();

        List<IUnitState> GetItemTargets();
    }
}
