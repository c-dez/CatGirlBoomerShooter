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
        public SpringArm3D spring;

        [Export] MeshInstance3D skin;


        public override void _Ready()
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            spring = GetNode<SpringArm3D>("SpringArm3D");
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
            rot.X = Mathf.Clamp(rot.X, Mathf.DegToRad(clampDown), Mathf.DegToRad(clampUp));
            Rotation = rot;
            cameraInputDirection = Vector2.Zero;
        }

        private void SkinRotation(float delta)
        {
            Vector3 moveDirection =userInputs.moveDirection;
            if (moveDirection.Length() < 0.2f)
            {
                skin.RotateY(cameraInputDirection.X * delta);
            }
            else
            {
                moveDirection.Y = 0;
                Vector3 targetPos = skin.GlobalTransform.Origin + moveDirection;
                skin.LookAt(targetPos, Vector3.Up);
            }
        }


    }
}