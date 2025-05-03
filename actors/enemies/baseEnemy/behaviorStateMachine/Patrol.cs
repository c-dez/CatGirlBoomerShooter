using Godot;
using System;

public partial class Patrol : Node
{
    [Export] public Node3D point1;
    [Export] public Node3D point2;
    public Timer waitTimer;

    public override void _Ready()
    {
        waitTimer = GetNode<Timer>("waitTimer");
        // waitTimer.Timeout += OnWaitTimeout;
        waitTimer.Timeout += OnWaitTimeout;

    }

    private void OnWaitTimeout()
    {
        // GD.Print("time");
    }

   
}
