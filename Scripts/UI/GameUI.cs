using Godot;
using System;

public partial class GameUI : Node
{

    

    [Signal] public delegate void DiceUIEventHandler();
    [Signal] public delegate void CardUIEventHandler(string internalPropName);
    [Signal] public delegate void PrisonUIEventHandler();
    [Signal] public delegate void TradeUIEventHandler();



}
