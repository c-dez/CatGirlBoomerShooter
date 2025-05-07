using Actors.Enemies;
using Godot;

public partial class RayCast3d : RayCast3D
{
    private Stats stats;

    public override void _Ready()
    {
        stats = GetNode<Stats>("../../Stats");
    }
    public override void _PhysicsProcess(double delta)
    {
        
        if (GetCollider() != null)
        {
            BaseEnemy target = (BaseEnemy)GetCollider();
            if (Input.IsActionJustPressed("mb1") )
            {
                target.Health -= stats.attackDamage;
                GD.Print($"{target.Health} form {Name}");
            }


        }
    }
}
