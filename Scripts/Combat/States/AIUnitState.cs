using Scripts.Actors;
using Scripts.Actors.Jobs;
using Scripts.Combat.Mapping;
using Scripts.Interfaces.Combat;
using Scripts.Items;
using System;
using System.Collections.Generic;
using Godot;

namespace Scripts.Combat.States {
    public class AIUnitState : UnitState, ICombatAI {
        private Dictionary<IUnitState, float> aggroList;

        public AIUnitState(Character character, int id, List<IUnitState> units) : base(character, id) {
            aggroList = new Dictionary<IUnitState, float>();

            foreach(var u in units) {
                aggroList.Add(u, 0);
            }
        }

        public void AddAggression(IUnitState unit, int aggroAmount) {
            aggroList[unit] += aggroAmount;

            if(aggroList[unit] > 200) {
                aggroList[unit] = 200;
                return;
            }

            float aggroTotal = 0;

            foreach(var k in aggroList.Keys) {
                aggroTotal += aggroList[k];
            }

            float extraAggro;

            if(aggroTotal > 200) {
                extraAggro = aggroTotal - 200;
            } else {
                return;
            }

            float aggroToRemove = (int) (extraAggro / aggroList.Count - 1) + 1;

            foreach(var k in aggroList.Keys) {
                if(k != unit) {
                    RemoveAggression(k, aggroToRemove);
                }
            }
        }

        public void AddAggression(IUnitState unit, float aggroPercent) {
            float aggroAmount = aggroList[unit] * aggroPercent;
            AddAggression(unit, aggroAmount);
        }

        public Dictionary<IUnitState, float> GetAggroList() {
            return aggroList;
        }

        public void RemoveAggression(IUnitState unit, int aggroAmount) {
            aggroList[unit] -= aggroAmount;

            if(aggroList[unit] < 0) {
                aggroList[unit] = 0;
            }
        }

        public void RemoveAggression(IUnitState unit, float aggroPercent) {
            float aggroAmount = aggroList[unit] * aggroPercent;
            RemoveAggression(unit, aggroAmount);
        }

        public List<IUnitState> GetAbilityTargets() {
            throw new NotImplementedException();
        }

        public JobAbility GetAbilityUsed() {
            throw new NotImplementedException();
        }

        public AIAction GetAction() {
            throw new NotImplementedException();
        }

        public List<IUnitState> GetAttackTargets() {
            throw new NotImplementedException();
        }

        public List<IUnitState> GetItemTargets() {
            throw new NotImplementedException();
        }

        public Item GetItemUsed() {
            throw new NotImplementedException();
        }

        public Vector2 GetMoveToPosition() {
            throw new NotImplementedException();
        }

        public List<MapTile> GetPath(MapTile target, Map map) {
            List<MapTile> open = new List<MapTile>();
            List<MapTile> close = new List<MapTile>();
            MapTile current = position;
            throw new NotImplementedException();
        }
    }

    public enum AIAction {

    }
}
