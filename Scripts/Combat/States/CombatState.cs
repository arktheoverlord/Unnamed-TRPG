using Assets.Scripts.Actors;
using Assets.Scripts.Actors.Jobs;
using Assets.Scripts.Combat.Helper;
using Assets.Scripts.Combat.Mapping;
using Assets.Scripts.Interfaces.Combat;
using Assets.Scripts.Items.Equipment;
using System;
using System.Collections.Generic;
using Godot;

namespace Assets.Scripts.Combat.States {
    public class CombatState : ICombatState {
        public Map Map { get; private set; }
        public List<IUnitState> Units { get; private set; }
        public List<IUnitState> TurnOrder { get; private set; }

        public CombatState(List<Character> pcs, List<Character> npcs) {
            Units = new List<IUnitState>();
            TurnOrder = new List<IUnitState>();
            AddUnits(pcs, npcs);
        }

        /// <summary>
        /// Add all units to the state machine.
        /// </summary>
        /// <param name="pcs">The player's party excluding guest units.</param>
        /// <param name="pcGuests">The player's guests.</param>
        /// <param name="npcs">The enemy party.</param>
        public void AddUnits(List<Character> pcs, List<Character> npcs, List<Character> pcGuests = null) {
            int id = 0;
            if (pcs.Count > 6 || pcs.Count < 1) {
                throw new Exception(string.Format("PC count not within range 1 - 6\n\tCount: {0}", pcs.Count));
            }

            if (npcs.Count > 8 || npcs.Count < 1) {
                throw new Exception(string.Format("NPC count not within range 1 - 8\n\tCount: {0}", npcs.Count));
            }

            foreach (var pc in pcs) {
                Units.Add(new UnitState(pc, id));
                id++;
            }

            List<IUnitState> pcStates = new List<IUnitState>();

            foreach (var unit in Units) {
                if (unit.IsPC()) {
                    pcStates.Add(unit);
                }
            }

            foreach (var npc in npcs) {
                Units.Add(new AIUnitState(npc, id, pcStates));
                id++;
            }

            if (pcGuests != null) {
                List <IUnitState> npcStates = new List<IUnitState>();

                foreach (var unit in Units) {
                    if (!unit.IsPC() && !unit.IsGuest()) {
                        npcStates.Add(unit);
                    }
                }

                foreach (var guest in pcGuests) {
                    Units.Add(new AIUnitState(guest, id, npcStates));
                    id++;
                }
            }
        }

        public void Update(float deltaTime) {
            if (TurnOrder.Count < 20) {
                for (int i = 0; i < Units.Count; i++) {
                    Units[i].IncreaseSpeedTotal();
                    if (Units[i].GetSpeedTotal() >= 120) {
                        Units[i].ResetSpeedTotal();
                        TurnOrder.Add(Units[i]);
                    }
                }
            }
        }

        public IUnitState GetNextUnitTurn() {
            var temp = TurnOrder[0];
            TurnOrder.RemoveAt(0);
            return temp;
        }

        public List<Vector2> GetUnitMovementArea(IUnitState unit) {
            List<Vector2> moveArea = new List<Vector2>();

            return moveArea;
        }

        public void UnitMove(IUnitState unit, Tile position) {
            unit.SetPosition(position);
        }

        public List<Vector2> GetAbilityTargetArea(IUnitState unit, JobAbility ability) {
            List<Vector2> targetArea = new List<Vector2>();

            return targetArea;
        }

        public List<Vector2> GetAttackTargetArea(IUnitState unit) {
            List<Vector2> attackArea = new List<Vector2>();

            return attackArea;
        }

        //TODO Refactor to include special abilites
        public Dictionary<IUnitState, UnitStateChanges> UnitUseAbility(IUnitState user, JobAbility ability, List<IUnitState> targets, RandomNumberGenerator rng) {
            var stateChanges = new Dictionary<IUnitState, UnitStateChanges>(){
                { user, new UnitStateChanges() }
            };

            if (targets != null && targets.Count > 0) {
                for (int i = 0; i < targets.Count; i++) {
                    stateChanges.Add(targets[i], new UnitStateChanges());
                } 
            }

            if (CombatHelper.DoesAbilityMiss(user, ability, rng)) {
                stateChanges[user].DidDodgeOrMiss = true;
            }

            if (targets != null && targets.Count > 0) {
                foreach (var target in targets) {
                    var targetChanges = stateChanges[target];
                    var userChanges = stateChanges[user];

                    AbilityEffects.AbilityTargetedEffects(user, target, ability, targetChanges, userChanges, rng);
                } 
            }

            AbilityEffects.AbilitySelfEffects(user, ability, stateChanges, rng);

            if (ability.ModifiesAggression) {
                //TODO Add aggression modification logic
            }

            if (ability.HasExtraEffect) {
                //TODO Add extra effect logic
            }

            return stateChanges;
        }



        public List<float> UnitAttack(IUnitState unit, List<IUnitState> targets, Weapon weapon) {
            //TODO Rewrite this to use UnitStateChanges
            //List<float> damage = new List<float>();
            //var attacker = GetUnitMetaData(unit);

            //for (int i = 0; i < targets.Capacity; i++) {
            //    int attackerAttackVector2 = CombatHelper.GetAttackerAttackVector2(unit, attacker.Vector2, targets[i], GetUnitMetaData();
            //    var target = GetUnitMetaData(targets[i]);
            //    float facingMissChance = GetFacingHitChance(attackerAttackVector2, target.Facing);

            //    if (DoesAttackHit(unit, facingMissChance)) {
            //        if (DoesCharacterEvade(targets[i])) {
            //            damage.Add(-2);
            //        }
            //        else {
            //            float attackStat = GetAttackStat(attacker.UnitState, weapon.DamageType);
            //            float defenseStat = GetDefenseStat(target.UnitState, weapon.DamageType);
            //            float critChance = GetCriticalStat(attacker.UnitState, weapon.DamageType);
            //            float weaponPower = GetWeaponPower(weapon);
            //            damage.Add(CalculateDamage(attackStat, weaponPower, defenseStat, critChance, 1));
            //        }
            //    }
            //    else {
            //        damage.Add(-1);
            //    }
            //}

            //return damage;
            throw new NotImplementedException();
        }

        public void UnitUseItem(IUnitState unit, int itemIndex, List<IUnitState> targets) {
            throw new NotImplementedException();
        }

        public void UnitDamaged(IUnitState unit, float damage, IUnitState damagedBy, bool isHPDamage) {
            ////TODO Rewrite to use UnitStateChanges
            //var defender = GetUnitMetaData(unit);
            //var attacker = GetUnitMetaData(damagedBy);

            //if (isHPDamage) {
            //    if (attacker.IsPC || attacker.IsGuest) {
            //        defender.UnitState.DamageHP(damage, attacker.UnitState);
            //    }
            //    else {
            //        defender.UnitState.DamageHP(damage);
            //    }
            //}
            //else {
            //    if (attacker.IsPC || attacker.IsGuest) {
            //        defender.UnitState.DamageMP(damage, attacker.UnitState);
            //    }
            //    else {
            //        defender.UnitState.DamageMP(damage);
            //    }
            //}
        }

        public void UnitHealed(IUnitState unit, float healed, IUnitState healedBy, bool isHPHeal) {
            ////TODO Rewrite to use UnitStateChanges
            //var defender = GetUnitMetaData(unit);
            //var healer = GetUnitMetaData(healedBy);

            //if (isHPHeal) {
            //    if (healer.IsPC || healer.IsGuest) {
            //        defender.UnitState.RestoreHP(healed, healer.UnitState);
            //    }
            //    else {
            //        defender.UnitState.RestoreHP(healed);
            //    }
            //}
            //else {
            //    if (healer.IsPC || healer.IsGuest) {
            //        defender.UnitState.RestoreMP(healed, healer.UnitState);
            //    }
            //    else {
            //        defender.UnitState.RestoreMP(healed);
            //    }
            //}
        }

        public void UnitKnockedOut(IUnitState unit) {
            throw new NotImplementedException();
        }

        public void UpdateUnitState(IUnitState unit, UnitStateChanges changes) {
            unit.RestoreHP(changes.HPHeal);
            unit.RestoreMP(changes.MPHeal);
            unit.DamageHP(changes.HPDamage);
            unit.DamageMP(changes.MPDamage);

            foreach (var status in changes.StatusesAdded) {
                unit.AddStatusEffect(status);
            }

            foreach (var status in changes.StatusesRemoved) {
                unit.RemoveStatusEffect(status);
            }

            foreach (var stat in changes.StatModifiers.Keys) {
                unit.ModifieStat(stat, changes.StatModifiers[stat]);
            }
        }
    }

    public enum Stat {
        None, MaxHP, CurrentHP, MaxMP, CurrentMP, Strength, Dexterity, Intellect, Constitution, Wisdom, HPRegen, MPRegen, PhysicalCritChance, MagicalCritChance, Block, Evade, Speed, Move, Jump
    }
}