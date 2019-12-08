using Assets.Scripts.Actors.Jobs;
using Assets.Scripts.Combat.States;
using Assets.Scripts.Items;
using System.Collections.Generic;
using Godot;

namespace Assets.Scripts.Interfaces.Combat {
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
