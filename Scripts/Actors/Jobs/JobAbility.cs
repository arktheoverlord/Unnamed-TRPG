using Scripts.Combat;
using Scripts.Combat.States;
using Scripts.Helpers;
using System.Collections.Generic;
using static Scripts.Helpers.ExtraEffectHelper;

namespace Scripts.Actors.Jobs {
    public class JobAbility {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Accuracy { get; set; }
        public float MPCostAmount { get; set; }
        public float MPCostPercent { get; set; }
        public int Range { get; set; }
        public bool Passive { get; set; }
        public int Element { get; set; } //TODO Change type once elements are added

        public bool Heals { get; set; } = false;
        public float HealPower { get; set; }
        public float HealPercent { get; set; }
        public float HealAmount { get; set; }

        public bool HealsMP { get; set; } = false;
        public float MPHealPercent { get; set; }
        public float MPHealAmount { get; set; }

        public bool HealsSelf { get; set; } = false;
        public float SelfHealPower { get; set; }
        public float SelfHealPercent { get; set; }
        public float SelfHealAmount { get; set; }

        public bool HealsSelfMP { get; set; } = false;
        public float SelfMPHealPercent { get; set; }
        public float SelfMPHealAmount { get; set; }

        public DamageType DamageType { get; set; }

        public bool Damages { get; set; } = false;
        public float DamagePower { get; set; }
        public float DamagePercent { get; set; }
        public float DamageAmount { get; set; }

        public bool DamagesMP { get; set; } = false;
        public float MPDamagePercent { get; set; }
        public float MPDamageAmount { get; set; }

        public bool DamagesSelf { get; set; } = false;
        public float SelfDamagePower { get; set; }
        public float SelfDamagePercent { get; set; }
        public float SelfDamageAmount { get; set; }

        public bool HasRecoil { get; set; } = false;
        public float RecoilPercent { get; set; }

        public int Targets { get; set; }
        public int MaxDistenceBetweenTargets { get; set; }
        public bool SplitDamageBetweenTargets { get; set; }
        public List<float> DamageSplits { get; set; }

        public bool AddsStatusEffects { get; set; }
        public List<StatusEffect> StatusEffectsAdded { get; set; }
        public List<float> EffectChances { get; set; }

        public bool RemovesStatusEffects { get; set; }
        public List<StatusEffect> StatusEffectsRemoved { get; set; }

        public bool AddsSelfStatusEffects { get; set; }
        public List<StatusEffect> SelfStatusEffectsAdded { get; set; }
        public List<float> SelfEffectChances { get; set; }

        public bool RemovesSelfStatusEffects { get; set; }
        public List<StatusEffect> SelfStatusEffectsRemoved { get; set; }

        public bool ModifiesStats { get; set; } = false;
        public Dictionary<Stat, StatModifier> StatsModified { get; set; }

        public bool ModifiesSelfStats { get; set; } = false;
        public Dictionary<Stat, StatModifier> SelfStatsModified { get; set; }

        public bool ModifiesAggression { get; set; } = false;
        public int AggressionModifier { get; set; }
        public int AggressionModifierPercent { get; set; }

        public bool HasExtraEffect { get; set; } = false;
        public ExtraEffect ExtraAbilityEffect { get; set; }

        public class StatModifier {
            public float StatModifierAmount { get; set; }
            public float StatModifierPercent { get; set; }
            public float StatModifierChance { get; set; }
        }
    }

    public enum DamageType {
        Melee, Ranged, Magical
    }
}
