using Godot;
using Actors.Enemies;

public partial class PatrolPoint1 : Node3D
{
    public MeshInstance3D mesh;
    public Area3D area;

    public override void _Ready()
    {
        mesh = GetNode<MeshInstance3D>("MeshInstance3D");
        area = GetNode<Area3D>("Area3D");

    }


}