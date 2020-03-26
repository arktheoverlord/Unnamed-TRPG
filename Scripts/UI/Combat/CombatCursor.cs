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

    private bool justPressed = false;
    private bool held = false;
    private string buttonPressed = "";
    private float timeSincePressed = 0f;
    private float timeSinceLastMove = 0f;
    private bool menuLocked = false;
    private bool characterMovementLocked = false;

    private Spatial cameraPivot;
    private Camera camera;
    private CharacterBody overlapingEntity = null;
    private PhysicsDirectSpaceState spaceState;
    private static bool alreadyCreated = false;
    //private CharacterBody movingCharacter;
    //private bool isCharacterMoving = false;

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

    #region
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

    [Signal]
    public delegate void MovementLocationSelected(Vector3 location, CharacterBody body);
    #endregion

    public override void _Ready() {
        cameraPivot = GetNode<Spatial>(CameraPivot);
        camera = GetNode<Camera>(CameraPath);
        camera.LookAt(Translation, Vector3.Up);
        spaceState = GetWorld().DirectSpaceState;
    }

    public override void _Process(float delta) {
        if (!menuLocked) {
            ProcessCursorMovement(delta);
            ProcessInteraction();
        }

        if (Input.IsActionJustPressed("Exit")) {
            GetTree().Quit();
        }

        if (Input.IsActionJustPressed("Back")) {
            characterMovementLocked = false;
            menuLocked = true;
            Translation = overlapingEntity.Translation - new Vector3(0, 0.51f, 0);
        }
    }

    public override void _Input(InputEvent @event) {
        if (!menuLocked) {
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
        //Check if the player pressed a movement key on this frame.
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

        //Check if the player is still holding the same movement input
        if (justPressed && Input.IsActionPressed(buttonPressed)) {
            timeSincePressed += delta;
            //Check if the player has been holding the input for at least 1/3 of a second
            if (timeSincePressed >= 20f / 60f) {
                held = true;
                justPressed = false;
                timeSincePressed = 0f;
            }
        }
        //If the player isn't, reset everything
        else {
            justPressed = false;
            timeSincePressed = 0f;
        }

        //Check if the player has been holding the input for at least 1/3 of a second
        if (held) {
            //Check if the player is still holding the same movement input
            if (Input.IsActionPressed(buttonPressed)) {
                timeSinceLastMove += delta;
                if (timeSinceLastMove >= 5f / 60f) {
                    Move(buttonPressed);
                    timeSinceLastMove = 0f;
                }
            }
            //If the player isn't, reset everything
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
        //Raycast from the center of the cursor to the target
        var cast = spaceState.IntersectRay(Translation + new Vector3(0, 0.5F, 0), target, new Array() { this }, collideWithAreas: true);
        //Did the raycast hit a collider?
        if (cast == null || cast.Count == 0) {
            //The raycast didn't hit a collider, so raycast from the center of the targt to a point 100 units below the target
            cast = spaceState.IntersectRay(target, target - new Vector3(0, 100, 0), new Array() { this }, collideWithAreas: true);
            if (cast != null && cast.Count > 0) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            //The raycast hit a collider, so the collider must be in the map
            return true;
        }

    }

    private Vector3 GetYOffset(Vector3 target) {
        //Raycast from the center of the cursor to the target
        var cast = spaceState.IntersectRay(Translation + new Vector3(0, 0.5F, 0), target, new Array() { this }, collideWithAreas: true);
        //Did raycast hit a collider and is it a tile?
        if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
            var y = ((Tile)cast["collider"]).GetHighestTile();
            return new Vector3(0, y, 0);
        }
        else {
            //Raycast from the center of the orginal target to a point 100 units below the original target
            cast = spaceState.IntersectRay(target + new Vector3(0, 0.5F, 0), target - new Vector3(0, 100, 0), new Array() { this }, collideWithAreas: true, collideWithBodies: false);
            //Did the raycast hit a collider and is it a tile?
            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                //Find the different in height between the translation of the cursor and the translation of the tile
                int y = (int)(Translation.y - ((Tile)cast["collider"]).Translation.y);
                return new Vector3(0, -y, 0);
            }
        }
        return new Vector3();
    }

    private void ProcessInteraction() {
        if (Input.IsActionJustPressed(Interact)) {
            if (overlapingEntity != null && !characterMovementLocked) {
                if (overlapingEntity.GetType() == typeof(CharacterBody)) {
                    if (GetOverlapingEntityType() == Team.PC) {
                        if (Input.GetMouseMode() == Input.MouseMode.Captured) {
                            Input.SetMouseMode(Input.MouseMode.Visible);
                        }
                        menuLocked = true;
                        EmitSignal(nameof(PCInteracted), overlapingEntity);
                    }
                    else {

                    }
                }
            }
            else if (characterMovementLocked) {
                EmitSignal(nameof(MovementLocationSelected), Translation, overlapingEntity);
                characterMovementLocked = false;
            }
            else {

            }
        }
    }

    private void OnBodyEntered(Node body) {
        if (body.GetType() == typeof(CharacterBody)) {
            overlapingEntity = (CharacterBody)body;
            if (body != null) {
                EmitSignal(nameof(OnCharacterHighlighted), (CharacterBody)body);
            }
        }
    }

    private void OnBodyExited(Node body) {
        if (!characterMovementLocked) {
            overlapingEntity = null;
            EmitSignal(nameof(OnCharacterDehighlighted));
        }
    }

    private Team GetOverlapingEntityType() {
        return ((CharacterBody)overlapingEntity).State.CharacterTeam;
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
    public void OnExitButtonPressed() {
        menuLocked = false;
    }

    public void OnMoveButtonPressed(CharacterState state) {
        menuLocked = false;
        characterMovementLocked = true;
    }
    #endregion
}
