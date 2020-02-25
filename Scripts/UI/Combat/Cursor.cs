using Godot;
using System;
using Scripts;

public class Cursor : Area2D {
    private bool justPressed = false;
    private bool held = false;
    private string buttonPressed = "";
    private float timeSincePressed = 0f;
    private float timeSinceLastMove = 0f;

    public override void _Ready() {
    }

    public override void _Process(float delta) {
        if (Input.IsActionPressed("R")) {
            Console.WriteLine(justPressed);
            Console.WriteLine(held);
            Console.WriteLine(buttonPressed);
            Console.WriteLine(timeSincePressed);
        }

        if (!justPressed && !held) {
            if (Input.IsActionPressed(Constants.Right)) {
                Move(Constants.Right);
                justPressed = true;
                buttonPressed = Constants.Right;
            }
            if (Input.IsActionPressed(Constants.Left)) {
                Move(Constants.Left);
                justPressed = true;
                buttonPressed = Constants.Left;
            }
            if (Input.IsActionPressed(Constants.Up)) {
                Move(Constants.Up);
                justPressed = true;
                buttonPressed = Constants.Up;
            }
            if (Input.IsActionPressed(Constants.Down)) {
                Move(Constants.Down);
                justPressed = true;
                buttonPressed = Constants.Down;
            }
        }

        if (justPressed && Input.IsActionPressed(buttonPressed)) {
            timeSincePressed += delta;
            if (timeSincePressed >= 20f / 60f) {
                held = true;
                justPressed = false;
                timeSincePressed = 0f;
            }
        }
        else {
            justPressed = false;
            timeSincePressed = 0f;
        }

        if (held) {
            if (Input.IsActionPressed(buttonPressed)) {
                timeSinceLastMove += delta;
                if (timeSinceLastMove >= 5f / 60f) {
                    Move(buttonPressed);
                    timeSinceLastMove = 0f;
                }
            }
            else {
                held = false;
                buttonPressed = "";
                timeSinceLastMove = 0f;
            }
        }
    }

    private void Move(string direction) {
        switch (direction) {
            case "ui_right":
                Position = new Vector2(Position.x + 32, Position.y);
                break;
            case "ui_left":
                Position = new Vector2(Position.x - 32, Position.y);
                break;
            case "ui_up":
                Position = new Vector2(Position.x, Position.y - 32);
                break;
            case "ui_down":
                Position = new Vector2(Position.x, Position.y + 32);
                break;
        }
    }
}
