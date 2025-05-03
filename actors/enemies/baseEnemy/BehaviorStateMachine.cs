using Godot;
using System;
namespace Actors.Enemies
{



    public partial class BehaviorStateMachine : Node
    {
        public enum Behavior_State
        {
            idle,
            patroling,
            investigate,
            chase

        }

        public int state;
        // [ExportGroup("Behavior Nodes")]
        // [Export] private Node idle;
        // [Export] private Node investigate;
        // [Export] private Node chase;


        public override void _PhysicsProcess(double delta)
        {
            // GD.Print(state);
            // if (Input.IsActionJustPressed("e_key"))
            // {
            //     if (state == 1)
            //     {
            //         state = 0;
            //     }
            //     else{
            //         state = 1;
            //     }
            // }

            if (Input.IsActionJustPressed("q_key"))
            {
                state = 0;
            }
        }

    }

}