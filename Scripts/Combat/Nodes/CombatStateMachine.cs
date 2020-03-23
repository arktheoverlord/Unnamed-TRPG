using Godot;
using System;
using System.Collections.Generic;
using Scripts.Combat.States;
using Scripts.Combat.Mapping;
using Scripts.Characters;
using Scripts.Characters.Jobs;
using Scripts.Items;

namespace Scripts.Combat.Nodes {
    public class CombatStateMachine : Node {
        public List<CharacterState> Units { get; private set; }

        private Dictionary<CharacterState, StateChanges> uncommitedChanges;
        
        private int currentID = 1;

        private List<Vector3> LastMovementArea;

        #region Signals
        [Signal]
        public delegate void ValidMovementLocationSelected();
        #endregion
        public override void _Ready() {
            Units = new List<CharacterState>();
            LastMovementArea = new List<Vector3>();
        }

        ///<summary>Initilize the state machine with the give set of NPCs and Guests</summary>.
        public void Initilize(List<Character> NPCs, List<Character> Guests = null) {
            for (int i = 0; i < NPCs.Count; i++) {
                AddNPC(NPCs[i]);
            }

            if (Guests != null || Guests.Count > 0) {
                for (int i = 0; i < Guests.Count; i++) {
                    AddGuest(Guests[i]);
                }
            }
        }

        public void CommitChanges() {

        }

        public void UnitAbility(CharacterState unit, Tile target, JobAbility ability) {

        }

        public void UnitAttack(CharacterState unit, Tile target) {

        }

        #region Movement
        public void UnitMove(CharacterState unit, Vector3 location) {

        }

        public void GetMoveArea(int id, Vector3 center, HUD hud) {
            CharacterState character = null;
            foreach (var unit in Units) {
                if (unit.ID == id) {
                    character = unit;
                }
            }

            List<Vector3> area = new List<Vector3>();
            for (int x = (int)character.BaseCharacter.Move; x > 0; x--) {
                for (int z = ((int)character.BaseCharacter.Move) - x; z >= 0; z--) {
                    area.Add(center + new Vector3(x, -0.5f, z));
                    area.Add(center + new Vector3(-x, -0.5f, z));
                    if (z > 0) {
                        area.Add(center + new Vector3(x, -0.5f, -z));
                        area.Add(center + new Vector3(-x, -0.5f, -z));
                    }
                }
            }

            for (int z = (int)character.BaseCharacter.Move; z > (int)-character.BaseCharacter.Move - 1; z--) {
                area.Add(center - new Vector3(0, 0.5f, z));
            }

            area.Remove(center - new Vector3(0, 0.5f, 0));

            LastMovementArea = area;

            hud.DisplayMovementArea(area);
        }

        private void OnMovementLocationSelected(int id, Vector3 location){
            if(LastMovementArea.Contains(location)){
                EmitSignal(nameof(ValidMovementLocationSelected));
            }
        }
        #endregion

        public void UnitItem(CharacterState unit, Tile target) {

        }

        public void OnCharacterHighlighted(int id, HUD hud) {
            foreach (var unit in Units) {
                if (unit.ID == id) {
                    hud.ShowCharacterPanel(unit);
                    return;
                }
            }
        }

        #region Character Adding
        public void OnCharacterAdded(Character character, CombatCharacterManager ccm, Team team) {
            switch (team) {
                case Team.PC:
                    AddPC(character);
                    break;
                case Team.NPC:
                    AddNPC(character);
                    break;
                case Team.Guest:
                    AddGuest(character);
                    break;
            }

            if (character.CharacterName == "TestTestTest") {
                Units[0].Position = new Vector3(1.5f, 0.51f, 1.5f);
                ccm.AddCharacter(currentID - 1, new Vector3(1.5f, 0.51f, 1.5f));
            }
            else {

            }
        }

        public void AddPC(Character PC) {
            AddPC(new CharacterState(PC, Team.PC, currentID));
            currentID++;
        }

        public void AddPC(CharacterState PC) {
            if (PC.ID != currentID) {
                PC.ID = currentID;
                currentID++;
            }
            Units.Add(PC);

            foreach (var s in Units) {
                if (s.CharacterTeam == Team.NPC) {
                    ((AIState)s).AddAggroTarget(PC);
                }
            }
        }

        public void RemovePC(CharacterState PC) {
            Units.Remove(PC);
            foreach (var s in Units) {
                if (s.CharacterTeam == Team.NPC) {
                    ((AIState)s).RemoveAggroTarget(PC);
                }
            }
        }

        public void AddNPC(Character NPC) {
            AddNPC(new AIState(NPC, Team.NPC, currentID));
            currentID++;
        }

        public void AddNPC(CharacterState NPC) {
            if (NPC.ID != currentID) {
                NPC.ID = currentID;
                currentID++;
            }
            Units.Add(NPC);

            foreach (var s in Units) {
                if (s.CharacterTeam == Team.Guest) {
                    ((AIState)s).AddAggroTarget(NPC);
                }
            }
        }

        public void RemoveNPC(CharacterState NPC) {
            Units.Remove(NPC);
            foreach (var s in Units) {
                if (s.CharacterTeam == Team.Guest) {
                    ((AIState)s).RemoveAggroTarget(NPC);
                }
            }
        }

        public void AddGuest(Character Guest) {
            AddGuest(new CharacterState(Guest, Team.PC, currentID));
            currentID++;
        }

        public void AddGuest(CharacterState Guest) {
            if (Guest.ID != currentID) {
                Guest.ID = currentID;
                currentID++;
            }
            Units.Add(Guest);

            foreach (var s in Units) {
                if (s.CharacterTeam == Team.NPC) {
                    ((AIState)s).AddAggroTarget(Guest);
                }
            }

        }

        public void RemoveGuest(CharacterState Guest) {
            Units.Remove(Guest);
            foreach (var s in Units) {
                if (s.CharacterTeam == Team.NPC) {
                    ((AIState)s).RemoveAggroTarget(Guest);
                }
            }
        }
        #endregion
    }
}
