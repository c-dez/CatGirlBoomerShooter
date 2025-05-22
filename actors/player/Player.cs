using Actors.Players;
using Godot;
using System;
namespace Actor.Players
{
    public partial class Player : CharacterBody3D
    {
        // move se encarga de movimiento, las variables relacionadas estan dentro de el nodo
        Move move;


        public override void _Ready()
        {
            move = GetNode<Move>("Components/Move");
        }

        public override void _PhysicsProcess(double delta)
        {
            move.MoveOnFloor();
            move.Jump((float)delta);
            move.WallJump();// falta muros que detectar
        }
    }

}
