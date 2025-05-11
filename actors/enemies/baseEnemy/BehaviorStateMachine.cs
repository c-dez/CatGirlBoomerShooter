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
                    Vector3 direction = visionCone.GetOverlappingBodies()[0].GlobalPosition + new Vector3(0,1.0f,0) - visionRay.GlobalPosition;
                    direction = direction.Normalized();
                    // calcula y (horizontal)
                    float targetAngleY = Mathf.Atan2(direction.X, direction.Z);
                    float currentY = visionRay.Rotation.Y;
                    float newY = Mathf.RotateToward(currentY,targetAngleY, (float)delta * 6f);
                    
                    // calcula x (vertical)
                    float targetAngleX = - Mathf.Asin(direction.Y);
                    float currentX = visionRay.Rotation.X;
                    float newX = Mathf.RotateToward(currentX, targetAngleX, (float)delta * 6f);

                    // // aplicar rotacion
                    visionRay.Rotation = new Vector3(newX, newY, 0);

                    // Posible problema al ver a otro CharacteBody3D
                    // Esta si por que player solo es un Characterbody3d, solo hereda de CharacterBody3D
                    visionRayDetectsPlayer = visionRay.GetCollider() is CharacterBody3D;
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