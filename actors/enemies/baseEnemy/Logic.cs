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
        int[] currentAndLastState = new int[2];

        // debug
        Label stateMachineLabel;

        enum BehaviorState
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
            visionCone.OnPlayerSightEnter += OnPlayerOnSightEnter;
            visionCone.OnPlayerSightExited += OnPlayerOnSightExited;

        }


        public override void _PhysicsProcess(double delta)
        {
            float dt = (float)delta;
            StateLabel();
            if (currentAndLastState[0] == (int)BehaviorState.chase)
            {
                nav.NavigateTo(dt, visionCone.targetPlayer.GlobalPosition, body.speed);
            }
            else if (currentAndLastState[0] == (int)BehaviorState.investigate)
            {
                nav.NavigateTo(dt,visionCone.lastPlayerPosition , body.walkSpeed);
            }
        }


        void OnPlayerOnSightEnter()
        {
            SetCurrentAndLastState((int)BehaviorState.chase);
        }


        void OnPlayerOnSightExited()
        {
            SetCurrentAndLastState((int)BehaviorState.investigate);   
        }

        void SetCurrentAndLastState(int behaviorState)
        {
            // en un array guardo el state actual y pasado, en esta funcion se usa para hacer el cambio de state y se encarga de la logica para almacenar el lastState
            int newState = behaviorState;
            currentAndLastState[1] = currentAndLastState[0];
            currentAndLastState[0] = newState;
        }




        void StateLabel()
        {
            // para Debugg
            string currentState = currentAndLastState[0] switch
            {
                (int)BehaviorState.idle => "idle",
                (int)BehaviorState.patroling => "patroling",
                (int)BehaviorState.investigate => "investigate",
                (int)BehaviorState.chase => "chase",
                _ => "no state",
            };
            string _lastState = currentAndLastState[1] switch
            {
                (int)BehaviorState.idle => "idle",
                (int)BehaviorState.patroling => "patroling",
                (int)BehaviorState.investigate => "investigate",
                (int)BehaviorState.chase => "chase",
                _ => "no state",
            };
            stateMachineLabel.Text = $"current State: {currentState}\nlastState: {_lastState}";
        }
    }


}
