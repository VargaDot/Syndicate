using Godot;
using DataManager;
using System;

//This is responsible for the ownership system
public partial class Registry : Node2D
{
    enum KIND
    {
        NORMAL,
        STATION,
        UTILITY,
    }

    public override void _Ready()
    {
        LoadValues();
    }

    void LoadValues()
    {
        
    }



}
