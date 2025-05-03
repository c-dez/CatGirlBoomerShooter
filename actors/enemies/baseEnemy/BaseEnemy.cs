using Godot;

namespace Actors.Enemies
{
    public partial class BaseEnemy : CharacterBody3D
    {

        public CharacterBody3D player;

        [ExportGroup("Base Stats")]
        [Export] public int Health = 100;
        [Export] private float speed = 5.0f;
        [Export]public float walkSpeed = 2.0f;
        [Export] private int attackDamage = 5;

        [ExportGroup("Detection Radius")]
        [Export] private float noticeRadius = 10;
        [Export] public float attackRadius = 3;

        [ExportGroup("Nodes")]
        // [Export] private Node3D skin;
        private Node3D skin;
        // [Export] private AnimationTree animationTree;

        //animation
        // private AnimationNodeStateMachinePlayback moveStateMachine;

        private NavigationAgent3d nav;

        private BehaviorStateMachine behaviorStateMachide;

        public override void _Ready()
        {
            player = (CharacterBody3D)GetTree().GetFirstNodeInGroup("Player");
            // moveStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/MoveStateMachine/playback");
            skin = GetNode<Node3D>("Components/Skin");
            nav = GetNode<NavigationAgent3d>("Components/NavigationAgent3D");
            // behaviorStateMachide = GetNode<BehaviorStateMachine>("Components/BehaviorStateMachine");
        }


        public override void _PhysicsProcess(double delta)
        {
            Gravity();
            // MoveToPlayer((float)delta);
            // NavigateToTarget((float)delta);
            OnDead();

            nav.PatrolTo((float)delta);
        }


        private void NavigateToTarget(float delta)
        {
            if (IsPlayerInNoticeRadius())
            {
                // 
                Vector3 direction;
                Vector3 velocity;
                nav.TargetPosition = behaviorStateMachide.patrol.point2.GlobalPosition;
                // nav.TargetPosition = player.GlobalPosition;
                direction = (nav.GetNextPathPosition() - GlobalPosition).Normalized();
                velocity = new Vector3(direction.X, 0, direction.Z) * speed;
                Velocity = velocity;
                if (Position.DistanceTo(player.Position) < attackRadius)
                {
                    Velocity = Vector3.Zero;
                }
                // look at target
                Vector3 targetDirection = (player.Position - Position).Normalized();
                Vector2 targetVector2 = new(targetDirection.X, targetDirection.Z);
                float targetAngle = -targetVector2.Angle() + Mathf.Pi / 2;
                Vector3 rotation = Rotation;
                rotation.Y = Mathf.RotateToward(Rotation.Y, targetAngle, delta * 6.0f);
                Rotation = rotation;
            }
            else
            {
                Velocity = Vector3.Zero;
            }
            MoveAndSlide();
        }


        // public virtual void MoveToPlayer(float delta)
        // {
        //     // en este caso si puedo modificar Velocity, dejo comentarios de la forma "vieja" como recordatorio
        //     // Vector3 velocity = Velocity;
        //     if (IsPlayerInNoticeRadius())
        //     {
        //         Vector3 targetDirection = (player.Position - Position).Normalized();
        //         Vector2 TargetVector = new(targetDirection.X, targetDirection.Z);
        //         float targetAngle = -TargetVector.Angle() + Mathf.Pi / 2;
        //         Vector3 rotation = Rotation;
        //         rotation.Y = Mathf.RotateToward(Rotation.Y, targetAngle, delta * 6.0f);
        //         Rotation = rotation;

        //         if (Position.DistanceTo(player.Position) > attackRadius)
        //         {
        //             // velocity = new Vector3(TargetVector.X, 0, TargetVector.Y) * speed;
        //             Velocity = new Vector3(TargetVector.X, 0, TargetVector.Y) * speed;

        //             //animation
        //             // moveStateMachine.Travel("walk");
        //         }
        //         else
        //         {
        //             // velocity = Vector3.Zero;
        //             Velocity = Vector3.Zero;
        //             //animation
        //             // moveStateMachine.Travel("idle");

        //         }
        //         // Velocity = velocity;
        //         MoveAndSlide();
        //     }
        //     else
        //     {
        //         //animation
        //         // moveStateMachine.Travel("idle");

        //     }
        // }


        private void Gravity()
        {
            if (!IsOnFloor())
            {
                Vector3 velocity = Velocity;
                velocity.Y -= 10f;
                Velocity = velocity;
                MoveAndSlide();
            }
        }


        public void OnDead()
        {
            if (Health <= 0)
            {
                GD.Print($"{Name} is dead");
                QueueFree();
            }
        }


        public virtual void RangeAttack()
        {
            GD.Print("range attack");
        }


        public virtual void AttackMelee()
        {
            GD.Print($"attack for: {attackDamage} ");
        }


        public virtual bool IsPlayerInNoticeRadius()
        {
            return Position.DistanceTo(player.Position) < noticeRadius;
        }
    }
}
