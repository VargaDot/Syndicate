using Godot;
using Godot.Collections;
using System;

public partial class DataManager : Node
{
    /// <summary>
    /// Utilises the internal name for a board tile and a designated ID for a function and another possible id for a specific function
    /// <list type = "number">
    /// <item><description><para><em> Name </em></para></description></item>
    /// <item><description><para><em> Purchasing Price </em></para></description></item>
    /// <item><description><para><em> Build Cost </em></para></description></item>
    /// <item><description><para><em> Mortgage </em></para></description></item>
    /// <item><description><para><em> Rent Values </em></para></description></item>
    /// </list>
    /// <strong>Anything related to RentValues requires an additional ID for their upgrade level (No house = 0, Hotel = 5), the default value is 0</strong>
    /// </summary>
    /// <returns>
    /// A variant, to be converted manually through additional arguments.
    /// </returns>
    public Variant DataRequester(string TileName, int functionID, int upgradeArg = 0)
    {
        Variant request = "";

        switch (functionID)
        {
            case(1):
                request = GetPropertyName(TileName);
                break;
            case(2):
                request = GetPropertyPrice(TileName);
                break;
            case(3):
                request = GetPropertyBuildcost(TileName);
                break;
            case(4):
                request = GetPropertyMortgage(TileName);
                break;
            case(5):
                request = GetPropertyRent(TileName, upgradeArg);
                break;
            default:
                GD.PushError("Something happened with the data requester, here are the input values");
                GD.Print("Tilename: ", TileName, " FunctionID: ", functionID, " UpgradeLevel:", upgradeArg, " Requested element: ", request);
                break;
        }

        return request;
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

    //Gets the property name string from the parsed .json
    static string GetPropertyName(string internalPropName)
    {
        var data = OpenPropertyFile();
        string propertyName = null;

        if (data.ContainsKey(internalPropName))
            return propertyName = (string)data[internalPropName]["ID"]["Name"];
        else
        {
            GD.PushError("Invalid Propname");
            GD.Print("Property Name: ", internalPropName, " Name: ", propertyName);
        }

        return propertyName;
    }

    //Gets the property price from the parsed .json
    static int GetPropertyPrice(string internalPropName)
    {
        var data = OpenPropertyFile();
        int value = 0;

        if (data.ContainsKey(internalPropName))
        {
            value = (int)data[internalPropName]["Costs"]["PurchasePrice"];
        }
        else
        {
            GD.PushError("Invalid Propname");
            GD.Print("Property Name: ", internalPropName, " Value: ", value);
        }

        return value;
    }

    //Gets the property build cost from the parsed .json
    static int GetPropertyBuildcost(string internalPropName)
    {
        var data = OpenPropertyFile();
        int value = 0;

        if (data.ContainsKey(internalPropName))
        {
            value = (int)data[internalPropName]["Costs"]["BuildCost"];
        }
        else
        {
            GD.PushError("Invalid Propname");
            GD.Print("Property Name: ", internalPropName, " Value: ", value);
        }

        return value;
    }

    //Gets the property mortgage from the parsed .json
    static int GetPropertyMortgage(string internalPropName)
    {
        var data = OpenPropertyFile();
        int value = 0;

        if (data.ContainsKey(internalPropName))
        {
            value = (int)data[internalPropName]["Costs"]["Mortgage"];
        }
        else
        {
            GD.PushError("Invalid Propname");
            GD.Print("Property Name: ", internalPropName, " Value: ", value);
        }

        return value;
    }
    
    //Gets the property rent values from the parsed .json
    static int GetPropertyRent(string internalPropName, int upgradeLevel)
    {
        var data = OpenPropertyFile();
        int value = 0;

        //Checks the upgrade level for the property and displays its correct value!
        if (data.ContainsKey(internalPropName) && upgradeLevel == 0)
        {
            value = (int)data[internalPropName]["RentValues"]["NoBuildings"];
        }
        else if (data.ContainsKey(internalPropName) && upgradeLevel == 1)
        {
            value = (int)data[internalPropName]["RentValues"]["House"];
        }
        else if (data.ContainsKey(internalPropName) && upgradeLevel == 2)
        {
            value = (int)data[internalPropName]["RentValues"]["House2"];
        }
        else if (data.ContainsKey(internalPropName) && upgradeLevel == 3)
        {
            value = (int)data[internalPropName]["RentValues"]["House3"];
        }
        else if (data.ContainsKey(internalPropName) && upgradeLevel == 4)
        {
            value = (int)data[internalPropName]["RentValues"]["House4"];
        }
        else if (data.ContainsKey(internalPropName) && upgradeLevel == 5)
        {
            value = (int)data[internalPropName]["RentValues"]["Hotel"];
        }
        else
        {
            GD.PushError("Invalid Upgrade Level/PropertyName");
            GD.Print("Value:", value, " Upgrade Level: ", upgradeLevel, " PropertyName: ", internalPropName);
        }

        return value;
    }
}
