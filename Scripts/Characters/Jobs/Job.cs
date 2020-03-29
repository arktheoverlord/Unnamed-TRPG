namespace TRPG.Characters.Jobs {
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

        public string Name { get; set; } = "Test Job Please Ignore";
        public string Description { get; set; } = "";
        public int MaxLevel { get; set; } = 0;
        public byte EXPProgression { get; set; } = 0;

        public float HPProgression { get; set; } = 0;
        public float MPProgression { get; set; } = 0;
        public float StrengthProgression { get; set; } = 0;
        public float DexterityProgession { get; set; } = 0;
        public float IntellectProgression { get; set; } = 0;
        public float ConstitutionProgression { get; set; } = 0;
        public float WisdomProgression { get; set; } = 0;
        public float PhysicalCritChanceProgression { get; set; } = 0;
        public float MagicalCritChanceProgression { get; set; } = 0;
        public float SpeedProgression { get; set; } = 0;
        public float MoveProgression { get; set; } = 0;
        public float EvadeProgresion { get; set; } = 0;
        public float BlockProgression { get; set; } = 0;
        public float JumpProgression { get; set; } = 0;
    }
}
