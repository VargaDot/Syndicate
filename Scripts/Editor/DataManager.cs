using System;
using Godot;
using Godot.Collections;

///Namespace for loading and saving data, if you're seeing this then intellisense wise, everything's fine
namespace DataManager
{
    ///<summary> Class for requesting data from Properties.JSON </summary>
    public class DataLoader
    {
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

        ///<summary> Gets the property's price </summary>
        ///<returns> ushort </returns>
        public static ushort GetPropertyPrice(string internalPropName)
        {
            if(!data.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");
            
            //The reason why its a ushort, is because bytes have a max size of 255 and the price can reach 400 on some circumstances.
            ushort value = 0;
            value = (ushort)data[internalPropName]["Costs"]["PurchasePrice"];

            return value;
        }
        
        ///<summary>
        ///Gets the property costs from the parsed .json
        ///<list type = "number">
        ///<item><description><para><em> BuildCost </em></para></description></item>
        ///<item><description><para><em> Mortgage </em></para></description></item>
        ///</list>
        ///</summary>
        ///<returns> Byte </returns>
        public static byte GetPropertyCosts(string internalPropName, byte ID)
        {
            byte value = 0;
            if(!data.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");

            switch (ID)
            {
                case(1):
                    value = (byte)data[internalPropName]["Costs"]["BuildCost"];
                    break;
                case(2):
                    value = (byte)data[internalPropName]["Costs"]["Mortgage"];
                    break;
                default:
                    GD.PushError("GetPropertyCosts Invalid ID");
                    GD.Print("Value: ", value, " ID: ", ID);
                    break;
            }
            
            return value;
        }

        ///<summary>
        ///Gets the property rent values and purchasing info in the form of a numerical value from the parsed .json
        ///<list type = "number">
        ///<item><description><para><em> NoBuildings </em></para></description></item>
        ///<item><description><para><em> House </em></para></description></item>
        ///<item><description><para><em> House2 </em></para></description></item>
        ///<item><description><para><em> House3 </em></para></description></item>
        ///<item><description><para><em> House4 </em></para></description></item>
        ///<item><description><para><em> Hotel </em></para></description></item>
        ///</list>
        ///</summary>
        ///<returns> ushort </returns>
        public static ushort GetPropertyRent(string internalPropName, byte upgradeLevel)
        {
            ushort value = 0;
            if(!data.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");

            switch (upgradeLevel)
            {
                case(1):
                    value = (ushort)data[internalPropName]["RentValues"]["NoBuildings"];
                    break;
                case(2):
                    value = (ushort)data[internalPropName]["RentValues"]["House"];
                    break;
                case(3):
                    value = (ushort)data[internalPropName]["RentValues"]["House2"];
                    break;
                case(4):
                    value = (ushort)data[internalPropName]["RentValues"]["House3"];
                    break;
                case(5):
                    value = (ushort)data[internalPropName]["RentValues"]["House4"];
                    break;
                case(6):
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
        public static string GetTextForCard(string internalPropName, byte ID)
        {
            string value = null;
            if(!data.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");
            
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
        ///<returns> Byte </returns>
        public static byte GetAnyID(string internalPropName, byte ID)
        {
            if(!data.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");
            
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

    //<summary> Class for saving data to Registry.JSON </summary>
    public class DataSaver
    {
        //Registry.JSON's path
        const string REGISTRY_DATA = "Data/Registry.JSON";
        static Dictionary<byte, Dictionary<byte,byte>> OpenRegistry()
        {
            using var file = FileAccess.OpenCompressed(REGISTRY_DATA, FileAccess.ModeFlags.Read, FileAccess.CompressionMode.Fastlz);

            Dictionary<byte, Dictionary<byte,byte>> data = new Dictionary<byte, Dictionary<byte, byte>>();
            data = (Dictionary<byte, Dictionary<byte,byte>>)Json.ParseString(file.GetAsText());

            return data;
        }
        static Dictionary<byte, Dictionary<byte,byte>> data = OpenRegistry();

        public static bool CheckOwnership(byte AgentID, byte BoardID)
        {
            bool status = false;
            if(!data.ContainsKey(AgentID) || !data[AgentID].ContainsKey(BoardID))
                GD.PushError("Invalid AgentID/BoardID");

            if (true)
            {
                
            }

            return status; 
        }

        
    }
}