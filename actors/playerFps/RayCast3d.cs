using Enemies;
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
            // GD.Print(target.GetType());
            if (Input.IsActionJustPressed("mb1") )
            {
                // int attackDamage = attackDamage;
                target.Health -= stats.attackDamage;
                GD.Print(target.Health);
            }


        }
    }
}
