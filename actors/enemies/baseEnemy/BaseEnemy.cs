using System.Text.RegularExpressions;
using Godot;

namespace Actors.Enemies
{
    public partial class BaseEnemy : CharacterBody3D
    {

        public CharacterBody3D player;

        [ExportGroup("Base Stats")]
        [Export] public int Health = 100;
        [Export] private float speed = 5.0f;
        [Export] public float walkSpeed = 2.0f;
        [Export] private int attackDamage = 5;

        [ExportGroup("Detection Radius")]
        [Export] private float noticeRadius = 10;
        [Export] public float attackRadius = 3;

        [ExportGroup("Nodes")]
        private Node3D skin;

        //navigation
        private NavigationAgent3d nav;
        private BehaviorStateMachine behaviorStateMachine;
        private Patrol patrol;
        private Area3D areaDetectPatrolPoints;

        private int patrolPointsNodesIndex = 0;
        // private BehaviorStateMachine behaviorStateMachide;

        public override void _Ready()
        {
            player = (CharacterBody3D)GetTree().GetFirstNodeInGroup("Player");
            skin = GetNode<Node3D>("Components/Skin");
            nav = GetNode<NavigationAgent3d>("Components/NavigationAgent3D");
            behaviorStateMachine = GetNode<BehaviorStateMachine>("Components/BehaviorStateMachine");
            patrol = GetNode<Patrol>("Components/BehaviorStateMachine/Patrol");
            areaDetectPatrolPoints = GetNode<Area3D>("Components/AreaDetectPatrolPoints");
            //signal
            areaDetectPatrolPoints.AreaEntered += OnAreaEntered;
            behaviorStateMachine.state = (int)BehaviorStateMachine.Behavior_State.idle;



            Debugg();

        }


        public override void _PhysicsProcess(double delta)
        {

            Gravity();
            OnDead();
            // Mover a funcion cuando este lista
            switch (behaviorStateMachine.state)
            {
                case (int)BehaviorStateMachine.Behavior_State.idle:
                    nav.NavigateTo((float)delta, GlobalPosition, 0f);
                    break;
                case (int)BehaviorStateMachine.Behavior_State.patroling:
                    nav.NavigateTo((float)delta, patrol.patrolPointsNodes[patrolPointsNodesIndex].GlobalPosition, walkSpeed);
                    break;
                case (int) BehaviorStateMachine.Behavior_State.chase:
                    nav.NavigateTo((float)delta,player.GlobalPosition, speed);
                    break;

                default:
                    GD.PrintErr($"No State in {Name}  behaviorStateMachine switch");
                    break;
            }
        }

        public void OnAreaEntered(Node3D body)
        {
            patrolPointsNodesIndex += 1;
            if (patrolPointsNodesIndex == patrol.patrolPointsNodes.Count)
            patrolPointsNodesIndex = 0;
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


        private void Debugg()
        {

            if (patrol.patrolPointsNodes == null)
            {
                GD.PrintErr($"{Name} patrol/points are null {patrol.GetPath()}");

            }
        }

    }
}
