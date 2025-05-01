using Godot;
namespace Actors.Players
{
    public partial class UserInputs : Node
    {
        [Export] private Camera3D camera;
        private Timer jumpTimer;
        public Vector3 moveDirection = Vector3.Zero;
        public bool jumpPressed = false;


        public override void _Ready()
        {
            jumpTimer = GetNode<Timer>("JumpTimer");
        }
        public override void _Process(double delta)
        {
            GetMoveDirection();
            JumpBuffer();
        }

        
        private void GetMoveDirection()
        {
            Vector2 rawInput = Input.GetVector("left", "right", "forward", "backwards");
            Vector3 forward = camera.GlobalBasis.Z;
            forward.Y = 0;
            Vector3 right = camera.GlobalBasis.X;
            right.Y = 0;
            moveDirection = (forward * rawInput.Y + right * rawInput.X).Normalized();
        }

        private void JumpBuffer()
        {
            if (Input.IsActionJustPressed("space"))
            {
                jumpTimer.Start();
            }
            jumpPressed = jumpTimer.TimeLeft > 0 ;
        }
        
    }
    
}