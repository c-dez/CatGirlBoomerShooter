using Actors.Players;
using Godot;
namespace Actor.Players
{
    public partial class Player : CharacterBody3D
    {
        Move move;
        UserInputs userInputs;


        public override void _Ready()
        {
            move = GetNode<Move>("Components/Move");
            userInputs = GetNode<UserInputs>("Components/UserInputs");
        }

        public override void _PhysicsProcess(double delta)
        {
            move.MoveOnFloor();
            move.Jump((float)delta);
            move.WallJump();// falta muros que detectar
        }

        public override void _Process(double delta)
        {
            userInputs.Dodge();
            userInputs.Shoot();
            userInputs.ChangeBetweenFirstAndThridPersonCamera();
        }
    }

}
