using Godot;
namespace Actors.Enemies
{
    public partial class Logic : Node
    {
        BaseEnemy body;
        VisionCone visionCone;
        Node3D skin;
        Navigation nav;

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


        }

        public override void _PhysicsProcess(double delta)
        {
            GD.Print(visionCone.lastPlayerPosition);
        }
    }

}
