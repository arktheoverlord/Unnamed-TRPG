using Scripts.Actors.Jobs;
using Scripts.Combat.Mapping;
using Scripts.Combat.States;
using Scripts.Items.Equipment;
using System.Collections.Generic;
using Godot;

namespace Scripts.Interfaces.Combat {
    public interface ICombatState {
        void Update(float deltaTime);

        void UnitMove(IUnitState unit, Tile position);

        Dictionary<IUnitState, UnitStateChanges> UnitUseAbility(IUnitState unit, JobAbility ability, List<IUnitState> targets, RandomNumberGenerator rng);

        List<float> UnitAttack(IUnitState unit, List<IUnitState> targets, Weapon weapon);

        void UnitUseItem(IUnitState unit, int itemIndex, List<IUnitState> targets);

        void UnitDamaged(IUnitState unit, float damage, IUnitState attacker, bool isHPDamage);

        void UnitHealed(IUnitState unit, float healed, IUnitState healer, bool isHPHeal);

        void UnitKnockedOut(IUnitState unit);

        IUnitState GetNextUnitTurn();

        void UpdateUnitState(IUnitState unit, UnitStateChanges changes);

        List<Vector2> GetUnitMovementArea(IUnitState unit);

        List<Vector2> GetAbilityTargetArea(IUnitState unit, JobAbility ability);

        List<Vector2> GetAttackTargetArea(IUnitState unit);
    }
}
