using Godot;
using System;

public partial class StateMachine : Node
{

    public enum STATES
    {
        moving,
        dashing,
        isGrapping,
        notGrpping,
    }

    
    public int state = 0;


}

