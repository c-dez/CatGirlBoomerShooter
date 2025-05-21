using System.Security.Cryptography.X509Certificates;
using Actors.Enemies;
using Godot;

public partial class RayCast3d : RayCast3D
{
    [Export] Stats stats;

    public override void _Ready()
    {
        // stats = GetNode<Stats>("../../Stats");
    }
    public override void _PhysicsProcess(double delta)
    {


    }
    public void RayShoot()
    {
        if (GetCollider() != null )
        {
            if (GetCollider() is BaseEnemy enemy)
            {
                BaseEnemy target = enemy;
                target.Health -= stats.attackDamage;
                GD.Print($"{target.Health} to  {target.Name}");

            }

        }
    }
}
