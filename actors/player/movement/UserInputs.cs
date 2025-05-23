using Actor.Players;
using Actors.Enemies;
using Godot;
namespace Actors.Players
{
    public partial class UserInputs : Node
    {
        public Vector3 moveDirection = Vector3.Zero;
        public Vector3 lastMoveDirection = Vector3.Zero;
        public bool JumpKeyPressed = false;
        [Export] Camera camera;
        [Export] float isDogingTime = 0.25f;
        [Export] float dodgeKeyBufferTime = 0.2f;
        [Export] CanvasLayer crossHair;
        [Export] RayCast3d shootRay;
        // Player player;
        Player player;

        Timer JumpKeyBufferTimerNode;
        Timer dodgeKeyTimerBufferNode;
        Timer isDogingTimerNode;
        bool DodgeKeyPressed = false;
        StateMachine stateMachine;


        public override void _Ready()
        {
            JumpKeyBufferTimerNode = GetNode<Timer>("JumpKeyBufferTimer");
            dodgeKeyTimerBufferNode = GetNode<Timer>("DodgeKeyBufferTimer");
            isDogingTimerNode = GetNode<Timer>("IsDogingTimer");
            player = (Player)GetTree().GetFirstNodeInGroup("Player");


            stateMachine = GetParent<Node3D>().GetNode<StateMachine>("StateMachine");
        }


        public override void _Process(double delta)
        {
            GetMoveDirection();
            JumpKeyBuffer();
            DodgeKeyBuffer();
            // Dodge();
            // Shoot();
            // ChangeBetweenFirstAndThridPersonCamera();




        }

        public void Shoot()
        {
            if (Input.IsActionJustPressed("mb1") && Input.IsActionPressed("mb2"))
            {
                if (shootRay.GetRayCollider() != null)
                {
                    if (shootRay.GetRayCollider() is BaseEnemy enemy)
                    {
                        GD.Print(enemy.Name);
                        // se le va a hacer dano a enemigo
                    }
                    else if (shootRay.GetRayCollider() is GrappingNode grappingNode)
                    {
                        GD.Print(grappingNode.Name);
                        // se va a entrar a grapping state
                        // se va a bloquear los inputs de usuario de movimiento
                        //se va a mover player de su pocision actual a la pocision de grappingNode
                        //al llegar al punto se regresa a move state y se desbloquea los inputs de el usuario
                    }
                }
            }
        }


        public void ChangeBetweenFirstAndThridPersonCamera()
        {
            // cambia el Lenght de srping arm para simular entre tercera y primera persona
            camera.spring.SpringLength = Input.IsActionPressed("mb2") ? -1f : 5f;
            crossHair.Visible = Input.IsActionPressed("mb2");
        }


        public void Dodge()
        // Deberia de estar en este script de UserInputs?
        {
            // hace dodge cuando esta en el suelo bloqueando la  direccion de el usuario con la ultima direccion antes de dodge, hasta que dodge termina
            if (DodgeKeyPressed && lastMoveDirection == Vector3.Zero && player.IsOnFloor())
            {
                isDogingTimerNode.Start(isDogingTime);
                lastMoveDirection = moveDirection;
                stateMachine.state = (int)StateMachine.STATES.dashing;
            }
            if (isDogingTimerNode.TimeLeft == 0)
            {
                lastMoveDirection = Vector3.Zero;
                stateMachine.state = (int)StateMachine.STATES.moving;

            }
        }


        void GetMoveDirection()
        {
            Vector2 rawInput = Input.GetVector("left", "right", "forward", "backwards");
            Vector3 forward = camera.GlobalBasis.Z;
            forward.Y = 0;
            Vector3 right = camera.GlobalBasis.X;
            right.Y = 0;
            moveDirection = (forward * rawInput.Y + right * rawInput.X).Normalized();
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
            // ATENCION DodgeKeyBufferTime tiene que ser menor que IsDogingTime si no causa errores de logica y se dispara 2 veces dodge al seguir introduciendo input de movimiento
            if (Input.IsActionJustPressed("shift"))
            {
                dodgeKeyTimerBufferNode.Start(dodgeKeyBufferTime);
            }
            DodgeKeyPressed = dodgeKeyTimerBufferNode.TimeLeft > 0f;
        }

    }
}

