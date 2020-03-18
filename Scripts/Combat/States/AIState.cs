using Scripts.Characters;
using System.Collections.Generic;

namespace Scripts.Combat.States {
    public class AIState : CharacterState {
        public Dictionary<CharacterState, float> AggroList { get; private set; }

        public AIState(Character baseCharacter, Type type) : base(baseCharacter, type) {
            AggroList = new Dictionary<CharacterState, float>();
        }

        public void AddAggroTarget(CharacterState target) {
            AggroList.Add(target, 0f);
        }

        public void RemoveAggroTarget(CharacterState target){
            AggroList.Remove(target);
        }

        public void GetTurn(){
            
        } 
    }
}