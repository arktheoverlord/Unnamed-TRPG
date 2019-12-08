using Assets.Scripts.Actors.Jobs;
using Assets.Scripts.Combat.Helper;
using Assets.Scripts.Interfaces.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Assets.Scripts.Combat.States {
    public class AbilityEffects {
        public static void AbilityTargetedEffects(IUnitState user, IUnitState target, JobAbility ability, UnitStateChanges targetChanges, UnitStateChanges userChanges, RandomNumberGenerator rng) {
            if ((ability.Heals || ability.HealsMP) && !userChanges.DidDodgeOrMiss) {
                AbilityHeal(user, ability, target, targetChanges, rng);
            }

            if (ability.Damages && !userChanges.DidDodgeOrMiss) {
                AbilityDamageHP(user, ability, target, targetChanges, rng);
            }

            if(ability.DamagesMP && !userChanges.DidDodgeOrMiss) {
                AbilityDamageMP(ability, target, targetChanges, rng);
            }

            if (ability.AddsStatusEffects && !userChanges.DidDodgeOrMiss) {
                AddStatusEffects(ability, target, targetChanges, rng);
            }

            if (ability.RemovesStatusEffects && !userChanges.DidDodgeOrMiss) {
                RemoveStatusEffects(ability, target, targetChanges, rng);
            }

            if (ability.ModifiesStats && !userChanges.DidDodgeOrMiss) {
                AbilityStatModification(ability, target, targetChanges, rng);
            }
        }

        public static void AbilitySelfEffects(IUnitState unit, JobAbility ability, Dictionary<IUnitState, UnitStateChanges> stateChanges, RandomNumberGenerator rng) {
            if (ability.HealsSelf) {
                AbilitySelfHealHP(unit, ability, stateChanges[unit], rng);
            }

            if (ability.HealsSelfMP) {
                AbilitySelfHealMP(unit, ability, stateChanges[unit]);
            }

            if (ability.DamagesSelf && !stateChanges[unit].DidDodgeOrMiss) {
                AbilitySelfDamageHP(unit, ability, stateChanges[unit], rng);
            }

            if (ability.HasRecoil && !stateChanges[unit].DidDodgeOrMiss) {
                AbilityRecoil(unit, ability, stateChanges);
            }

            if (ability.AddsSelfStatusEffects) {
                foreach (var status in ability.StatusEffectsAdded) {
                    stateChanges[unit].StatusesAdded.Add(status);
                }
            }

            if (ability.RemovesSelfStatusEffects) {
                foreach (var status in ability.StatusEffectsAdded) {
                    stateChanges[unit].StatusesRemoved.Add(status);
                }
            }

            if (ability.ModifiesSelfStats) {
                AbilitySelfStatModification(unit, ability, stateChanges[unit], rng);
            }
        }

        #region Ability Effects
        private static void AbilityHeal(IUnitState attacker, JobAbility ability, IUnitState target, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (!target.IsPC() && attacker.IsPC() && CombatHelper.DoesCharacterEvade(target, rng)) {
                stateChanges.DidDodgeOrMiss = true;
            }
            else {
                if (ability.Heals) {
                    AbilityHealHP(attacker, ability, stateChanges, target, rng);
                }

                if (ability.HealsMP) {
                    AbilityHealMP(ability, stateChanges, target);
                }
            }
        }

        private static void AbilityHealHP(IUnitState unit, JobAbility ability, UnitStateChanges stateChanges, IUnitState target, RandomNumberGenerator rng) {
            if (ability.HealPower > 0) {
                float healStat = CombatHelper.GetAttackStat(unit, ability.DamageType);
                float critChance = CombatHelper.GetCriticalStat(unit, ability.DamageType);
                stateChanges.HPHeal += CombatHelper.CalculateHeal(healStat, ability.HealPower, critChance, 2f, rng);
            }
            else if (ability.HealPercent > 0) {
                float heal = target.GetStat(Stat.MaxHP) * ability.HealPercent;

                if (heal < 1) {
                    heal = 1;
                }

                stateChanges.HPHeal += heal;
            }
            else if (ability.HealAmount > 0) {
                stateChanges.HPHeal += ability.HealAmount;
            }
            else {
                throw new Exception(string.Format("Ability {0} has Heals flag set to true but HealPower, HealPercent, and HealAmount are 0 or less or empty", ability.Name));
            }
        }

        private static void AbilityHealMP(JobAbility ability, UnitStateChanges stateChanges, IUnitState target) {
            if (ability.MPHealPercent > 0) {
                float heal = target.GetStat(Stat.MaxMP) * ability.MPHealPercent;

                if (heal < 1) {
                    heal = 1;
                }

                stateChanges.MPHeal += heal;
            }
            else if (ability.MPHealAmount > 0) {
                stateChanges.MPHeal += ability.MPHealAmount;
            }
            else {
                throw new Exception(string.Format("Ability {0} has Heals flag set to true but MPHealPercent and MPHealAmount are 0 or less or empty", ability.Name));
            }
        }

        private static void AbilitySelfHealHP(IUnitState unit, JobAbility ability, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (ability.SelfHealPower > 0) {
                float healStat = CombatHelper.GetAttackStat(unit, ability.DamageType);
                float critChance = CombatHelper.GetCriticalStat(unit, ability.DamageType);
                stateChanges.HPHeal += CombatHelper.CalculateHeal(healStat, ability.SelfHealPower, critChance, 2f, rng);
            }
            else if (ability.SelfHealPercent > 0) {
                float heal = unit.GetStat(Stat.MaxHP) * ability.SelfHealPercent;

                if (heal < 1) {
                    heal = 1;
                }

                stateChanges.HPHeal += heal;
            }
            else if (ability.SelfHealAmount > 0) {
                stateChanges.HPHeal += ability.SelfHealAmount;
            }
            else {
                throw new Exception(string.Format("Ability {0} has HealsSelf flag set to true but SelfHealPower, SelfHealPercent, and SelfHealAmount are 0 or less or empty", ability.Name));
            }
        }

        private static void AbilitySelfHealMP(IUnitState unit, JobAbility ability, UnitStateChanges stateChanges) {
            if (ability.SelfMPHealPercent > 0) {
                float heal = unit.GetStat(Stat.MaxMP) * ability.SelfMPHealPercent;

                if (heal < 1) {
                    heal += 1;
                }

                stateChanges.MPHeal += heal;
            }
            else if (ability.SelfMPHealAmount > 0) {
                stateChanges.MPHeal += ability.SelfMPHealAmount;
            }
            else {
                throw new Exception(string.Format("Ability {0} has HealsSelfMP flag set to true but SelfMPHealPercent, and SelfMPHealAmount are 0 or less or empty", ability.Name));
            }
        }

        private static void AbilityDamageHP(IUnitState unit, JobAbility ability, IUnitState target, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (CombatHelper.DoesCharacterEvade(target, rng)) {
                stateChanges.DidDodgeOrMiss = true;
            }
            else {
                if (ability.DamagePower > 0) {
                    float attackStat = CombatHelper.GetAttackStat(unit, ability.DamageType);
                    float defenseStat = CombatHelper.GetDefenseStat(target, ability.DamageType);
                    float critChance = CombatHelper.GetCriticalStat(unit, ability.DamageType);
                    stateChanges.HPDamage = CombatHelper.CalculateDamage(attackStat, ability.DamagePower, defenseStat, critChance, 1, rng);
                }
                else if (ability.DamagePercent > 0) {
                    float damage = target.GetStat(Stat.CurrentHP) * ability.DamagePercent;

                    if (damage < 1) {
                        damage = 1;
                    }

                    stateChanges.HPDamage += damage;
                }
                else if (ability.DamageAmount > 0) {
                    stateChanges.HPDamage += ability.DamageAmount;
                }
                else {
                    throw new Exception(string.Format("Ability {0} has Damages flag set to true but DamagesPower, DamagesPercent, and DamageAmount are 0 or less or empty", ability.Name));
                }
            }
        }

        private static void AbilityDamageMP(JobAbility ability, IUnitState target, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (CombatHelper.DoesCharacterEvade(target, rng)) {
                stateChanges.DidDodgeOrMiss = true;
            }
            else {
                if(ability.MPDamagePercent > 0) {
                    float damage = target.GetStat(Stat.CurrentMP) * ability.MPDamagePercent;

                    if (damage < 1) {
                        damage = 1;
                    }

                    stateChanges.MPDamage += damage;
                }
                else if(ability.MPDamageAmount > 0) {
                    stateChanges.MPDamage += ability.MPDamageAmount;
                } else {
                    throw new Exception(string.Format("Ability {0] has MP Damage flag set to true but MPDamagePercent and MPDamageAmount are equl to or less than 0 or null", ability.Name));
                }
            }
        }

        private static void AbilitySelfDamageHP(IUnitState unit, JobAbility ability, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (ability.SelfDamagePower > 0) {
                float attackStat = CombatHelper.GetAttackStat(unit, ability.DamageType);
                float defenseStat = CombatHelper.GetDefenseStat(unit, ability.DamageType);
                float critChance = CombatHelper.GetCriticalStat(unit, ability.DamageType);
                stateChanges.HPDamage += CombatHelper.CalculateDamage(attackStat, ability.SelfDamagePower, defenseStat, critChance, 1, rng);
            }
            else if (ability.SelfDamagePercent > 0) {
                float damage = unit.GetStat(Stat.CurrentHP) * ability.SelfDamagePercent;

                if (damage < 1) {
                    damage = 1;
                }

                stateChanges.HPDamage += damage;
            }
            else if (ability.SelfDamageAmount > 0) {
                stateChanges.HPDamage += ability.SelfDamageAmount;
            }
            else {
                throw new Exception(string.Format("Ability {0} has SelfDamage flag set to true but SelfDamagePower, SelfDamagePercent, and SelfDamageAmount are 0 or less or empty", ability.Name));
            }
        }

        private static void AbilityRecoil(IUnitState unit, JobAbility ability, Dictionary<IUnitState, UnitStateChanges> stateChanges) {
            if (ability.RecoilPercent <= 0) {
                throw new Exception(string.Format("Ability {0} has Recoil flag set to true but RecoilPercent is 0 or less or empty", ability.Name));
            }

            float totalDamage = 0;

            foreach (var k in stateChanges.Keys) {
                if (!stateChanges[k].DidDodgeOrMiss) {
                    totalDamage += stateChanges[k].HPDamage;
                }
            }

            stateChanges[unit].HPDamage += totalDamage * ability.RecoilPercent;
        }

        private static void AddStatusEffects(JobAbility ability, IUnitState target, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (CombatHelper.DoesCharacterEvade(target, rng)) {
                stateChanges.DidDodgeOrMiss = true;
            }
            else {
                foreach (var status in ability.StatusEffectsAdded) {
                    stateChanges.StatusesAdded.Add(status);
                }
            }
        }

        private static void RemoveStatusEffects(JobAbility ability, IUnitState target, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (CombatHelper.DoesCharacterEvade(target, rng)) {
                stateChanges.DidDodgeOrMiss = true;
            }
            else {
                foreach (var status in ability.StatusEffectsAdded) {
                    stateChanges.StatusesRemoved.Add(status);
                }
            }
        }

        private static void AbilityStatModification(JobAbility ability, IUnitState target, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            if (CombatHelper.DoesCharacterEvade(target, rng)) {
                stateChanges.DidDodgeOrMiss = true;
            }
            else {
                foreach (var stat in ability.StatsModified.Keys) {
                    if (ability.StatsModified[stat].StatModifierChance <= 0) {
                        throw new Exception(string.Format("Ability {0} has a stat modifer chance equal to or less " +
                            "than 0 for stat {1}. Why tho?", ability.Name, stat.ToString()));
                    }
                    else if (rng.Randf() <= ability.StatsModified[stat].StatModifierChance) {
                        if (ability.StatsModified[stat].StatModifierAmount > 0) {
                            if (stateChanges.StatModifiers.Keys.Contains(stat)) {
                                stateChanges.StatModifiers[stat] += ability.StatsModified[stat].StatModifierAmount;
                            }
                            else {
                                stateChanges.StatModifiers.Add(stat, ability.StatsModified[stat].StatModifierAmount);
                            }
                        }
                        else if (ability.StatsModified[stat].StatModifierPercent > 0) {
                            float amount = target.GetStat(stat) *  ability.StatsModified[stat].StatModifierPercent;

                            if (stateChanges.StatModifiers.Keys.Contains(stat)) {
                                stateChanges.StatModifiers[stat] += amount;
                            }
                            else {
                                stateChanges.StatModifiers.Add(stat, amount);
                            }
                        }
                        else {
                            throw new Exception(string.Format("Ability {0} has the ModifiesStats flag set to true " +
                                "but StatModifierAmount and StatModiferPercent for stat {1} are 0 or less or empty", ability.Name, stat.ToString()));
                        }
                    }
                }
            }
        }

        private static void AbilitySelfStatModification(IUnitState unit, JobAbility ability, UnitStateChanges stateChanges, RandomNumberGenerator rng) {
            foreach (var stat in ability.StatsModified.Keys) {
                if (ability.StatsModified[stat].StatModifierChance <= 0) {
                    throw new Exception(string.Format("Ability {0} has a stat modifer chance equal to or less " +
                        "than 0 for stat {1}. Why tho?", ability.Name, stat.ToString()));
                }
                else if (rng.Randf() <= ability.StatsModified[stat].StatModifierChance) {
                    if (ability.StatsModified[stat].StatModifierAmount > 0) {
                        stateChanges.StatModifiers.Add(stat, ability.StatsModified[stat].StatModifierAmount);
                    }
                    else if (ability.StatsModified[stat].StatModifierPercent > 0) {
                        float amount = unit.GetStat(stat) *  ability.StatsModified[stat].StatModifierPercent;
                        stateChanges.StatModifiers.Add(stat, amount);
                    }
                    else {
                        throw new Exception(string.Format("Ability {0} has the ModifiesStats flag set to true " +
                            "but StatModifierAmount and StatModiferPercent for stat {1} are 0 or less or empty", ability.Name, stat.ToString()));
                    }
                }
            }
        }
        #endregion
    }
}