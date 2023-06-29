using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class Properties : Node
{
    public override void _Ready()
    {
        LoadBoard();
    }

    //Responsible for filling all the dictionaries with the necessary values
    void LoadBoard()
    {

    }

    /*
    public Dictionary prop
    {
        get
        {
            var file = new File();
            file.Open("res://Data/Properties.JSON", FileMode.Open);
            var text = file.GetAsText();
        }
        return prop;
    };
    */
    
}
