using Scripts.Characters.Jobs;
using Scripts.Items;
using Scripts.Items.Accessories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Characters {
    public class Character {
        public struct JobMetadata {
            public bool Unlocked;
            public int Level;
            public int EXP;
            public List<JobAbility> UnlockedAbilities;
        }

        public string Name { get; private set; }
        public int CharacterLevel { get; private set; }
        public Job CurrentJob { get; private set; }
        public int CurrentJobLevel { get; private set; }
        public Dictionary<Job, JobMetadata> JobMeta { get; private set; }
        public List<Job> EquipedJobAbilities { get; private set; }

        //Resources
        public float Health { get; private set; }
        public float Mana { get; private set; }
        public float HealthRegen { get; private set; }
        public float ManaRegen { get; private set; }
        //Attack Stats
        public float Strength { get; private set; }
        public float Dexterity { get; private set; }
        public float Intellect { get; private set; }
        public float PhysicalCritChance { get; private set; }
        public float MagicalCritChance { get; private set; }
        //Defense Stats
        public float Constitution { get; private set; }
        public float Wisdom { get; private set; }
        public float Evade { get; private set; }
        public float Block { get; private set; }
        public float FireResistence { get; private set; }
        public float LightningResistence { get; private set; }
        public float ColdResistence { get; private set; }
        //MovementStats
        public float Move { get; private set; }
        public float Speed { get; private set; }
        public float Jump { get; private set; }

        public Equipment MainHand { get; private set; }
        public Equipment OffHand { get; private set; }
        public Equipment HeadArmor { get; private set; }
        public Equipment BodyArmor { get; private set; }
        public Equipment LegArmor { get; private set; }
        public Accessory AccessoryOne { get; private set; }
        public Accessory AccessoryTwo { get; private set; }

        public int RemoveMainHand() {
            var temp = MainHand.ID;
            MainHand = new Equipment();
            return temp;
        }

        public int RemoveOffHand() {
            var temp = OffHand.ID;
            OffHand = new Equipment();
            return temp;
        }

        public int RemoveHeadArmor() {
            var temp = HeadArmor.ID;
            HeadArmor = new Equipment();
            return temp;
        }

        public int RemoveBodyArmor() {
            var temp = BodyArmor.ID;
            BodyArmor = new Equipment();
            return temp;
        }

        public int RemoveLegArmor() {
            var temp = LegArmor.ID;
            LegArmor = new Equipment();
            return temp;
        }

        public Accessory RemoveAccessoryOne() {
            var temp = AccessoryOne;
            AccessoryOne = new Accessory();
            return temp;
        }

        public Accessory RemoveAccessoryTwo() {
            var temp = AccessoryTwo;
            AccessoryTwo = new Accessory();
            return temp;
        }

        public void EquipMainHand(Equipment mainHand) {

        }

        public void EquipOffHand(Equipment offHand) {

        }

        public void EquipBodyArmor(Equipment armor) {
            if (BodyArmor != null || BodyArmor.EquipmentType == EquipmentType.Empty) {

            }
            else {

            }
        }

        public float GetTotalHealth() {
            return Health + MainHand.Health + OffHand.Health + HeadArmor.Health + BodyArmor.Health
            + LegArmor.Health + AccessoryOne.Health + AccessoryTwo.Health
            + (Health * AccessoryOne.HealthPercent) + (Health * AccessoryTwo.HealthPercent);
        }

        public float GetTotalMana() {
            return Mana + MainHand.Mana + OffHand.Mana + HeadArmor.Mana + BodyArmor.Mana
            + LegArmor.Mana + AccessoryOne.Mana + AccessoryTwo.Mana
            + (Mana * AccessoryOne.ManaPercent) + (Mana * AccessoryTwo.ManaPercent);
        }

        public float GetTotalHealthRegen() {
            return HealthRegen + MainHand.HealthRegen + OffHand.HealthRegen + HeadArmor.HealthRegen + BodyArmor.HealthRegen
            + LegArmor.HealthRegen + AccessoryOne.HealthRegen + AccessoryTwo.HealthRegen;
        }

        public float GetTotalManaRegen() {
            return ManaRegen + MainHand.HealthRegen + OffHand.ManaRegen + HeadArmor.ManaRegen + BodyArmor.ManaRegen
            + LegArmor.ManaRegen + AccessoryOne.ManaRegen + AccessoryTwo.ManaRegen;
        }

        public float GetTotalStrength() {
            return Strength + MainHand.Strength + OffHand.Strength + HeadArmor.Strength + BodyArmor.Strength
            + LegArmor.Strength + AccessoryOne.Strength + AccessoryTwo.Strength
            + (Strength * AccessoryOne.StrengthPercent) + (Strength * AccessoryTwo.StrengthPercent);
        }

        public float GetTotalDexterity() {
            return Dexterity + MainHand.Dexterity + OffHand.Dexterity + HeadArmor.Dexterity + BodyArmor.Dexterity
            + LegArmor.Dexterity + AccessoryOne.Dexterity + AccessoryTwo.Dexterity
            + (Dexterity * AccessoryOne.DexterityPercent) + (Dexterity * AccessoryTwo.DexterityPercent);
        }

        public float GetTotalIntellect() {
            return Intellect + MainHand.Intellect + OffHand.Intellect + HeadArmor.Intellect + BodyArmor.Intellect
            + LegArmor.Intellect + AccessoryOne.Intellect + AccessoryTwo.Intellect
            + (Intellect * AccessoryOne.IntellectPercent) + (Intellect * AccessoryTwo.IntellectPercent);
        }

        public float GetTotalPhysicalCritChance() {
            return PhysicalCritChance + MainHand.PhysicalCritChance + OffHand.PhysicalCritChance + HeadArmor.PhysicalCritChance + BodyArmor.PhysicalCritChance
            + LegArmor.PhysicalCritChance + AccessoryOne.PhysicalCritChance + AccessoryTwo.PhysicalCritChance
            + (PhysicalCritChance * AccessoryOne.PhysicalCritChancePercent) + (PhysicalCritChance * AccessoryTwo.PhysicalCritChancePercent);
        }

        public float GetTotalMagicalCritChance() {
            return MagicalCritChance + MainHand.MagicalCritChance + OffHand.MagicalCritChance + HeadArmor.MagicalCritChance + BodyArmor.MagicalCritChance
            + LegArmor.MagicalCritChance + AccessoryOne.MagicalCritChance + AccessoryTwo.MagicalCritChance
            + (MagicalCritChance * AccessoryOne.MagicalCritChancePercent) + (MagicalCritChance * AccessoryTwo.MagicalCritChancePercent);
        }

        public float GetTotalConstitution() {
            return Constitution + OffHand.Constitution + HeadArmor.Constitution + BodyArmor.Constitution
            + LegArmor.Constitution + AccessoryOne.Constitution + AccessoryTwo.Constitution
            + (Constitution * AccessoryOne.ConstitutionPercent) + (Constitution * AccessoryTwo.ConstitutionPercent);
        }

        public float GetTotalWisdom() {
            return Wisdom + OffHand.Wisdom + HeadArmor.Wisdom + BodyArmor.Wisdom
            + LegArmor.Wisdom + AccessoryOne.Wisdom + AccessoryTwo.Wisdom
            + (Wisdom * AccessoryOne.WisdomPercent) + (Wisdom * AccessoryTwo.WisdomPercent);
        }

        public float GetTotalEvadeChance() {
            float baseChance = BodyArmor.Evade + LegArmor.Evade + AccessoryOne.Evade + AccessoryTwo.Evade;
            if (IsOffHandAShield()) {
                return OffHand.Evade + baseChance;
            }
            else if (IsMainHandABow()) {
                return MainHand.Evade + baseChance;
            }
            else {
                return baseChance;
            }
        }

        public float GetTotalBlockChance() {
            if (IsOffHandAShield()) {
                return OffHand.Block + AccessoryOne.Block + AccessoryTwo.Block;
            }
            return 0;
        }

        public float GetTotalFireResistence() {
            var baseRes = HeadArmor.FireResistence + BodyArmor.FireResistence + LegArmor.FireResistence + AccessoryOne.FireResistence + AccessoryTwo.FireResistence;
            if (IsOffHandAShield()) {
                return OffHand.FireResistence + baseRes;
            }
            else {
                return baseRes;
            }
        }

        public float GetTotalLightningResistence() {
            var baseRes = HeadArmor.LightningResistence + BodyArmor.LightningResistence + LegArmor.LightningResistence + AccessoryOne.LightningResistence + AccessoryTwo.LightningResistence;
            if (IsOffHandAShield()) {
                return OffHand.LightningResistence + baseRes;
            }
            else {
                return baseRes;
            }
        }

        public float GetTotalColdResistence() {
            var baseRes = HeadArmor.ColdResistence + BodyArmor.ColdResistence + LegArmor.ColdResistence + AccessoryOne.ColdResistence + AccessoryTwo.ColdResistence;
            if (IsOffHandAShield()) {
                return OffHand.ColdResistence + baseRes;
            }
            else {
                return baseRes;
            }
        }

        public float GetTotalMove() {
            return BodyArmor.Move + LegArmor.Move + AccessoryOne.Move + AccessoryTwo.Move;
        }

        public float GetTotalSpeed() {
            return BodyArmor.Speed + LegArmor.Speed + AccessoryOne.Speed + AccessoryTwo.Speed;
        }

        public float GetTotalJump() {
            return BodyArmor.Jump + LegArmor.Jump + AccessoryOne.Jump + AccessoryTwo.Jump;
        }

        public bool IsMainHandASword() {
            EquipmentType equipType = MainHand.EquipmentType;
            return equipType == EquipmentType.GreatSword || equipType == EquipmentType.LongSword || equipType == EquipmentType.Katana || equipType == EquipmentType.ShortSword || equipType == EquipmentType.Dagger;
        }

        public bool IsOffHandASword() {
            EquipmentType equipType = OffHand.EquipmentType;
            return equipType == EquipmentType.ShortSword || equipType == EquipmentType.Dagger;
        }

        public bool IsMainHandABow() {
            EquipmentType equipType = MainHand.EquipmentType;
            return equipType == EquipmentType.ShortBow || equipType == EquipmentType.LongBow || equipType == EquipmentType.GreatBow;
        }

        public bool IsMainHandAnAxe() {
            EquipmentType equipType = MainHand.EquipmentType;
            return equipType == EquipmentType.BattleAxe || equipType == EquipmentType.GreatAxe;
        }

        public bool IsOffHandAnAxe() {
            return MainHand.EquipmentType == EquipmentType.BattleAxe;
        }

        public bool IsMainHandAHammer() {
            EquipmentType equipType = MainHand.EquipmentType;
            return equipType == EquipmentType.BattleHammer || equipType == EquipmentType.GreatHammer;
        }

        public bool IsOffHandAHammer() {
            return MainHand.EquipmentType == EquipmentType.BattleHammer;
        }

        public bool IsOffHandAShield() {
            EquipmentType equipType = OffHand.EquipmentType;
            return equipType == EquipmentType.Buckler || equipType == EquipmentType.KiteShield || equipType == EquipmentType.TowerShield;
        }

        public bool IsMainHand2Handed() {
            EquipmentType equipType = MainHand.EquipmentType;
            return equipType == EquipmentType.GreatSword || equipType == EquipmentType.LongSword || equipType == EquipmentType.Katana
            || equipType == EquipmentType.GreatHammer || equipType == EquipmentType.GreatAxe || equipType == EquipmentType.ShortBow
            || IsMainHandABow();
        }

        public bool IsEmptyHanded() {
            return MainHand.EquipmentType == EquipmentType.Empty && OffHand.EquipmentType == EquipmentType.Empty;
        }

        public bool IsDualWielding() {
            return MainHand.EquipmentType != EquipmentType.Empty && !IsMainHand2Handed() && (IsOffHandAHammer() || IsOffHandAnAxe() || IsOffHandASword());
        }

        public bool CanDualWield() {
            return !IsEmptyHanded() && !IsMainHand2Handed();
        }

        public Character Copy() {
            var copy = new Character();
            copy.Name = Name;
            copy.CharacterLevel = CharacterLevel;
            copy.CurrentJob = CurrentJob;
            copy.JobMeta = JobMeta;
            copy.EquipedJobAbilities = EquipedJobAbilities;
            copy.Health = Health;
            copy.Mana = Mana;
            copy.HealthRegen = HealthRegen;
            copy.ManaRegen = ManaRegen;
            copy.Strength = Strength;
            copy.Dexterity = Dexterity;
            copy.Intellect = Intellect;
            copy.PhysicalCritChance = PhysicalCritChance;
            copy.MagicalCritChance = MagicalCritChance;
            copy.Constitution = Constitution;
            copy.Wisdom = Wisdom;
            copy.Evade = Evade;
            copy.Block = Block;
            copy.FireResistence = FireResistence;
            copy.LightningResistence = LightningResistence;
            copy.ColdResistence = ColdResistence;
            copy.Move = Move;
            copy.Speed = Speed;
            copy.Jump = Jump;
            copy.MainHand = MainHand;
            copy.OffHand = OffHand;
            copy.HeadArmor = HeadArmor;
            copy.BodyArmor = BodyArmor;
            copy.LegArmor = LegArmor;
            copy.AccessoryOne = AccessoryOne;
            copy.AccessoryTwo = AccessoryTwo;

            return copy;
        }
    }
}
