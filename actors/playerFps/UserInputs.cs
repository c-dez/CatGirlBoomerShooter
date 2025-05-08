using System.Collections.Generic;
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
        [Export] private float ShiftBufferTime = 0.2f;
        private Timer jumpTimer;
        

        private int frames =0;





        public override void _Ready()
        {
            jumpTimer = GetNode<Timer>("JumpTimer");
            shiftTimer = GetNode<Timer>("ShiftTimer");

        }


        public override void _Process(double delta)
        {
            GetMoveDirection();
            JumpBuffer();
            ShiftBuffer();
            // DodgePressed();

           
        }


        private void DodgePressed()
        {
            // Esta funcion se encarga de hacer buffer a tecla shift, asignar valor a lastMoveDirection
            // Y se usa en Actors.Players.Move.MoveOnFloor() para que decida  que moveDirection usar
            // if (Input.IsActionJustPressed("shift") && LastMoveDirection == Vector3.Zero && !ShiftPressed)
            // {
            //     shiftTimer.Start(DodgeTimeDuration);
            //     LastMoveDirection = moveDirection;
            // }
            // if (shiftTimer.TimeLeft > 0)
            // {
            //     ShiftPressed = true;
            // }
            // else
            // {
            //     ShiftPressed = false;
            //     LastMoveDirection = Vector3.Zero;
            // }
        }

        private void ShiftBuffer()
        {
            if (Input.IsActionJustPressed("shift") && !ShiftPressed)
            {
                shiftTimer.Start(ShiftBufferTime);
            }
            ShiftPressed = shiftTimer.TimeLeft > 0;
            //debug
            if (ShiftPressed)
            {
                frames++;

            }
            if (!ShiftPressed && frames> 0)
            {
                GD.Print($"Shift pressed frames {frames}");
                frames = 0;
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

