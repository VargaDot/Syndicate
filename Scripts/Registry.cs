using Godot;
using DataManager;
using System;

//This is responsible for the ownership system
public partial class Registry : Node2D
{
    enum TileType
    {
        GO,
        PROPERTY,
        ITAX,
        CHANCE,
        CHEST,
        JAIL,
        GOJAIL,
        PARKING,
        LTAX,
    }

    public override void _Ready()
    {
        LoadValues();
    }

    void LoadValues()
    {
        
    }



}
