using Godot;
using System;
using Scripts.Combat;

public class HUD : Node {
    private Label test;
    private Label fpsCounter;

    public override void _Ready(){
        test = GetNode<Label>("Test");
        fpsCounter = GetNode<Label>("FPSCounter");
    }

    public override void _Process(float delta){
        fpsCounter.Text = Engine.GetFramesPerSecond().ToString();
    }

    public void OnPCInteracted(CharacterBody body){
        test.SetVisible(true);
    }
}
