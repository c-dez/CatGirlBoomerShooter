using Godot;
namespace Actors.Players
{
    public partial class UserInputs : Node
    {
        [Export] Node3D Camera;
        [Export] float IsDogingTime = 0.25f;
        [Export] float DodgeKeyBufferTime = 0.3f;
        CharacterBody3D Player;
        Timer JumpKeyBufferTimerNode;
        Timer DodgeKeyTimerBufferNode;
        Timer IsDogingTimerNode;
        public Vector3 MoveDirection = Vector3.Zero;
        public Vector3 LastMoveDirection = Vector3.Zero;
        public bool JumpKeyPressed = false;
        bool DodgeKeyPressed = false;


        // debug
        int frames = 0;


        public override void _Ready()
        {
            JumpKeyBufferTimerNode = GetNode<Timer>("JumpKeyBufferTimer");
            DodgeKeyTimerBufferNode = GetNode<Timer>("DodgeKeyBufferTimer");
            IsDogingTimerNode = GetNode<Timer>("IsDogingTimer");
            Player = GetNode<CharacterBody3D>("../..");
        }


        public override void _Process(double delta)
        {
            GetMoveDirection();
            JumpKeyBuffer();
            DodgeKeyBuffer();
            DoDodge();

            //debug
            // if (dodgeTimer.TimeLeft > 0)
            // {
            //     frames++;

            // }
            // if (dodgeTimer.TimeLeft == 0 && frames > 0)
            // {
            //     GD.Print($"Dodge frames {frames}");
            //     frames = 0;
            // }
        }


        void DoDodge()
        // Deberia de estar en este script de UserInputs?
        {
            // hace dodge cuando esta en el suelo bloqueando la  direccion de el usuario con la ultima direccion antes de dodge, hasta que dodge termina
            if (DodgeKeyPressed && LastMoveDirection == Vector3.Zero && Player.IsOnFloor())
            {
                IsDogingTimerNode.Start(IsDogingTime);
                LastMoveDirection = MoveDirection;
            }
            if (IsDogingTimerNode.TimeLeft == 0)
            {
                LastMoveDirection = Vector3.Zero;
            }
        }


        void GetMoveDirection()
        {
            Vector2 rawInput = Input.GetVector("left", "right", "forward", "backwards");
            Vector3 forward = Camera.GlobalBasis.Z;
            forward.Y = 0;
            Vector3 right = Camera.GlobalBasis.X;
            right.Y = 0;
            MoveDirection = (forward * rawInput.Y + right * rawInput.X).Normalized();
        }


        void JumpKeyBuffer()
        {
            if (Input.IsActionJustPressed("space"))
            {
                JumpKeyBufferTimerNode.Start();
            }
            JumpKeyPressed = JumpKeyBufferTimerNode.TimeLeft > 0f;
        }


        void DodgeKeyBuffer()
        {
            // ATENCION Cambiado "shift" a "mb1" para testing
            // ATENCION DodgeKeyBufferTime tiene que ser menor que IsDogingTime si no causa errores de logica y se dispara 2 veces dodge al seguir introduciendo input de movimiento
            if (Input.IsActionJustPressed("mb2") )
            {
                DodgeKeyTimerBufferNode.Start(DodgeKeyBufferTime);
            }
            DodgeKeyPressed = DodgeKeyTimerBufferNode.TimeLeft > 0f;
        }

    }
}

