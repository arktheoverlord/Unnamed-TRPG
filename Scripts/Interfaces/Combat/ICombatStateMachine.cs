using Assets.Scripts.Actors.Jobs;
using Assets.Scripts.Combat;
using Assets.Scripts.Combat.Mapping;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces.Combat {
    interface ICombatStateMachine : IStateMachine {
        void EndCombat();

        ICombatState UnitMove(IUnitState unit, Tile position);

        ICombatState UnitUseAbility(IUnitState unit, Job job, JobAbility ability, List<int> targetIndexes);

        ICombatState UnitAttack(IUnitState unit, List<int> targetIndexes);

        ICombatState UnitUseItem(IUnitState unit, int itemIndex, List<int> targetIndexes);

        ICombatState UnitDamaged(IUnitState unit, int damage);

        ICombatState UnitHealed(IUnitState unit, int healed);

        ICombatState UnitKnockedOut(IUnitState unit);
    }
}
