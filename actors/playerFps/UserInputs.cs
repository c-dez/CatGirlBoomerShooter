using Godot;
using System;

public partial class UserInputs : Node
{
    [Export] private Camera3D camera;
    public Vector3 moveDirection = Vector3.Zero;

    public override void _Ready()
    {
        
    }
    public override void _Process(double delta)
    {
        GetMoveDirection();
    }
    private void GetMoveDirection()
    {
        Vector2 rawInput = Input.GetVector("left", "right", "forward", "backwards");
        Vector3 forward = camera.GlobalBasis.Z;
        Vector3 right = camera.GlobalBasis.X;
        moveDirection = (forward * rawInput.Y + right * rawInput.X).Normalized();
        // GD.Print(moveDirection);
    }
}
