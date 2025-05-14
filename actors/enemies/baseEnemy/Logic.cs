using System;
using Godot;
namespace Actors.Enemies
{
    public partial class Logic : Node
    {
        BaseEnemy body;
        VisionCone visionCone;
        Node3D skin;
        Navigation nav;
        int state = 0;
        int[] currentAndLastState = new int[2];

         // debug
         Label stateMachineLabel;

        enum BehaviorStateMachine
        {
            idle,
            patroling,
            investigate,
            chase,
        }



        public override void _Ready()
        {
            body = GetNode<BaseEnemy>("../..");
            visionCone = GetNode<VisionCone>("../VisionCone");
            skin = GetNode<Node3D>("../Skin");
            nav = GetNode<Navigation>("../Navigation");

            //debug
            stateMachineLabel = GetNode<Label>("StateMachineLabel");
            //signals
            visionCone.BodyEntered += OnBodyEntered;
            visionCone.BodyExited += OnBodyExited;

            // visionCone.Connect("OnPlayerSightEnter", new Callable(this, nameof(OnTest))); 
            visionCone.OnPlayerSightEnter += OnPlayerOnSight;


        }

        public override void _PhysicsProcess(double delta)
        {
            StateLabel();

            if (visionCone.canSeePlayer)
            {
                // lastState = state;
                state = (int)BehaviorStateMachine.chase;
                // moverse a pos de player
            }
            GD.Print($"current {currentAndLastState[0]} last {currentAndLastState[1]}");


        }


        void OnPlayerOnSight(CharacterBody3D player)
        {
            int lastState = state;
            currentAndLastState[1] = lastState;
            state = (int)BehaviorStateMachine.chase;
            int newState = state;
            currentAndLastState[0] = newState;

        }
        void OnBodyEntered(Node3D body)
        {

        }
        void OnBodyExited(Node3D body)
        {

        }



        void StateLabel()
        {
            string currentState = currentAndLastState[0] switch
            {
                (int)BehaviorStateMachine.idle => "idle",
                (int)BehaviorStateMachine.patroling => "patroling",
                (int)BehaviorStateMachine.investigate => "investigate",
                (int)BehaviorStateMachine.chase => "chase",
                _ => "no state",
            };
            string _lastState = currentAndLastState[1] switch
            {
                (int)BehaviorStateMachine.idle => "idle",
                (int)BehaviorStateMachine.patroling => "patroling",
                (int)BehaviorStateMachine.investigate => "investigate",
                (int)BehaviorStateMachine.chase => "chase",
                _ => "no state",
            };
            stateMachineLabel.Text = $"current State: {currentState}\nlastState: {_lastState}";
        }
    }


}
