using Godot;
using Godot.Collections;
using System;

public partial class DataManager : Node
{
    /// <summary>
    /// Utilises the internal name for a property and a designated functionID and an optional ID for access to other functions
    /// <list type = "number">
    /// <item><description><para><em> Purchasing Price* </em></para></description></item>
    /// <item><description><para><em> Property Costs (Build costs & Mortgage) </em></para></description></item>
    /// <item><description><para><em> Property Rent Values </em></para></description></item>
    /// <item><description><para><em> Card Text </em></para></description></item>
    /// <item><description><para><em> Property IDs </em></para></description></item>
    /// </list>
    /// <strong>Functions with a * do not require additional arguments, default is always 0</strong>
    /// </summary>
    /// <returns>
    /// A variant, to be converted manually through additional arguments.
    /// </returns>
    public void DataRequester(Variant request, string TileName, byte functionID, byte Arg = 0)
    {
        if(data.ContainsKey(TileName))
        {
            switch (functionID)
            {
                case(1):
                    request = GetPropertyPrice(TileName);
                    break;
                case(2):
                    request = GetPropertyCosts(TileName, Arg);
                    break;
                case(3):
                    request = GetPropertyRent(TileName, Arg);
                    break;
                case(4):
                    request = GetTextForCard(TileName, Arg);
                    break;
                case(5):
                    request = GetAnyID(TileName, Arg);
                    break;
                default:
                    GD.PushError("Something happened with the data requester, here are the input values");
                    GD.Print("Tilename: ", TileName, " FunctionID: ", functionID, " Optional Argument: ", Arg, " Requested element: ", request);
                break;
            }
        }
        else
        {
            GD.PushError("Invalid Propname");
            GD.Print("Property Name: ", TileName);
        }
    }

    //Properties.JSON path
    const string PROPERTY_FILE = "Data/Properties.JSON";
    //Defining this function
    static Dictionary<string,Dictionary<string,Dictionary<string, Variant>>> OpenPropertyFile()
    {
        //Opens the file
        using var file = FileAccess.Open(PROPERTY_FILE, FileAccess.ModeFlags.Read);
        
        //Defines dictionary data and parses the json file
        Dictionary<string,Dictionary<string,Dictionary<string, Variant>>> data = new Dictionary<string,Dictionary<string,Dictionary<string, Variant>>>();
        data = (Dictionary<string,Dictionary<string,Dictionary<string, Variant>>>)Json.ParseString(file.GetAsText());
        
        //self explanatory no?
        return data;
    }
    static Dictionary<string, Dictionary<string, Dictionary<string, Variant>>> data = OpenPropertyFile();

    //Gets the property price from the parsed .json
    static ushort GetPropertyPrice(string internalPropName)
    {
        //The reason why its a ushort, is because bytes have a max size of 255 and the price can reach 400 on some circumstances.
        ushort value = 0;
        value = (ushort)data[internalPropName]["Costs"]["PurchasePrice"];

        return value;
    }
    
    //Gets house building costs and mortgage
    static byte GetPropertyCosts(string internalPropName, byte ID)
    {
        byte value = 0;
        switch (ID)
        {
            case(0):
                value = (byte)data[internalPropName]["Costs"]["BuildCost"];
                break;
            case(1):
                value = (byte)data[internalPropName]["Costs"]["Mortgage"];
                break;
            default:
                GD.PushError("GetPropertyCosts Invalid ID");
                GD.Print("Value: ", value, " ID: ", ID);
                break;
        }
        
        return value;
    }

    //Gets the property rent values from the parsed .json
    static ushort GetPropertyRent(string internalPropName, byte upgradeLevel)
    {
        ushort value = 0;
        switch (upgradeLevel)
        {
            case(0):
                value = (ushort)data[internalPropName]["RentValues"]["NoBuildings"];
                break;
            case(1):
                value = (ushort)data[internalPropName]["RentValues"]["House"];
                break;
            case(2):
                value = (ushort)data[internalPropName]["RentValues"]["House2"];
                break;
            case(3):
                value = (ushort)data[internalPropName]["RentValues"]["House3"];
                break;
            case(4):
                value = (ushort)data[internalPropName]["RentValues"]["House4"];
                break;
            case(5):
                value = (ushort)data[internalPropName]["RentValues"]["Hotel"];
                break;
            default:
                GD.PushError("GetPropertyRent Invalid ID");
                GD.Print("Value:", value, " Upgrade Level: ", upgradeLevel);
            break;
        }

        return value;
    }

    ///<summary>
    ///Gets the property rent values and purchasing info in the form of text from the parsed .json
    ///<list type = "number">
    ///<item><description><para><em> Name </em></para></description></item>
    ///<item><description><para><em> NoBuildings </em></para></description></item>
    ///<item><description><para><em> House </em></para></description></item>
    ///<item><description><para><em> House2 </em></para></description></item>
    ///<item><description><para><em> House3 </em></para></description></item>
    ///<item><description><para><em> House4 </em></para></description></item>
    ///<item><description><para><em> Hotel </em></para></description></item>
    ///<item><description><para><em> Mortgage </em></para></description></item>
    ///<item><description><para><em> BuildCost </em></para></description></item>
    ///<item><description><para><em> PurchasePrice </em></para></description></item>
    ///</list>
    ///</summary>
    ///<returns> String </returns>
    static string GetTextForCard(string internalPropName, byte ID)
    {
        string value = null;
        switch (ID)
        {
            case(1):
                value = (string)data[internalPropName]["ID"]["Name"];
                break;
            case(2):
                value = (string)data[internalPropName]["RentValues"]["NoBuildings"];
                break;
            case(3):
                value = (string)data[internalPropName]["RentValues"]["House"];
                break;
            case(4):   
                value = (string)data[internalPropName]["RentValues"]["House2"];
                break;
            case(5):
                value = (string)data[internalPropName]["RentValues"]["House3"];
                break;
            case(6):
                value = (string)data[internalPropName]["RentValues"]["House4"];
                break;
            case(7):
                value = (string)data[internalPropName]["RentValues"]["Hotel"];
                break;
            case(8):
                value = (string)data[internalPropName]["Costs"]["Mortgage"];
                break;
            case(9):
                value = (string)data[internalPropName]["Costs"]["BuildCost"];
                break;
            case(10):
                value = (string)data[internalPropName]["Costs"]["PurchasePrice"];
                break;
            default:
                GD.PushError("GetTextForCard Invalid ID");
                GD.Print("Value: ", value, " ID: ", ID);
            break;
        }

        return value;
    }

    ///<summary>
    ///Gets CardID or BoardID from the parsed .JSON
    ///<list type = "number">
    ///<item><description><para><em> CardID </em></para></description></item>
    ///<item><description><para><em> BoardID </em></para></description></item>
    ///</list>
    ///</summary>
    ///<returns>
    ///Byte
    ///</returns>
    static byte GetAnyID(string internalPropName, byte ID)
    {
        byte value = 0;
        switch (ID)
        {
            case(1):
                value = (byte)data[internalPropName]["ID"]["CardID"];
                break;
            case(2):
                value = (byte)data[internalPropName]["ID"]["BoardID"];
                break;
            default:
                GD.PushError("GetAnyID Invalid ID");
                GD.Print("Value: ", value, " ID: ", ID);
            break;
        }

        return value;
    }

}
