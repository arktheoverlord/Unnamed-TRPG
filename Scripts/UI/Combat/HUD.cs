using Godot;
using System;
using Scripts.Combat;
using Scripts.Combat.Nodes;

public class HUD : Node {
    #region Signals
    [Signal]
    public delegate void OnExit();
    #endregion
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
    }

    public void OnPCInteracted(CharacterBody body) {
        actionPanel.Visible = true;
    }

    public void OnCharacterHighlighted(CharacterBody body) {
        characterName.Text = body.State.BaseCharacter.Name;
        characterLevel.Text = body.State.BaseCharacter.CharacterLevel.ToString();
        characterClass.Text = body.State.BaseCharacter.CurrentJob.Name;
        HP.Text = body.State.CurrentHealth + "/" + body.State.MaxHealth;
        MP.Text = body.State.CurrentMana + "/" + body.State.MaxMana;
        characterPanel.Visible = true;
    }

    public void OnCharacterDehighlighted() {
        characterPanel.Visible = false;
    }

    #region ActionPanel Buttons
    public void OnAttackButtonPressed(){
        test.Text = "Attack button pressed";
    }

    public void OnActionButtonPressed(){
        test.Text = "Action button pressed";
    }

    public void OnMoveButtonPressed(){
        test.Text = "Move button pressed";
    }

    public void OnInfoButtonPressed(){
        test.Text = "Info button pressed";
    }

    public void OnExitButtonPressed() {
        if(actionPanel.Visible){
            actionPanel.Visible = false;
            EmitSignal(nameof(OnExit));
            test.Text = "";
        }
    }
    #endregion
}
