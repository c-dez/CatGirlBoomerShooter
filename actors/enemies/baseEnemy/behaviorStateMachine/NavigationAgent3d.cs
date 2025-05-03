using Actors.Enemies;
using Godot;
using System.Threading;

public partial class NavigationAgent3d : NavigationAgent3D
{
    private BaseEnemy body;
    private Patrol patrol;
    // private Godot.Timer waitTimer;


    public override void _Ready()
    {
        body = GetParent<Node>().GetParent<BaseEnemy>();
        patrol = GetParent<Node3D>().GetNode<Patrol>("BehaviorStateMachine/Patrol");
        // GD.Print(patrol.Name);
        // Godot.Timer waitTimer = GetNode<Godot.Timer>("WaitTimer");

        // patrol.waitTimer.Timeout += OnWaitTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {


    }

    public virtual void PatrolTo(float delta)
    {
        TargetPosition = patrol.point1.GlobalPosition;
        Vector3 direction;
        Vector3 velocity;

        direction = (GetNextPathPosition() - body.GlobalPosition).Normalized();
        velocity = new Vector3(direction.X, 0, direction.Z) * body.walkSpeed;
        body.Velocity = velocity;

        //look at direction
        Vector3 targetDirection = new(direction.X, 0, direction.Z);
        Vector2 targetVector2 = new(targetDirection.X, targetDirection.Z);
        float targetAngle = -targetVector2.Angle() + Mathf.Pi / 2;
        Vector3 rotation = body.Rotation;
        rotation.Y = Mathf.RotateToward(body.Rotation.Y, targetAngle, delta * 6f);
        body.Rotation = rotation;

        if (body.Position.DistanceTo(patrol.point1.Position) < body.attackRadius)
        {
            body.Velocity = Vector3.Zero;

            // wait
            //necesito que solo detecte cuando entra, un area funciona
            if (!patrol.waitTimer.IsStopped())
            {
                return;
            }
            patrol.waitTimer.Start();
            GD.Print(patrol.waitTimer.TimeLeft);

        }
        body.MoveAndSlide();


    }

   


}
