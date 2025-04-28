using Godot;
using System;
namespace Actors;
public partial class Stats : Node
{
    [Export] public int Health = 100;
    public int ammo = 10;
}
