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
        private Area3D visionCone;
        private RayCast3D visionRay;
        // estos dos bools tienen que ser true para que enemigo vea a jugador
        private bool visionRayDetectsPlayer = false;
        private bool visionConeDetectsPlayer = false;


        public override void _Ready()
        {
            nav = GetParent<Node3D>().GetNode<NavigationAgent3D>("NavigationAgent3D");
            visionCone = GetParent<Node3D>().GetNode<Area3D>("VisionCone");
            visionRay = GetNode<RayCast3D>("../VisionCone/RayCast3D");
            // Signals
            visionCone.BodyEntered += OnPlayerEntered;
            visionCone.BodyExited += OnPlayerExited;
        }


        public override void _PhysicsProcess(double delta)
        {
            // Si player esta en visionCone(area), visionRay mira a posicion de jugador, si no hay obstaculo entre player y visualRay, entonces visualRay puede ver jugador
            if (visionCone.HasOverlappingBodies())
            {
                if (visionCone.GetOverlappingBodies()[0].IsInGroup("Player"))
                {
                    // Ray mira hacia uso mucho esta logica deberia hacerla funcion global
                    Vector3 direction = visionCone.GetOverlappingBodies()[0].GlobalPosition - visionRay.GlobalPosition;
                    Vector3 targetDirection = new(direction.X, 0, direction.Z);
                    Vector2 targetVector = new(targetDirection.X, targetDirection.Z);
                    float targetAngle = -targetVector.Angle() + Mathf.Pi / 2;
                    Vector3 rotation = visionRay.Rotation;
                    rotation.Y = Mathf.RotateToward(visionRay.Rotation.Y, targetAngle, (float)delta * 6f);
                    visionRay.Rotation = rotation;
                    // Posible problema al ver a otro
                    if (visionRay.GetCollider() is CharacterBody3D)
                    {
                        visionRayDetectsPlayer = true;
                    }
                    else
                    {
                        visionRayDetectsPlayer = false;
                    }
                }
            }
            // Si los dos ven a jugador, se cambia de state
            if (visionConeDetectsPlayer && visionRayDetectsPlayer)
            {
                state = (int)Behavior_State.chase;
            }
            else
            {
                state = (int)Behavior_State.patroling;
            }
            // Ray es top level para tener rotacion independiente, pero asigno su posicion igual a la de vision cone
            visionRay.GlobalPosition = visionCone.GlobalPosition;
        }


        private void OnPlayerEntered(Node3D body)
        {
            // Signal from visualCone
            if (body.IsInGroup("Player"))
            {
                visionConeDetectsPlayer = true;
            }
        }


        private void OnPlayerExited(Node3D body)
        {
            // Signal from visualCone
            if (body.IsInGroup("Player"))
            {
                visionConeDetectsPlayer = false;
            }
        }

    }

}