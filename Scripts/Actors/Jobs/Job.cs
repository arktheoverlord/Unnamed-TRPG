
namespace Assets.Scripts.Actors.Jobs {
    public class Job {
        public struct UnlockRequirment {
            public Job Job { get; set; }
            public int Level { get; set; }
        }

        public struct AbilityUnlock {
            public JobAbility Ability { get; set; }
            public int Level { get; set; }
            public int Quest { get; set; }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxLevel { get; set; }
        public byte EXPProgression { get; set; }

        public int HPProgression { get; set; }
        public int MPProgression { get; set; }
        public int StrengthProgression { get; set; }
        public int DexterityProgession { get; set; }
        public int IntellectProgression { get; set; }
        public int ConstitutionProgression { get; set; }
        public int WisdomProgression { get; set; }
        public int PhysicalCritChanceProgression { get; set; }
        public int MagicalCritChanceProgression { get; set; }
        public int SpeedProgression { get; set; }
        public int MoveProgression { get; set; }
        public int EvadeProgresion { get; set; }
        public int BlockProgression { get; set; }
        public int JumpProgression { get; set; }
    }
}
