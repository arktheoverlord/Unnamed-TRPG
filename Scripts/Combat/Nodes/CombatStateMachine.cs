using Godot;
using System;
using System.Collections.Generic;
using TRPG.Combat.States;
using TRPG.Combat.Mapping;
using TRPG.Characters;
using TRPG.Characters.Jobs;
using TRPG.Items;

namespace TRPG.Combat.Nodes {
    public class CombatStateMachine : Node {
        public List<CharacterState> Units { get; private set; }

        private Dictionary<CharacterState, StatChanges> uncommitedChanges;

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

        private void OnMovementLocationSelected(int id, Vector3 location) {
            if (LastMovementArea.Contains(location)) {
                EmitSignal(nameof(ValidMovementLocationSelected));
            }
        }
        #endregion

        public void UnitItem(CharacterState unit, Tile target) {

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
                foreach (var unit in Units) {
                    if (unit.ID == currentID - 1) {
                        StatChanges changes = new StatChanges();
                        var pos = new Vector3(-1, 3, -1);
                        changes.Position = pos;
                        unit.TurnEndUpdate(changes);
                        ccm.AddCharacter(unit, pos);
                        break;
                    }
                }
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
                if (PC.SetID(currentID)) {
                    currentID++;
                }
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
                if (NPC.SetID(currentID)) {
                    currentID++;
                }
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
                if (Guest.SetID(currentID)) {
                    currentID++;
                }
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
