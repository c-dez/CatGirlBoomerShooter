using Actors.Players;
using Godot;
using System;

public partial class GrappingNode : StaticBody3D
{
    public Area3D Area;

    public override void _Ready()
    {
        Area = GetNode<Area3D>("Area3D");
        Area.BodyEntered += OnBodyEnteredGrappingNode;
    }
    

     void OnBodyEnteredGrappingNode(Node3D body)
        {
            //cuando Player entra en esta area cambio GrappingNodePosition a Zero para que deje de usarlo en otros scripts

            GD.Print("enter");
        }
}
