using Scripts.Actors;
using Scripts.Actors.Jobs;
using Scripts.Combat.Mapping;
using Scripts.Combat.States;
using Scripts.Interfaces.Combat;
using System;
using System.Collections.Generic;
using Godot;

namespace Scripts.Combat {
    public class CombatStateMachine : ICombatStateMachine {

        public List<IUnitState> Units;
        public List<float> SpeedTotals;
        public List<int> TurnOrders;

        private bool combat = false;
        private State currentState;
        private ICombatState combatState;

        public int CurrentUnitTurn;

        private StatusEffect KnockedOut;
        private RandomNumberGenerator rng;

        public void StartMachine() {
            rng = new RandomNumberGenerator();
            throw new NotImplementedException();
        }

        public void UpdateStateMachine() {
            throw new NotImplementedException();
        }

        public void EndCombat() {
            if (PlayerPartyKnockedOut()) {
                currentState = State.CombatEndedLose;
            }
            else if (NPCPartyKnockedOut()) {
                currentState = State.CombatEndedWin;
            }
            else {
                currentState = State.InCombat;
            }
        }

        //The Player's party is considered knocked out if all player controlled units are incipacitated
        private bool PlayerPartyKnockedOut() {
            int koCount = 0;
            int unitCount = 0;

            foreach(var unit in Units) {
                if(unit.IsPC() && !unit.IsGuest()) {
                    unitCount++;
                    if (unit.GetStatusEffects().Contains(KnockedOut)) {
                        koCount++;
                    }
                }
            }

            return koCount == unitCount;
        }

        private bool NPCPartyKnockedOut() {
            int koCount = 0;
            int unitCount = 0;

            foreach(var unit in Units) {
                if(!unit.IsPC() && !unit.IsGuest()) {
                    unitCount++;
                    if (unit.GetStatusEffects().Contains(KnockedOut)) {
                        koCount++;
                    }
                }
            }

            return koCount == unitCount;
        }

        public bool HasEnded() {
            if(currentState == State.CombatEndedLose || currentState == State.CombatEndedWin) {
                return true;
            } else {
                return false;
            }
        }

        public ICombatState UnitMove(IUnitState unit, Tile position) {
            combatState.UnitMove(unit, position);
            return combatState;
        }

        public ICombatState UnitUseAbility(IUnitState unit, Job job, JobAbility ability, List<int> targetIndexes) {
            throw new NotImplementedException();
        }

        public ICombatState UnitAttack(IUnitState unit, List<int> targetIndexes) {
            throw new NotImplementedException();
        }

        public ICombatState UnitUseItem(IUnitState unit, int itemIndex, List<int> targetIndexes) {
            throw new NotImplementedException();
        }

        public ICombatState UnitDamaged(IUnitState unit, int damage) {
            throw new NotImplementedException();
        }

        public ICombatState UnitHealed(IUnitState unit, int healed) {
            throw new NotImplementedException();
        }

        public ICombatState UnitKnockedOut(IUnitState unit) {
            throw new NotImplementedException();
        }

        public List<Vector2> GetMoveArea(IUnitState unit) {
            throw new NotImplementedException();
        }

        public List<Vector2> GetTargetArea(int unitIndex, int abilityIndex) {
            throw new NotImplementedException();
        }

        private enum State {
            CombatStarted, InCombat, CombatEndedLose, CombatEndedWin
        }
    }
}
