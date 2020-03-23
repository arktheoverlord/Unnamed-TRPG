using Godot;
using System.Collections.Generic;
using Scripts.Combat.States;
using Scripts.Combat.Nodes;

public class HUD : Node {
    //Debug Stuff
    private Label test;
    private Label fpsCounter;

    [Export]
    public PackedScene BlueAreaHighlight;

    private Panel characterPanel;
    private Label characterName;
    private Label characterLevel;
    private Label characterClass;
    private Label HP;
    private Label MP;

    private Panel actionPanel;

    private CharacterState state;

    #region Signals
    [Signal]
    public delegate void OnExit();

    [Signal]
    public delegate void MoveButtonPressed(int id, Vector3 center, HUD hud);

    [Signal]
    public delegate void OnCharacterHighlighted(int id, HUD hud);
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
    }

    public void OnPCInteracted(CharacterBody body) {
        actionPanel.Visible = true;
    }

    public void CharacterHighlighted(CharacterBody body){
        EmitSignal(nameof(OnCharacterHighlighted), body.CharacterID, this);
    }

    public void ShowCharacterPanel(CharacterState state) {
        this.state = state;
        characterName.Text = state.BaseCharacter.CharacterName;
        characterLevel.Text = state.BaseCharacter.CharacterLevel.ToString();
        characterClass.Text = state.BaseCharacter.CurrentJob.Name;
        HP.Text = state.CurrentHealth + "/" + state.MaxHealth;
        MP.Text = state.CurrentMana + "/" + state.MaxMana;
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
        EmitSignal(nameof(MoveButtonPressed), state.ID, state.Position, this);
    }

    public void DisplayMovementArea(List<Vector3> area){
        foreach(var vector in area){
            var instance = BlueAreaHighlight.Instance();
            ((Sprite3D) instance).Translation = vector;
            GetNode<Spatial>("AreaHighlight").AddChild(instance);
        }
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
