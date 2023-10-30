using Godot;

public partial class GameUI : Node
{
    public void GameSignalReceiver(byte functionID, Variant secondOption)
    {
        switch (functionID)
        {
            case 0:
                EmitSignal("DiceUI", secondOption);
                break;
            case 1:
                EmitSignal("CardUI", secondOption);
                break;
            case 2:
                EmitSignal("PrisonUI");
                break;
            case 3:
                EmitSignal("TradeUI");
                break;
            default:
                GD.PushError("Invalid functionID: ", functionID);
                break;
        }
    }

    [Signal] public delegate void DiceUIEventHandler(byte diceRoll);
    [Signal] public delegate void CardUIEventHandler(string internalPropName);
    [Signal] public delegate void PrisonUIEventHandler();
    [Signal] public delegate void TradeUIEventHandler();



}
