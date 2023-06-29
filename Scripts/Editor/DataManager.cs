using Godot;
using Godot.Collections;
using System;

public partial class DataManager : Node
{
    const string PROPERTY_FILE = "Data/Properties.JSON";
    static Dictionary<string, Dictionary<string, Array<string>>> OpenPropertyFile()
    {
        using var file = FileAccess.Open(PROPERTY_FILE, FileAccess.ModeFlags.Read);

        Dictionary<string,Dictionary<string,Array<string>>> data = new Dictionary<string, Dictionary<string, Array<string>>>();
        data = (Dictionary<string,Dictionary<string,Array<string>>>)Json.ParseString(file.GetAsText());

        return data;
    }

    /*
    public static string GetPropertyName(string internalPropName)
    {
        var data = OpenPropertyFile();

        if (data.ContainsKey(internalPropName))
            return data[internalPropName][yes];

        return null;
    }
    */
}
