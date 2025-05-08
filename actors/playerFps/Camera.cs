using Godot;
namespace Actors.Players
{
    public partial class Camera : Node3D
    {
        [Export] private UserInputs userInputs;
        [Export] private float mouseSensitivity = 0.3f / 3;
        [Export] private float clampUp = 50f;
        [Export] private float clampDown = -80f;
        private Vector2 cameraInputDirection = Vector2.Zero;


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
            SkinRotation();
        }


        private void CameraRotation(float delta)
        {
            Vector3 rot = Rotation;
            rot.X -= cameraInputDirection.Y * delta;
            rot.Y -= cameraInputDirection.X * delta;
            rot.X = Mathf.Clamp(rot.X, Mathf.DegToRad(clampDown), Mathf.DegToRad(clampUp));
            Rotation = rot;
            cameraInputDirection = Vector2.Zero;
        }

        private void SkinRotation()
        {
            Vector3 moveDirection = userInputs.MoveDirection;
            if (moveDirection.Length() < 0.2f)
            {
            }
            else
            {
                moveDirection.Y = 0;
            }
        }


    }
}