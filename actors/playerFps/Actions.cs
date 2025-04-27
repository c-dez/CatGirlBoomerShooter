using Godot;

public partial class Actions : Node
{
    [Export] Godot.Timer secondAttackTimer;
    private AnimationTree animationTree;
    private AnimationNodeStateMachinePlayback attackStateMachine;
    private bool isAttacking = false;
    public override void _Ready()
    {
        animationTree = GetNode<AnimationTree>("../AnimationTree");
        attackStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/AttackStateMachine/playback");

    }

    public override void _PhysicsProcess(double delta)
    {
        Attack();
        // GD.Print(isAttacking);
        
    }

    private void Attack()
    {
        if (Input.IsActionJustPressed("mb1") && !isAttacking)
        {
            animationTree.Set("parameters/AttackOneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
            attackStateMachine.Travel(secondAttackTimer.TimeLeft > 0f ? "attack2" : "attack1");
        }
    }


    private void AttackingToggle(bool value)
    {
        isAttacking = value;
    }


    private void StartSecondAttackTimer()
    {
        secondAttackTimer.Start();
    }
}
