using Godot;
using System;
namespace Actors.Enemies
{



    public partial class BehaviorStateMachine : Node
    {
        public enum ENEMY_STATE
        {
            idle,
            patroling,
            investigate,
            chase

        }
        // [ExportGroup("Behavior Nodes")]
        // [Export] private Node idle;
        // [Export] private Node investigate;
        // [Export] private Node chase;
        public Patrol patrol;

        public override void _Ready()
        {
            patrol = GetNode<Patrol>("Patrol");
        }

    }
    
}