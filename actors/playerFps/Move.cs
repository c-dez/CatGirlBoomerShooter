using Godot;
// using Players;
using static Godot.GD;
namespace Actors.Players
{

    public partial class Move : Node
    {
        [ExportGroup("Nodes")]
        [Export] private CharacterBody3D player;
        // [Export] public Node3D skin;
        [Export] private Area3D wallArea;
        [Export] private UserInputs userInputs;

        [ExportGroup("Movement")]
        // private AnimationNodeStateMachinePlayback moveStateMachine;
        private float speedModifier = 1.0f;
        private Vector3 lastMovementDirection = Vector3.Back;
        private float rotationSpeed = 12.0f;
        [Export] public float speed = 8.0f;

        //jump
        [Export] float jumpHeight; //1
        [Export] float jumpTimeToPeak; //0.4
        [Export] float jumpTimeToDecend; //0.5
        [Export] private float wallJumpForce = 4.0f;

        float coyoteTimeMax = 0.3f;
        float jumpBufferMax = 0.5f;
        float coyoteTimeCounter = 0f;
        float jumpBufferCounter = 0f;
        float jumpVelocity;
        float jumpGravity;
        float fallGravity;

        // nombre muy similar a lastMovementDirection cambiar por claridad
        Vector3 lastMoveDirectionOnFloor = Vector3.Zero;

        //squash
        private float _squashAndStretch = 1.0f;
        private float SquashAndStretch
        {
            get => _squashAndStretch;
            set
            {
                _squashAndStretch = value;
                // float negative = 1.0f + (1.0f - _squashAndStretch);
                // skin.Scale = new Vector3(negative, _squashAndStretch, negative);
            }
        }

        // wall jump
        private bool canWallJump = false;


        public override void _Ready()
        {
            // calcula etapas de jump
            jumpVelocity = (2.0f * jumpHeight) / jumpTimeToPeak;
            jumpGravity = (-2.0f * jumpHeight) / (jumpTimeToPeak * jumpTimeToPeak);
            fallGravity = (-2.0f * jumpHeight) / (jumpTimeToDecend * jumpTimeToDecend);
            //wall jump signals
            wallArea.AreaEntered += OnWallAreaEntered;
            wallArea.AreaExited += OnWallAreaExited;


        }

        public override void _PhysicsProcess(double delta)
        {
            Vector3 velocity = player.Velocity;
            MoveOnFloor(userInputs.moveDirection, velocity);
            LastMoveDirection(userInputs.moveDirection);
            Jump(velocity, (float)delta);
            WallJump();


        }


        private void WallJump()
        {
            Vector3 velocity = player.Velocity;
            if (canWallJump)
            {
                if (!player.IsOnFloor())
                {
                    if (userInputs.jumpPressed)
                    {
                        velocity.Y = wallJumpForce;
                        canWallJump = false;
                    }
                }
            }
            player.Velocity = velocity;
            player.MoveAndSlide();
        }


        private void OnWallAreaEntered(Node3D area)
        {
            canWallJump = true;
        }


        private void OnWallAreaExited(Node3D area)
        {
            canWallJump = false;
        }


        private void MoveOnFloor(Vector3 moveDirection, Vector3 velocity)
        {
            float _speed = speed * speedModifier;
            if (player.IsOnFloor())
            {
                if (moveDirection.Length() > 0.2f)
                {
                    velocity.X = moveDirection.X * _speed;
                    velocity.Z = moveDirection.Z * _speed;
                    if (player.IsOnFloor())
                    {
                        // moveStateMachine.Travel("run");
                    }
                }
                else
                {
                    velocity.X = Mathf.MoveToward(player.Velocity.X, 0, _speed);
                    velocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, _speed);

                    if (player.IsOnFloor())
                    {
                        // moveStateMachine.Travel("idle");
                    }
                }
            }
            else
            {
                {
                    velocity.X = moveDirection.X * _speed;
                    velocity.Z = moveDirection.Z * _speed;
                }
            }
            player.Velocity = velocity;
            Vector3 rot = player.Rotation;
            rot.X = 0f;
            player.Rotation = rot;
            player.MoveAndSlide();
        }


        private void LastMoveDirection(Vector3 moveDirection)
        {
            if (player.IsOnFloor())
            {
                lastMoveDirectionOnFloor = moveDirection;
            }
        }


        private void Jump(Vector3 velocity, float _delta)
        {
            if (userInputs.jumpPressed && (coyoteTimeCounter > 0.01f ))
            {
                velocity.Y = jumpVelocity;
                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
            }
            //on air
            if (!player.IsOnFloor())
            {
                // moveStateMachine.Travel("falling");
                if (velocity.Y < 0.0f)
                {
                    velocity.Y += fallGravity * _delta;
                }
                else
                {
                    velocity.Y += jumpGravity * _delta;
                }
            }
            // coyote time
            coyoteTimeCounter = player.IsOnFloor() ? coyoteTimeMax : coyoteTimeCounter - _delta;
            player.Velocity = velocity;
            player.MoveAndSlide();
        }


        private void DoSquashAndStretch(float value, float duration = 1.0f)
        {
            Tween tween = CreateTween();
            tween.TweenProperty(this, "SquashAndStretch", value, duration);
            tween.TweenProperty(this, "SquashAndStretch", 1.0f, duration * 1.8f).SetEase(Tween.EaseType.Out);
        }


    }
}