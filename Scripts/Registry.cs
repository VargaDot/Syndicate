using Godot;
using System;

//This is responsible for the ownership system
public partial class Registry : Node2D
{
    enum OwnershipStatus
    {
        Unowned = 0,
        Owned = 1,
        Mortgaged = 2,
    }

    
}
