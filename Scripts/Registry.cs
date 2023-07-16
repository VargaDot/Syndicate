using Godot;
using Godot.Collections;
using System;

//This is responsible for the ownership system
public partial class Registry : Node2D
{
    enum STATUS
    {
        UNOWNED,
        OWNED,
        MORTGAGED,
    }

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

    /*
    public static Dictionary<int,Dictionary<int,int>> TheRegistry = new Dictionary<int, Dictionary<int,int>>()
    {
        {00, new Dictionary<int, int> {{0,0}} },
        {00, new Dictionary<int, int> {{0,0}} },
        {00, new Dictionary<int, int> {{0,0}} },
        {00, new Dictionary<int, int> {{0,0}} },
        {00, new Dictionary<int, int> {{0,0}} },
    };
    */

}
