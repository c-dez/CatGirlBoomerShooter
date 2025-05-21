using Actors.Enemies;
using Godot;

public partial class RayCast3d : RayCast3D
{
    public CollisionObject3D GetRayCollider()
    {
        var collider = GetCollider();
        if (collider is BaseEnemy enemy)
        {
            return enemy;
        }
        else if (collider is GrappingNode grappingNode)
        {
            return grappingNode;
        }
        return null;
    }
}
