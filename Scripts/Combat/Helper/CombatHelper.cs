using Assets.Scripts.Actors.Jobs;
using Assets.Scripts.Combat.States;
using Assets.Scripts.Interfaces.Combat;
using Assets.Scripts.Items.Equipment;
using System;
using System.ComponentModel;
using Godot;

namespace Assets.Scripts.Combat.Helper {
    public class CombatHelper {

        /* Abilities and equipment that affect ability hit chance
         * will be added in the future, and need to be accounted for here.
         */
        //TODO Add equipment/ability hit chance modifiers to abilities
        public static bool DoesAbilityMiss(IUnitState unit, JobAbility ability, RandomNumberGenerator rng) {
            return rng.Randf() > ability.Accuracy;
        }

        /* Abilities and equipemnt that affect attack hit chance
         * will be added in the future, and need to be accounted for here.
         */
        //TODO Add equipment/ability hit chance modifiers to attacks
        public static bool DoesAttackHit(IUnitState unit, float hitChance, RandomNumberGenerator rng) {
            return rng.Randf() <= hitChance;
        }

        /* Abilities and equipemnt that affect evade chance
         * will be added in the future, and need to be accounted for here.
         */
        //TODO Add equipment/ability hit chance modifiers to evade chance
        public static bool DoesCharacterEvade(IUnitState unit, RandomNumberGenerator rng) {
            return rng.Randf() <= unit.GetStat(Stat.Evade);
        }

        public static float GetAttackStat(IUnitState unit, DamageType type) {
            switch (type) {
                case DamageType.Melee:
                    float stat = unit.GetStat(Stat.Strength);
                    return unit.GetStat(Stat.Strength);
                case DamageType.Ranged:
                    return unit.GetStat(Stat.Dexterity);
                case DamageType.Magical:
                    return unit.GetStat(Stat.Intellect);
                default:
                    throw new InvalidEnumArgumentException(string.Format("DamageType enum {0} doesn't exist. How did you accomplish that?", type));
            }
        }

        public static float GetDefenseStat(IUnitState unit, DamageType type) {
            switch (type) {
                case DamageType.Melee:
                case DamageType.Ranged:
                    return unit.GetStat(Stat.Constitution);
                case DamageType.Magical:
                    return unit.GetStat(Stat.Wisdom);
                default:
                    throw new InvalidEnumArgumentException(string.Format("DamageType enum {0} doesn't exist. How did you accomplish that?", type));
            }
        }

        public static float GetCriticalStat(IUnitState unit, DamageType type) {
            switch (type) {
                case DamageType.Melee:
                case DamageType.Ranged:
                    return unit.GetStat(Stat.PhysicalCritChance);
                case DamageType.Magical:
                    return unit.GetStat(Stat.MagicalCritChance);
                default:
                    throw new InvalidEnumArgumentException(string.Format("DamageType enum {0} doesn't exist. How did you accomplish that?", type));
            }
        }

        public static float GetWeaponPower(Weapon weapon) {
            switch (weapon.DamageType) {
                case DamageType.Melee:
                    return weapon.MeleePower;
                case DamageType.Ranged:
                    return weapon.RangedPower;
                case DamageType.Magical:
                    return weapon.MagicalPower;
                default:
                    throw new InvalidEnumArgumentException(string.Format("DamageType enum {0} doesn't exist. How did you accomplish that?", weapon.DamageType));
            }
        }

        public static float CalculateDamage(float attack, float power, float defense, float critChance, float critMultiplier, RandomNumberGenerator rng) {
            return attack / defense * power * (rng.Randf() <= critChance ? critMultiplier : 1);
        }

        public static float CalculateHeal(float stat, float power, float critChance, float critMultiplier, RandomNumberGenerator rng) {
            return stat * power / 100 * (rng.Randf() <= critChance ? critMultiplier : 1);
        }

        public static int GetAttackerAttackVector2(IUnitState attacker, Vector2 atkPos, IUnitState defender, Vector2 defPos) {

            var pos = defPos - atkPos;

            if (-pos.y <= pos.x && pos.x <= pos.y) {
                return 0;
            }
            else if (-pos.x < pos.y && pos.y < pos.x) {
                return 1;
            }
            else if (-pos.y >= pos.x && pos.x >= pos.y) {
                return 2;
            }
            else if (-pos.x > pos.y && pos.y > pos.x) {
                return 3;
            }
            else {
                throw new Exception(string.Format("Attacker and/or defender position(s) aren't/isn't valid:\n\tAttacker Vector2: {0}\n\tDefender Vector2: {1}", atkPos.ToString(), defPos.ToString()));
            }
        }

        public static float GetFacingHitChance(int attackerVector2, int defenderFacing) {
            switch (defenderFacing) {
                case 0:
                    switch (attackerVector2) {
                        case 0:
                            return 0.5f;
                        case 1:
                        case 3:
                            return 0.8f;
                        case 2:
                            return 1f;
                        default:
                            throw new Exception(string.Format("Attacker's position isn't valid, should be 0 - 3: {0}", attackerVector2));
                    }
                case 1:
                    switch (attackerVector2) {
                        case 0:
                        case 2:
                            return 0.8f;
                        case 1:
                            return 0.5f;
                        case 3:
                            return 1f;
                        default:
                            throw new Exception(string.Format("Attacker's position isn't valid, should be 0 - 3: {0}", attackerVector2));
                    }
                case 2:
                    switch (attackerVector2) {
                        case 0:
                            return 1f;
                        case 1:
                        case 3:
                            return 0.8F;
                        case 2:
                            return 0.5F;
                        default:
                            throw new Exception(string.Format("Attacker's position isn't valid, should be 0 - 3: {0}", attackerVector2));
                    }
                case 3:
                    switch (attackerVector2) {
                        case 0:
                        case 2:
                            return 0.8f;
                        case 1:
                            return 1f;
                        case 3:
                            return 0.5f;
                        default:
                            throw new Exception(string.Format("Attacker's position isn't valid, should be 0 - 3: {0}", attackerVector2));
                    }
                default:
                    throw new Exception(string.Format("Defender's facing isn't valid, should be 0 - 3: {0}", defenderFacing));
            }
        }
    }
}