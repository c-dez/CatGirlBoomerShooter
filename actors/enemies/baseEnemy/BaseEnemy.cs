using Godot;

namespace Actors.Enemies
{
    public partial class BaseEnemy : CharacterBody3D
    {

        public CharacterBody3D player;

        [ExportGroup("Base Stats")]
        [Export] public int Health = 100;
        [Export] public float speed = 5.0f;
        [Export] public float walkSpeed = 2.0f;
        [Export] private int attackDamage = 5;

        [ExportGroup("Detection Radius")]
        [Export] private float noticeRadius = 10;
        [Export] public float attackRadius = 3;

        [ExportGroup("Nodes")]
        private Node3D skin;

        //navigation
        private Area3D areaDetectPatrolPoints;

        private int patrolPointsNodesIndex = 0;


        public override void _Ready()
        {
            player = (CharacterBody3D)GetTree().GetFirstNodeInGroup("Player");
            skin = GetNode<Node3D>("Components/Skin");
            areaDetectPatrolPoints = GetNode<Area3D>("Components/AreaDetectPatrolPoints");

        }


        public override void _PhysicsProcess(double delta)
        {
            Gravity();
            OnDead();
        }

       


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
            GD.Print($"range attack from {Name}");
        }


        public virtual void AttackMelee()
        {
            GD.Print($"attack for: {attackDamage} from {Name} ");
        }


        public virtual bool IsPlayerInNoticeRadius()
        {
            return Position.DistanceTo(player.Position) < noticeRadius;
        }


       

    }
}
