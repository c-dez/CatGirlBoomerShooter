using Godot;
namespace Actors.Enemies
{
    // esta clase son los ojos de BaseEnemy, se encarga de poder mirar hacia jugador si no hay obstaculos y rotar body hacia esa direccion
    // usa una combinacion de area3d y raycast para lograr este efecto
    // si visionAreaDetectsPlayer && visionRayDetectsPlayer == true, "esta mirando"
    public partial class VisionCone : Area3D
    {
        public CharacterBody3D targetPlayer;
        public bool canSeePlayer = false;
        public Vector3 lastPlayerPosition = Vector3.Zero;
        [Signal] public delegate void OnPlayerSightEnterEventHandler();
        bool hasEmitedOnPlayerSightEnter = false;
        [Signal] public delegate void OnPlayerSightExitedEventHandler();
        bool hasEmitedOnPlayerSightExited = false;


        RayCast3D visionRay;
        BaseEnemy body;
        public bool visionAreaDetectsPlayer = false;
        public bool visionRayDetectsPlayer = false;



        public override void _Ready()
        {
            visionRay = GetNode<RayCast3D>("VisionRay");
            body = GetParent<Node>().GetParent<BaseEnemy>();
            //area signals
            BodyEntered += OnPlayerEnteredArea;
            BodyExited += OnPlayerExitedArea;

        }


        public override void _PhysicsProcess(double delta)
        {
            //visionRay es top level para que tenga rotacion independiente en x(horizontal), y para que su posicion global dea igual a la de su padre:
            visionRay.GlobalPosition = GlobalPosition;
            AreaCanSeePlayer((float)delta);

            if (!visionAreaDetectsPlayer || !visionRayDetectsPlayer)
            {
                hasEmitedOnPlayerSightEnter = false;
            }
        }


        public float GetDistanceToTargetPlayer()
        {
            if (targetPlayer != null)
            {
                float distance = body.GlobalPosition.DistanceTo(targetPlayer.GlobalPosition);
                return distance;
            }
            else
            {
                return 0f;
            }
        }


        public void LookAtDirection(Vector3 direction, float delta)
        {
            // esta public por que uso mucho esta logica
            Vector3 targetDirection = new(direction.X, 0, direction.Z);
            Vector2 targetVector2 = new(targetDirection.X, targetDirection.Z);
            float targetAngle = -targetVector2.Angle() + Mathf.Pi / 2;
            Vector3 rotation = body.Rotation;
            rotation.Y = Mathf.RotateToward(body.Rotation.Y, targetAngle, delta * 6f);
            body.Rotation = rotation;
        }

        void GetLastPlayerPosition()
        {
            if (targetPlayer != null)
            {
                lastPlayerPosition = targetPlayer.GlobalPosition;
            }
        }


        void AreaCanSeePlayer(float delta)
        {
            if (HasOverlappingBodies())
            {
                if (GetOverlappingBodies()[0].IsInGroup("Player"))
                {
                    targetPlayer = (CharacterBody3D)GetOverlappingBodies()[0];

                    // Ray mira hacia:
                    Vector3 direction = GetOverlappingBodies()[0].GlobalPosition + new Vector3(0, 1.0f, 0) - visionRay.GlobalPosition;
                    direction = direction.Normalized();

                    // calcula y (horizontal)
                    float targetAngleY = Mathf.Atan2(direction.X, direction.Z);
                    float currentY = visionRay.Rotation.Y;
                    float newY = Mathf.RotateToward(currentY, targetAngleY, (float)delta * 6f);

                    // calcula x (vertical)
                    float targetAngleX = -Mathf.Asin(direction.Y);
                    float currentX = visionRay.Rotation.X;
                    float newX = Mathf.RotateToward(currentX, targetAngleX, (float)delta * 6f);

                    // // aplicar rotacion
                    visionRay.Rotation = new Vector3(newX, newY, 0);

                    // Posible problema al ver a otro CharacteBody3D deberia se clase "Player"
                    visionRayDetectsPlayer = visionRay.GetCollider() is CharacterBody3D;

                    canSeePlayer = visionAreaDetectsPlayer && visionRayDetectsPlayer;

                    // emit signal
                    if (!hasEmitedOnPlayerSightEnter && canSeePlayer)
                    {
                        EmitSignal(SignalName.OnPlayerSightEnter);
                        hasEmitedOnPlayerSightEnter = true;
                    }

                    if (canSeePlayer)
                    {
                        lastPlayerPosition = targetPlayer.GlobalPosition;
                    }
                }
            }
            else
            {
                targetPlayer = null;

                if (hasEmitedOnPlayerSightExited)
                {
                    hasEmitedOnPlayerSightExited = false;
                }
            }
        }


        void OnPlayerEnteredArea(Node3D body)
        {
            if (body.IsInGroup("Player"))
            {
                visionAreaDetectsPlayer = true;
            }
        }


        void OnPlayerExitedArea(Node3D body)
        {
            if (body.IsInGroup("Player"))
            {
                visionAreaDetectsPlayer = false;
                EmitSignal(SignalName.OnPlayerSightExited);
            }
        }


    }

}

