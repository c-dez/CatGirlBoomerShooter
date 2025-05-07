using Godot;
namespace Actors.Players
{
    public partial class UserInputs : Node
    {
        [Export] private Node3D camera;
        [Export] private float DodgeTimeDuration = 0.25f;
        public Vector3 moveDirection = Vector3.Zero;
        public Vector3 LastMoveDirection = Vector3.Zero;
        public bool jumpPressed = false;
        public bool ShiftPressed = false;
        private Timer shiftTimer;
        private Timer jumpTimer;




        public override void _Ready()
        {
            jumpTimer = GetNode<Timer>("JumpTimer");
            shiftTimer = GetNode<Timer>("ShiftTimer");

        }


        public override void _Process(double delta)
        {
            GetMoveDirection();
            JumpBuffer();
            DodgePressed();

        }


        private void DodgePressed()
        {
            // Esta funcion se encarga de hacer buffer a tecla shift, asignar valor a lastMoveDirection
            // Y se usa en Actors.Players.Move.MoveOnFloor() para que decida  que moveDirection usar
            if (Input.IsActionJustPressed("shift") && LastMoveDirection == Vector3.Zero && !ShiftPressed)
            {
                shiftTimer.Start(DodgeTimeDuration);
                LastMoveDirection = moveDirection;
            }
            if (shiftTimer.TimeLeft > 0)
            {
                ShiftPressed = true;
            }
            else
            {
                ShiftPressed = false;
                LastMoveDirection = Vector3.Zero;
            }
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
            jumpPressed = jumpTimer.TimeLeft > 0f;
        }



    }

}

