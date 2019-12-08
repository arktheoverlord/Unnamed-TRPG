using System.Collections.Generic;
using Godot;

namespace Assets.Scripts.Combat.States {
    public class UnitStateChanges {
        public bool DidDodgeOrMiss { get; set; }

        public float HPHeal { get; set; }
        public float HPDamage { get; set; }

        public float MPHeal { get; set; }
        public float MPDamage { get; set; }
        
        public List<StatusEffect> StatusesAdded { get; set; }
        public List<StatusEffect> StatusesRemoved { get; set; }

        public Vector2 PositionMovedTo { get; set; }
        public int FacingChangedTo { get; set; }

        public Dictionary<Stat, float> StatModifiers { get; set; }
    }
}
