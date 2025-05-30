using Actors.Enemies;
using Godot;

public partial class Navigation : NavigationAgent3D
{
    private BaseEnemy body;

    public override void _Ready()
    {
        body = GetParent<Node>().GetParent<BaseEnemy>();
    }


    public virtual void NavigateTo(float delta, Vector3 targetPosition, float moveSpeed)
    {
        TargetPosition = targetPosition;

        Vector3 direction;
        Vector3 velocity;

        direction = (GetNextPathPosition() - body.GlobalPosition).Normalized();
        velocity = new Vector3(direction.X, 0, direction.Z) * moveSpeed;
        body.Velocity = velocity;

        // look at direction
        Vector3 targetDirection = new(direction.X, 0, direction.Z);
        Vector2 targetVector2 = new(targetDirection.X, targetDirection.Z);
        float targetAngle = -targetVector2.Angle() + Mathf.Pi / 2;
        Vector3 rotation = body.Rotation;
        rotation.Y = Mathf.RotateToward(body.Rotation.Y, targetAngle, delta * 6f);
        body.Rotation = rotation;

        


        body.MoveAndSlide();
    }




}
