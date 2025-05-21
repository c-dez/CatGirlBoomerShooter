using Actors.Enemies;
using Godot;

public partial class RayCast3d : RayCast3D
{
    [Export] Stats playerStats;

    public override void _Ready()
    {
        // stats = GetNode<Stats>("../../Stats");
    }
    public override void _PhysicsProcess(double delta)
    {


    }
    public void RayShoot()
    {
        if (GetCollider() != null)
        {
            if (GetCollider() is BaseEnemy enemy)
            {
                enemy.Health -= playerStats.attackDamage;
                GD.Print($"{enemy.Health} to  {enemy.Name}");

            }
            else if (GetCollider() is GrappingNode grappingNode)
            {
                GD.Print("true");
            }

        }
    }
}
