using Scripts.Actors.Jobs;
using Scripts.Items;
using Scripts.Items.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Actors {
    public class Character {
        public struct JobMetadata {
            public bool Unlocked;
            public int Level;
            public int EXP;
            public List<JobAbility> UnlockedAbilities;
        }

        public string Name { get; set; }
        public int CharacterLevel { get; set; }
        public Job CurrentJob { get; set; }
        public int CurrentJobLevel { get; set; }
        public Dictionary<Job, JobMetadata> JobMeta { get; set; }
        public List<Job> EquipedJobAbilities { get; set; }

        public float HP { get; set; }
        public float MP { get; set; }
        public float Strength { get; set; }
        public float Dexterity { get; set; }
        public float Intellect { get; set; }
        public float PhysicalCritChance { get; set; }
        public float MagicalCritChance { get; set; }
        public float Constitution { get; set; }
        public float Wisdom { get; set; }
        public float Evade { get; set; }
        public float Block { get; set; }
        public float Move { get; set; }
        public float Speed { get; set; }
        public float Jump { get; set; }
        public float HPRegen { get; set; }
        public float MPRegen { get; set; }

        public Item MainHand { get; set; }
        public Item OffHand { get; set; }
        public HeadArmor Helmet { get; set; }
        public BodyArmor BodyArmor { get; set; }
        public LegArmor LegArmor { get; set; }
        public List<Accessory> Accessories { get; set; }

        public Character Copy() {
            var copy = new Character();
            copy.Name = Name;
            copy.CharacterLevel = CharacterLevel;
            copy.CurrentJob = CurrentJob;
            copy.JobMeta = JobMeta;
            copy.EquipedJobAbilities = EquipedJobAbilities;
            copy.HP = HP;
            copy.MP = MP;
            copy.Strength = Strength;
            copy.Dexterity = Dexterity;
            copy.Intellect = Intellect;
            copy.PhysicalCritChance = PhysicalCritChance;
            copy.MagicalCritChance = MagicalCritChance;
            copy.Constitution = Constitution;
            copy.Wisdom = Wisdom;
            copy.Evade = Evade;
            copy.Block = Block;
            copy.Move = Move;
            copy.Speed = Speed;
            copy.Jump = Jump;
            copy.HPRegen = HPRegen;
            copy.MPRegen = MPRegen;
            copy.MainHand = MainHand;
            copy.OffHand = OffHand;
            copy.Helmet = Helmet;
            copy.BodyArmor = BodyArmor;
            copy.LegArmor = LegArmor;
            copy.Accessories = Accessories;

            return copy;
        }
    }
}
