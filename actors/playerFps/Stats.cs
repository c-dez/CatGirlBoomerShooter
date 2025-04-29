using Godot;
using System;
// namespace Actors
// {
    public partial class Stats : Node
    {
        [Export] public int Health = 100;
        [Export] public int ammo = 10;
        [Export] public int attackDamage = 10;
    }
// }