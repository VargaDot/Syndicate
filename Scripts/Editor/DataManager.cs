using System;
using Godot;
using System.Collections.Generic;

///Namespace for loading and saving data, if you're seeing this then intellisense wise, everything's fine
namespace DataManager
{
    ///<summary> Class for requesting data from Properties.JSON </summary>
    public class DataLoader
    {
        //Properties.JSON path
        const string PROPERTY_FILE = "Data/Properties.JSON";
        //Defining this function
        static Dictionary<string, Dictionary<string, Dictionary<string, object>>> OpenPropertyFile()
        {
            //Opens the file
            using var file = FileAccess.Open(PROPERTY_FILE, FileAccess.ModeFlags.Read);

            //Defines dictionary data and parses the json file
            object parsedData = Json.ParseString(file.GetAsText());
            if (parsedData is not Dictionary<string, Dictionary<string, Dictionary<string, object>>> data)
            {
                GD.Print("Failed to parse JSON data.");
                return new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();
            }

            //self explanatory no?
            return data;
        }
        private static Dictionary<string, Dictionary<string, Dictionary<string, object>>> data = OpenPropertyFile();

        ///<summary> Gets the property's price </summary>
        ///<returns> ushort </returns>
        public static ushort GetPropertyPrice(string internalPropName)
        {
            ushort value = 0;
            if(!data.ContainsKey(internalPropName))
            {
                GD.PushError("Invalid internal property name");
                return value;
            }
            
            //The reason why its a ushort, is because bytes have a max size of 255 and the price can reach 400 on some circumstances.
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

    ///<summary> The data saver, sorter and search engine for the board's registry </summary>
    public class TheRegistry
    {
        ///<summary> root is the player tree node, from here we can access the entire tree structure </summary>
        private Dictionary<byte, Player> root = new();

        public void AddPlayers(byte numberOfPlayers)
        {
            for (byte i = 1; i < numberOfPlayers; i++)
            {
                root[i] = new Player(i);
            }
        }

        public void RemovePlayer(byte playerID)
        {
            root.Remove(playerID);
        }

        public void AddProperty(byte PlayerID, byte PropertyID, byte upgradeLevel = 1, bool isMortgaged = false)
        {
            if(!root.ContainsKey(PlayerID))
                GD.PushError("Player ID is not valid");

            Player player = root[PlayerID];

            player.OwnedProperties[PropertyID] = new Property(PropertyID, upgradeLevel, isMortgaged);
        }

        public void RemoveProperty(byte playerID, byte PropertyID)
        {
            Player player = root[playerID];

            if(!root.ContainsKey(playerID))
                GD.PushError("Invalid PlayerID");

            if(!player.OwnedProperties.ContainsKey(PropertyID))
                GD.PushError("Invalid PropertyID");

            root[playerID].OwnedProperties.Remove(PropertyID);
        }

        public void UpdatePropertyLevel(byte playerID, byte PropertyID, byte newUpgradeLevel)
        {
            Player player = root[playerID];
            Property property = player.OwnedProperties[PropertyID];

            if(!root.ContainsKey(playerID))
                GD.PushError("Invalid PlayerID");

            if(!player.OwnedProperties.ContainsKey(PropertyID))
                GD.PushError("Invalid PropertyID");

            property.PropertyAttributes.Level = newUpgradeLevel;
        }

        public void UpdateMortgageStatus(byte playerID, byte PropertyID, bool newMortgageStatus)
        {
            Player player = root[playerID];
            Property property = player.OwnedProperties[PropertyID];

            if(!root.ContainsKey(playerID))
                GD.PushError("Invalid PlayerID");

            if(!player.OwnedProperties.ContainsKey(PropertyID))
                GD.PushError("Invalid PropertyID");

            property.PropertyAttributes.IsMortgaged = newMortgageStatus;
        }

        ///<summary> Returns an array with a player's owned properties </summary>
        ///<returns> Array<byte> </returns>
        public byte[] GetPlayerOwnedProperties(byte playerID)
        {
            if(!root.ContainsKey(playerID))
                GD.PushError("Invalid PlayerID");

            byte[] data = new byte[root[playerID].OwnedProperties.Count];
            return data;
        }

        ///<summary> Returns the ID of the property owner </summary>
        ///<returns> byte </returns>
        public byte FindPropertyOwner(byte propertyID)
        {
            byte owner = 0;

            foreach (byte playerID in root.Keys)
            {
                Player player = root[playerID];

                if (player.OwnedProperties.ContainsKey(propertyID))
                    owner = playerID;
                else
                    GD.Print("No owner found!");
            }

            return owner;
        }

        //Just for funsies
        public byte GetPlayerRegistrySize(byte playerID)
        {
            byte data = 0;
            if(!root.ContainsKey(playerID))
            {
                GD.PushError("Invalid PlayerID");
                return data;
            }

            data = (byte)root[playerID].OwnedProperties.Keys.Count;
            return data;
        }
    }

    // This entire part down here is for the tree data structure!

    ///<summary> The player class stores an ID and the property class (OwnedProperties) </summary>
    class Player
    {
        public byte ID {get; set;}
        public Dictionary<byte, Property> OwnedProperties {get; set;}

        public Player(byte id)
        {
            ID = id;
            OwnedProperties = new ();
        }
    }

    ///<summary> The property class stores a BoardID and an attributes class (PropertyAttributes) </summary>
    class Property
    {
        public byte ID {get; set;}
        public Attributes PropertyAttributes {get; set;}

        public Property(byte id, byte upgradeLevel, bool isMortgaged = false)
        {
            ID = id;
            PropertyAttributes = new (upgradeLevel, isMortgaged);
        }
    }

    ///<summary> The attributes class stores the level of a property (house, 2 houses ext) and a boolean that states whether its mortgaged or not </summary>
    class Attributes
    {
        public byte Level {get; set;}
        public bool IsMortgaged {get; set;}

        public Attributes(byte level, bool mortgageCondition)
        {
            Level = level;
            IsMortgaged = mortgageCondition;
        }
    }
}