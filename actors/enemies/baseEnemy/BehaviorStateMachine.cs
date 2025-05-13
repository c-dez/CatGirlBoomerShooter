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
        private NavigationAgent3D nav;
        private VisionCone visionCone;


        public override void _Ready()
        {
            nav = GetParent<Node3D>().GetNode<NavigationAgent3D>("NavigationAgent3D");
            visionCone = GetParent<Node3D>().GetNode<VisionCone>("VisionCone");
        }


        public override void _PhysicsProcess(double delta)
        {
            switch (state)
            {
                case (int)Behavior_State.idle:
                    break;
                case (int)Behavior_State.patroling:
                    break;
                case (int)Behavior_State.investigate:
                    break;
                case (int)Behavior_State.chase:
                    break;
                default:
                    break;
            }
        }


        
    }
}