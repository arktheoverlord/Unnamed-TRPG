using TRPG.Characters;
using System.Collections.Generic;

namespace TRPG.Combat.States {
    public class AIState : CharacterState {
        public Dictionary<CharacterState, float> AggroList { get; private set; }

        public AIState(Character baseCharacter, Team type, int id) : base(baseCharacter, type, id) {
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