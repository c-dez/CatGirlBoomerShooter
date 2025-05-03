using System.Collections.Generic;
using Godot;

public partial class Patrol : Node
{
    public Timer waitTimer;


    [Export] NodePath[] patrolPointsPaths;
    public List<PatrolPoint1> patrolPointsNodes = new();

    public override void _Ready()
    {
        waitTimer = GetNode<Timer>("waitTimer");
        waitTimer.Timeout += OnWaitTimeout;

        foreach (var path in patrolPointsPaths)
        {
            //patrolPointPaths obtiene el path de cada elemento, y ese path lo agrego a var node, y ese node lo anado a lista patrolPointNodes
            PatrolPoint1 node = GetNode<PatrolPoint1>(path);
            if (node != null)
            {
                patrolPointsNodes.Add(node);
            }
        }
    }


    public override void _PhysicsProcess(double delta)
    {

    }

    private void OnWaitTimeout()
    {

    }



}
