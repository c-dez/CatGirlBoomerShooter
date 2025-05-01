using Godot;

public partial class Block : CsgBox3D
{
    private CollisionShape3D collision;

    public override void _Ready()
    {
        collision = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");
        collision.Scale = Size;
    }
}
