using System.Collections.Generic;
using Godot;
namespace Actors.Players
{
    public partial class UserInputs : Node
    {
        [Export] private Node3D camera;
        [Export] private float DodgeTimeDuration = 0.25f;
        [Export] private float ShiftBufferTime = 1f;
        private CharacterBody3D player;
        private Timer jumpTimer;
        private Timer shiftTimer;
        private Timer dodgeTimer;
        public Vector3 moveDirection = Vector3.Zero;
        public Vector3 LastMoveDirection = Vector3.Zero;
        public bool jumpPressed = false;
        public bool ShiftPressed = false;



        // debug
        private int frames = 0;


        public override void _Ready()
        {
            jumpTimer = GetNode<Timer>("JumpTimer");
            shiftTimer = GetNode<Timer>("ShiftTimer");
            player = GetNode<CharacterBody3D>("../..");
            dodgeTimer = GetNode<Timer>("DodgeTimer");
        }


        public override void _Process(double delta)
        {
            GetMoveDirection();
            JumpBuffer();
            ShiftBuffer();
            DoDodge();

            //debug
            if (dodgeTimer.TimeLeft > 0)
            {
                frames++;

            }
            if (dodgeTimer.TimeLeft == 0 && frames > 0)
            {
                GD.Print($"Dodge frames {frames}");
                frames = 0;
            }
        }


        private void DoDodge()
        {
            // hace dodge cuando esta en el suelo bloqueando la  direccion de el usuario con la ultima direccion antes de dodge, hasta que dodge termina
            if (ShiftPressed && LastMoveDirection == Vector3.Zero && player.IsOnFloor())
            {
                dodgeTimer.Start(DodgeTimeDuration);
                LastMoveDirection = moveDirection;
            }
            if (dodgeTimer.TimeLeft == 0)
            {
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

        
        private void ShiftBuffer()
        {
            if (Input.IsActionJustPressed("shift"))
            {
                shiftTimer.Start(ShiftBufferTime);
            }
            ShiftPressed = shiftTimer.TimeLeft > 0;
        }

    }
}

