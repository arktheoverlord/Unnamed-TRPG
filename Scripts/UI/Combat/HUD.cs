using Godot;
using System.Collections.Generic;
using Scripts.Combat.States;
using Scripts.Combat.Nodes;
using Scripts.Characters;

namespace Scripts.UI.Combat {
    public class HUD : Node {
        //Debug Stuff
        private Label test;
        private Label fpsCounter;

        private Panel characterPanel;
        private Label characterName;
        private Label characterLevel;
        private Label characterClass;
        private Label HP;
        private Label MP;

        private Panel actionPanel;
        private bool isMoving = false;

        private CharacterBody body;

        #region Signals
        [Signal]
        public delegate void OnExit();

        [Signal]
        public delegate void MoveButtonPressed(CharacterBody body);
        #endregion

        public override void _Ready() {
            test = GetNode<Label>("Test");
            fpsCounter = GetNode<Label>("FPSCounter");
            characterPanel = GetNode<Panel>("CharacterPanel");
            characterName = characterPanel.GetNode<Label>("Name");
            characterLevel = characterPanel.GetNode<Label>("CharacterLevel");
            characterClass = characterPanel.GetNode<Label>("Class");
            HP = characterPanel.GetNode<Label>("HP");
            MP = characterPanel.GetNode<Label>("MP");
            actionPanel = GetNode<Panel>("ActionPanel");
        }

        public override void _Process(float delta) {
            fpsCounter.Text = Engine.GetFramesPerSecond().ToString();

            if (isMoving && Input.IsActionJustPressed("Back")) {
                isMoving = false;
                actionPanel.Visible = true;
            }
        }

        public void OnPCInteracted(CharacterBody body) {
            actionPanel.Visible = true;
        }

        public void OnCharacterHighlighted(CharacterBody body) {
            this.body = body;
            characterName.Text = body.State.BaseCharacter.CharacterName;
            characterLevel.Text = body.State.BaseCharacter.CharacterLevel.ToString();
            characterClass.Text = body.State.BaseCharacter.CurrentJob.Name;
            HP.Text = body.State.CurrentHealth + "/" + body.State.GetStatTotal(Stat.Health);
            MP.Text = body.State.CurrentMana + "/" + body.State.GetStatTotal(Stat.Mana);
            characterPanel.Visible = true;
        }

        public void OnCharacterDehighlighted() {
            characterPanel.Visible = false;
        }

        #region ActionPanel Buttons
        public void OnAttackButtonPressed() {
            test.Text = "Attack button pressed";
        }

        public void OnActionButtonPressed() {
            test.Text = "Action button pressed";
        }

        public void OnMoveButtonPressed() {
            actionPanel.Visible = false;
            isMoving = true;
            EmitSignal(nameof(MoveButtonPressed), body);
        }

        public void OnInfoButtonPressed() {
            test.Text = "Info button pressed";
        }

        public void OnExitButtonPressed() {
            if (actionPanel.Visible) {
                actionPanel.Visible = false;
                EmitSignal(nameof(OnExit));
                test.Text = "";
            }
        }
        #endregion
    }
}
