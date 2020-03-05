using Godot;
using System;
using Scripts;

public class Cursor : KinematicBody {
    [Export]
    private float mouseSens = 0.01f;

    [Export]
    private float zoomSens = 0.5f;

    private bool justPressed = false;
    private bool held = false;
    private string buttonPressed = "";
    private float timeSincePressed = 0f;
    private float timeSinceLastMove = 0f;

    private Spatial cameraPivot;
    private Camera camera;

    public override void _Ready() {
        cameraPivot = GetNode<Spatial>("CameraPivot");
        camera = GetNode<Camera>("CameraPivot/Camera");
    }

    public override void _Process(float delta) {
        ProcessKeyboardInput(delta);
    }

    public override void _Input(InputEvent @event) {
        ProcessMouseInput(@event);
    }

    private void ProcessMouseInput(InputEvent @event) {
        if (@event is InputEventMouseMotion eventMouseMotion && Input.IsActionPressed("MouseLeft")) {
            Input.SetMouseMode(Input.MouseMode.Captured);
            cameraPivot.RotateY(-eventMouseMotion.Relative.x * mouseSens);
        }

        if (Input.IsActionJustReleased("MouseLeft")) {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }

        if (Input.IsActionPressed("MouseWheelUp")) {
            Vector3 target = camera.Translation + new Vector3(0, -zoomSens, -zoomSens);
            if (target.y > 3 && target.z > 3) {
                camera.TranslateObjectLocal(new Vector3(0, 0, -zoomSens));
            }
        }

        if (Input.IsActionPressed("MouseWheelDown")) {
            Vector3 target = camera.Translation + new Vector3(0, zoomSens, zoomSens);
            if (target.y <= 13 && target.z <= 13) {
                camera.TranslateObjectLocal(new Vector3(0, 0, zoomSens));
            }
        }
    }

    private void ProcessKeyboardInput(float delta) {
        if (Input.IsActionJustPressed("R")) {
            cameraPivot.Rotation = new Vector3(cameraPivot.Rotation.x, 0, cameraPivot.Rotation.z);
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

    private void ProcessMouseInput(float delta) {

    }

    private void Move(string direction) {
        switch (direction) {
            case "ui_right":
                Translate(GetDirectionFromPivotRotation(new Vector3(2, 0, 0)));
                break;
            case "ui_left":
                Translate(GetDirectionFromPivotRotation(new Vector3(-2, 0, 0)));
                break;
            case "ui_up":
                Translate(GetDirectionFromPivotRotation(new Vector3(0, 0, -2)));
                break;
            case "ui_down":
                Translate(GetDirectionFromPivotRotation(new Vector3(0, 0, 2)));
                break;
        }
    }

    private Vector3 GetDirectionFromPivotRotation(Vector3 direction) {
        float y = Mathf.Rad2Deg(cameraPivot.GetRotation().y);

        if (y >= -45 && y <= 45) {
            return direction;
        }
        else if (y >= 46 && y <= 135) {
            return new Vector3(direction.z, 0, -direction.x);
        }
        else if (y >= 136 || y <= -136) {
            return new Vector3(-direction.x, 0, -direction.z);
        }
        else {
            return new Vector3(-direction.z, 0, direction.x);
        }
    }

}
