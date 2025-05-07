using Godot;
namespace Actors.Players
{
    public partial class Actions : Node
    {
        // [Export] Timer secondAttackTimer;
        // private AnimationTree animationTree;
        // private AnimationNodeStateMachinePlayback attackStateMachine;
        // private bool isAttacking = false;
        private UserInputs userInputs;

        public override void _Ready()
        {
            // animationTree = GetNode<AnimationTree>("../AnimationTree");
            // attackStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/AttackStateMachine/playback");
            userInputs = GetNode<UserInputs>("../UserInputs");
        }
        

        public override void _PhysicsProcess(double delta)
        {
            // Attack();
            Dodge();
            // userInputs.GetLastMoveDirection();
            // GD.Print(userInputs.GetLastMoveDirection());


        }

        private void Dodge()
        {
            if (userInputs.GetUserLastMoveDirectionInput() != Vector3.Zero)
            {

            }
            // GD.Print(userInputs.GetLastMoveDirection());
        }


        // private void Attack()
        // {
        //     if (Input.IsActionJustPressed("mb1") && !isAttacking)
        //     {
        //         animationTree.Set("parameters/AttackOneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
        //         attackStateMachine.Travel(secondAttackTimer.TimeLeft > 0f ? "attack2" : "attack1");
        //     }
        // }


        // private void AttackingToggle(bool value)
        // {
        //     isAttacking = value;
        // }


        // private void StartSecondAttackTimer()
        // {
        //     secondAttackTimer.Start();
        // }
    }
}