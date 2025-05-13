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


        }

        public override void _PhysicsProcess(double delta)
        {
            StateLabel();
        }


        void StateLabel()
        {
            stateMachineLabel.Text = state switch
            {
                (int)BehaviorStateMachine.idle => "idle",
                (int)BehaviorStateMachine.patroling => "patroling",
                (int)BehaviorStateMachine.investigate => "investigate",
                (int)BehaviorStateMachine.chase => "chase",
                _ => "no state",
            };
        }
    }


}
