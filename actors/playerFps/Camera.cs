using Godot;

public partial class Camera : Camera3D
{
    [Export] private float mouseSensitivity = 0.3f / 3;
    private Vector2 cameraInputDirection = Vector2.Zero;
    [Export] private UserInputs userInputs;


    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            cameraInputDirection = mouseMotion.Relative * mouseSensitivity;
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        CameraRotation((float)delta);
        SkinRotation((float)delta);
    }

    private void CameraRotation(float delta)
    {
        Vector3 rot = Rotation;
        rot.X -= cameraInputDirection.Y * delta;
        rot.Y -= cameraInputDirection.X * delta;
        rot.X = Mathf.Clamp(rot.X, Mathf.DegToRad(-45), Mathf.DegToRad(30));
        Rotation = rot;
        cameraInputDirection = Vector2.Zero;
    }

    private void SkinRotation(float delta)
    {
        Vector3 moveDirection = userInputs.moveDirection;
        if (moveDirection.Length() < 0.2f)
        {
        }
        else
        {
            moveDirection.Y = 0;
        }
    }


}
