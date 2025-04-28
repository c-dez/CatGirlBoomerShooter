using Enemies;
using Godot;

public partial class RayCast3d : RayCast3D
{
    public override void _PhysicsProcess(double delta)
    {

        if (GetCollider() != null)
        {
            BaseEnemy target = (BaseEnemy)GetCollider();
            // GD.Print(target.GetType());
            if (Input.IsActionJustPressed("mb1") )
            {
                GD.Print("as");
            }


        }
    }
}
