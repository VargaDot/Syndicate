using Godot;
using Godot.Collections;
using System;

public partial class DataManager : Node
{
    const string PROPERTY_FILE = "Data/Properties.JSON";
    static Dictionary<string,Dictionary<string,Dictionary<string, Variant>>> OpenPropertyFile()
    {
        using var file = FileAccess.Open(PROPERTY_FILE, FileAccess.ModeFlags.Read);

        Dictionary<string,Dictionary<string,Dictionary<string, Variant>>> data = new Dictionary<string,Dictionary<string,Dictionary<string, Variant>>>();
        data = (Dictionary<string,Dictionary<string,Dictionary<string, Variant>>>)Json.ParseString(file.GetAsText());

        return data;
    }

    public static string GetPropertyName(string internalPropName)
    {
        var data = OpenPropertyFile();
        string propertyName = null;

        if (data.ContainsKey(internalPropName))
            return propertyName = (string)data[internalPropName]["ID"]["Name"];

        return propertyName;
    }

    public static int GetPropertyPrice(string internalPropName)
    {
        var data = OpenPropertyFile();
        int value = 0;

        if (data.ContainsKey(internalPropName))
        {
            value = (int)data[internalPropName]["Costs"]["PurchasePrice"];
        }

        return value;
    }

    public static int GetPropertyBuildcost(string internalPropName)
    {
        var data = OpenPropertyFile();
        int value = 0;

        if (data.ContainsKey(internalPropName))
        {
            value = (int)data[internalPropName]["Costs"]["BuildCost"];
        }

        return value;
    }

    public static int GetPropertyMortgage(string internalPropName)
    {
        var data = OpenPropertyFile();
        int value = 0;

        if (data.ContainsKey(internalPropName))
        {
            value = (int)data[internalPropName]["Costs"]["Mortgage"];
        }

        return value;
    }

    public static int GetPropertyRent(string internalPropName)
    {
        var data = OpenPropertyFile();
        int value = 0;

        if (data.ContainsKey(internalPropName))
        {
            value = (int)data[internalPropName]["RentValues"]["Rent"];
        }

        return value;
    }
}
