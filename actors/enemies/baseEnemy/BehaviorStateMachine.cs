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
        public NavigationAgent3D nav;
        public Area3D visionCone;

        public override void _Ready()
        {
            nav = GetParent<Node3D>().GetNode<NavigationAgent3D>("NavigationAgent3D");
            visionCone = GetParent<Node3D>().GetNode<Area3D>("VisionCone");
            visionCone.BodyEntered += OnPlayerEntered;
            visionCone.BodyExited += OnPlayerExited;
        }
        


        public override void _PhysicsProcess(double delta)
        {
           
        }

        private void OnPlayerEntered(Node3D body)
        {
            if (body.IsInGroup("Player"))
            {
                GD.Print($"{Name} detected {body.Name}");
                state =(int)Behavior_State.chase;
            }
        }


        private void OnPlayerExited(Node3D body)
        {
            if (body.IsInGroup("Player"))
            {
                GD.Print($"{Name} lost {body.Name}");
                state =(int)Behavior_State.idle;
            }   
        }

    }

}