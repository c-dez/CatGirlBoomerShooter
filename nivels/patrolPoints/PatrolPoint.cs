using Godot;

public partial class PatrolPoint : Node3D
{
    public MeshInstance3D mesh;
    public Area3D area;

    public override void _Ready()
    {
        mesh = GetNode<MeshInstance3D>("MeshInstance3D");
        area = GetNode<Area3D>("Area3D");

    }


}