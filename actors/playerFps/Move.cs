using Godot;
namespace Actors.Players
{

    public partial class Move : Node
    {
        [ExportGroup("Nodes")]
        [Export] CharacterBody3D player;
        [Export] Area3D wallArea;
        [Export] UserInputs userInputs;
        StateMachine stateMachine;
        // private AnimationNodeStateMachinePlayback moveStateMachine;
        // [Export] public Node3D skin;

        [ExportGroup("Movement")]
        [Export] float speed = 8.0f;
        [Export] float jumpHeight; // 1 me gusta como se sienten estos valores
        [Export] float jumpTimeToPeak; // 0.4
        [Export] float jumpTimeToDecend; // 0.5
        [Export] float wallJumpForce = 4.0f; // 5
        [Export] float DodgeSpeed = 1f; // 1.7
        float speedModifier = 1.0f;

        // campos internos
        float coyoteTimeMax = 0.3f;
        float coyoteTimeCounter = 0f;
        float jumpBufferCounter = 0f;
        float jumpVelocity;
        float jumpGravity;
        float fallGravity;

        // wall jump
        bool canWallJump = false;

        //squash
        float _squashAndStretch = 1.0f;
        float SquashAndStretch
        {
            //NO IMPLEMENTADO
            get => _squashAndStretch;
            set
            {
                _squashAndStretch = value;
                // float negative = 1.0f + (1.0f - _squashAndStretch);
                // skin.Scale = new Vector3(negative, _squashAndStretch, negative);
            }
        }




        public override void _Ready()
        {
            // calcula etapas de jump
            jumpVelocity = 2.0f * jumpHeight / jumpTimeToPeak;
            jumpGravity = -2.0f * jumpHeight / (jumpTimeToPeak * jumpTimeToPeak);
            fallGravity = -2.0f * jumpHeight / (jumpTimeToDecend * jumpTimeToDecend);
            //wall jump signals
            wallArea.AreaEntered += OnWallAreaEntered;
            wallArea.AreaExited += OnWallAreaExited;
            //referencias a nodos
            stateMachine = GetParent<Node3D>().GetNode<StateMachine>("StateMachine");

        }


        public override void _PhysicsProcess(double delta)
        {
            Vector3 velocity = player.Velocity;
            MoveOnFloor(userInputs.MoveDirection, velocity, userInputs.LastMoveDirection);
            Jump(velocity, (float)delta);
            WallJump();
        }


        void WallJump()
        {
            Vector3 velocity = player.Velocity;
            if (canWallJump)
            {
                if (!player.IsOnFloor())
                {
                    if (userInputs.JumpKeyPressed)
                    {
                        velocity.Y = wallJumpForce;
                        canWallJump = false;
                    }
                }
            }
            player.Velocity = velocity;
            player.MoveAndSlide();
        }


        void MoveOnFloor(Vector3 moveDirection, Vector3 velocity, Vector3 userInputsLastMoveDirection)
        {
            if (player.IsOnFloor())
            {
                if (moveDirection.Length() > 0.2f && stateMachine.state == (int)StateMachine.STATES.moving)
                {

                    // si state == moving, los inputs de movimiento de el usuario son usados
                    // si state == dashing, significa que los inputs de usuario son ignorados
                    velocity.X = moveDirection.X * speed;
                    velocity.Z = moveDirection.Z * speed;
                    if (player.IsOnFloor())
                    {
                        // moveStateMachine.Travel("run");
                    }
                }
                else if (stateMachine.state == (int)StateMachine.STATES.dashing)

                {
                    // Movimiento cuando dodge capturo lastMoveDirection antes de Dodge en UserInputs
                    velocity.X = userInputsLastMoveDirection.X * DodgeSpeed;
                    velocity.Z = userInputsLastMoveDirection.Z * DodgeSpeed;
                }
                else
                {
                    // Cuando user no hay input
                    velocity.X = Mathf.MoveToward(player.Velocity.X, 0, speed);
                    velocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, speed);

                    if (player.IsOnFloor())
                    {
                        // NO IMPLEMENTADO: aqui va states para animacines
                        // moveStateMachine.Travel("idle");
                    }
                }
            }
            else
            {
                {
                    velocity.X = moveDirection.X * speed;
                    velocity.Z = moveDirection.Z * speed;
                }
            }
            player.Velocity = velocity;
            Vector3 rot = player.Rotation;
            rot.X = 0f;
            player.Rotation = rot;
            player.MoveAndSlide();
        }


        void Jump(Vector3 velocity, float _delta)
        {
            if (userInputs.JumpKeyPressed && (coyoteTimeCounter > 0.01f))
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


        void DoSquashAndStretch(float value, float duration = 1.0f)
        {
            //NO IMPLEMENTADO:  animacion para skin
            Tween tween = CreateTween();
            tween.TweenProperty(this, "SquashAndStretch", value, duration);
            tween.TweenProperty(this, "SquashAndStretch", 1.0f, duration * 1.8f).SetEase(Tween.EaseType.Out);
        }


        void OnWallAreaEntered(Node3D area)
        {
            canWallJump = true;
        }


        void OnWallAreaExited(Node3D area)
        {
            canWallJump = false;
        }



    }
}