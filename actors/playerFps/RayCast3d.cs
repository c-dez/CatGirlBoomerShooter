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
        
        if (GetCollider() != null)
        {
            if (GetCollider() is BaseEnemy enemy && Input.IsActionJustPressed("mb1"))
            {
                BaseEnemy target = enemy;
                target.Health -= stats.attackDamage;
                GD.Print($"{target.Health} to  {target.Name}");

            }

        }
    }
}
