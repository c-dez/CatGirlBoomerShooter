using Actors.Players;
using Godot;
namespace Actor.Players
{
    public partial class Player : CharacterBody3D
    {
        Move move;
        UserInputs userInputs;
        Camera camera;


        public override void _Ready()
        {
            move = GetNode<Move>("Components/Move");
            userInputs = GetNode<UserInputs>("Components/UserInputs");
            camera = GetNode<Camera>("Components/ThirdPersonCamera");
        }

        public override void _PhysicsProcess(double delta)
        {
            move.MoveOnFloor();
            move.Jump((float)delta);
            move.WallJump();// falta muros que detectar

            camera.CameraRotation((float)delta);
            camera.SkinRotation((float)delta);
        }

        public override void _Process(double delta)
        {
            userInputs.Dodge();
            userInputs.Shoot();
            userInputs.ChangeBetweenFirstAndThridPersonCamera();
        }
    }

}
