using Godot;
using Godot.Collections;
using Scripts;
using Scripts.Combat;
using Scripts.Combat.States;
using Scripts.Combat.Mapping;
using Scripts.Combat.Nodes;

public class CombatCursor : Area {
    [Export]
    private float mouseSens = 0.01f;

    [Export]
    private float zoomSens = 1f;

    [Signal]
    public delegate void PCInteracted(CharacterBody body);

    [Signal]
    public delegate void NPCInteracted(CharacterBody body);

    [Signal]
    public delegate void CreateDebugNPC();

    [Signal]
    public delegate void OnCharacterHighlighted(CharacterBody body);

    [Signal]
    public delegate void OnCharacterDehighlighted();

    private bool justPressed = false;
    private bool held = false;
    private string buttonPressed = "";
    private float timeSincePressed = 0f;
    private float timeSinceLastMove = 0f;
    private bool cursorLocked = false;

    private Spatial cameraPivot;
    private Camera camera;
    private KinematicBody overlapingEntity = null;
    private PhysicsDirectSpaceState spaceState;

    public const string CameraPivot = "CameraPivot";
    public const string CameraPath = "CameraPivot/Camera";
    public const float MoveDistence = 1f;
    public const string Up = "MovementUp";
    public const string Down = "MovementDown";
    public const string Left = "MovementLeft";
    public const string Right = "MovementRight";
    public const string ZoomIn = "ZoomIn";
    public const string ZoomOut = "ZoomOut";
    public const string Pan = "PanCamera";
    public const string Interact = "Interact";
    public const string Debug = "Debug";

    public override void _Ready() {
        cameraPivot = GetNode<Spatial>(CameraPivot);
        camera = GetNode<Camera>(CameraPath);
        camera.LookAt(Translation, Vector3.Up);
        spaceState = GetWorld().DirectSpaceState;
    }

    public override void _Process(float delta) {
        if (!cursorLocked) {
            ProcessCursorMovement(delta);
            ProcessInteraction();
        }

        if (Input.IsActionJustPressed(Debug)) {
            EmitSignal(nameof(CreateDebugNPC));
        }
    }

    public override void _Input(InputEvent @event) {
        if (!cursorLocked) {
            ProcessCameraMovement(@event);
        }
    }

    private void ProcessCameraMovement(InputEvent @event) {
        if (@event is InputEventMouseMotion eventMouseMotion && Input.IsActionPressed(Pan)) {
            Input.SetMouseMode(Input.MouseMode.Captured);
            cameraPivot.RotateY(-eventMouseMotion.Relative.x * mouseSens);
        }

        if (Input.IsActionJustReleased(Pan)) {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }

        if (Input.IsActionPressed(ZoomIn)) {
            Vector3 target = camera.Translation + new Vector3(0, -zoomSens, -zoomSens);
            if (target.y > 3) {
                camera.Translation = target;
                camera.LookAt(Translation, Vector3.Up);
            }
        }

        if (Input.IsActionPressed(ZoomOut)) {
            Vector3 target = camera.Translation + new Vector3(0, zoomSens, zoomSens);
            if (target.y <= 26) {
                camera.Translation = target;
                camera.LookAt(Translation, Vector3.Up);
            }
        }
    }

    private void ProcessCursorMovement(float delta) {
        if (!justPressed && !held) {
            if (Input.IsActionPressed(Right)) {
                Move(Right);
                justPressed = true;
                buttonPressed = Right;
            }
            if (Input.IsActionPressed(Left)) {
                Move(Left);
                justPressed = true;
                buttonPressed = Left;
            }
            if (Input.IsActionPressed(Up)) {
                Move(Up);
                justPressed = true;
                buttonPressed = Up;
            }
            if (Input.IsActionPressed(Down)) {
                Move(Down);
                justPressed = true;
                buttonPressed = Down;
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
            case Right:
                var offset = GetDirectionFromPivotRotation(new Vector3(MoveDistence, 0, 0));
                var target = offset + Translation + new Vector3(0, 0.5F, 0);
                if (IsTargetWithinMap(target)) {
                    Translation += offset + GetYOffset(target);
                }
                break;
            case Left:
                offset = GetDirectionFromPivotRotation(new Vector3(-MoveDistence, 0, 0));
                target = offset + Translation + new Vector3(0, 0.5F, 0);
                if (IsTargetWithinMap(target)) {
                    Translation += offset + GetYOffset(target);
                }
                break;
            case Up:
                offset = GetDirectionFromPivotRotation(new Vector3(0, 0, -MoveDistence));
                target = offset + Translation + new Vector3(0, 0.5F, 0);
                if (IsTargetWithinMap(target)) {
                    Translation += offset + GetYOffset(target);
                }
                break;
            case Down:
                offset = GetDirectionFromPivotRotation(new Vector3(0, 0, MoveDistence));
                target = offset + Translation + new Vector3(0, 0.5F, 0);
                if (IsTargetWithinMap(target)) {
                    Translation += offset + GetYOffset(target);
                }
                break;
        }
    }

    private bool IsTargetWithinMap(Vector3 target) {
        var cast = spaceState.IntersectRay(Translation + new Vector3(0, 0.5F, 0), target, new Array() { this }, collideWithAreas: true);
        if (cast == null || cast.Count == 0) {
            cast = spaceState.IntersectRay(target, target - new Vector3(0, 100, 0), new Array() { this }, collideWithAreas: true);
            if (cast != null && cast.Count > 0) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }

    }

    private Vector3 GetYOffset(Vector3 target) {
        var cast = spaceState.IntersectRay(Translation + new Vector3(0, 0.5F, 0), target, new Array() { this }, collideWithAreas: true);
        if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
            var y = ((Tile)cast["collider"]).GetHighestTile();
            return new Vector3(0, y, 0);
        }
        else {
            cast = spaceState.IntersectRay(target + new Vector3(0, 0.5F, 0), target - new Vector3(0, 100, 0), new Array() { this }, collideWithAreas: true, collideWithBodies: false);
            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                int y = (int)(Translation.y - ((Tile)cast["collider"]).Translation.y);
                return new Vector3(0, -y, 0);
            }
        }
        return new Vector3();
    }

    private void ProcessInteraction() {
        if (Input.IsActionJustPressed(Interact)) {
            if (overlapingEntity != null) {
                if (overlapingEntity.GetType() == typeof(CharacterBody)) {
                    if (GetOverlapingEntityTeam() == CharacterState.Type.PC) {
                        if (Input.GetMouseMode() == Input.MouseMode.Captured) {
                            Input.SetMouseMode(Input.MouseMode.Visible);
                        }
                        cursorLocked = true;
                        EmitSignal(nameof(PCInteracted), overlapingEntity);
                    }
                }
            }
            else {

            }
        }
    }

    private CharacterState.Type GetOverlapingEntityTeam() {
        //return ((CharacterBody)overlapingEntity).State.CharacterTeam;
        return CharacterState.Type.PC;
    }

    private void OnBodyEntered(Node body) {
        if (body.GetType() == typeof(CharacterBody)) {
            overlapingEntity = (KinematicBody)body;
            if(body != null){
                EmitSignal(nameof(OnCharacterHighlighted), (KinematicBody)body);
            }
        }
    }

    private void OnBodyExited(Node body) {
        overlapingEntity = null;
        EmitSignal(nameof(OnCharacterDehighlighted));
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

    #region GUI Interaction Stuff
    private void OnExitButtonPressed(){
        cursorLocked = false;
    }
    #endregion
}
