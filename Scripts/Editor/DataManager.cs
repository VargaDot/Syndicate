using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

///Namespace for loading and saving data, if you're seeing this then intellisense wise, everything's fine
namespace DataManager
{
    ///<summary> Class for requesting data from Properties.JSON </summary>
    public class DataLoader
    {
        //Properties.JSON path
        readonly static string PROPERTY_FILE = File.ReadAllText("Data/Properties.JSON");

        // This whole portion here is just defining what and how to read the JSON file's structures
        private static readonly Dictionary<string, Dictionary<string, object>> IDdata = OpenPropertyID();
        static Dictionary<string, Dictionary<string, object>> OpenPropertyID()
        {
            Dictionary<string, Dictionary<string, object>> data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(PROPERTY_FILE);
            return data;
        }

        private static readonly Dictionary<string, Dictionary<string, Dictionary<string, byte>>> CostData = OpenPropertyCosts();
        static Dictionary<string, Dictionary<string, Dictionary<string, byte>>> OpenPropertyCosts()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, byte>>> data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, byte>>>>(PROPERTY_FILE);
            return data;
        }

        private static readonly Dictionary<string, byte[]> RentData = OpenPropertyRent();
        static Dictionary<string, byte[]> OpenPropertyRent()
        {
            Dictionary<string, byte[]> data = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(PROPERTY_FILE); 
            return data;
        }

        ///<summary> Gets the property's price </summary>
        ///<returns> ushort </returns>
        public static ushort GetPropertyPrice(string internalPropName)
        {
            ushort value = 0;
            if(!IDdata.ContainsKey(internalPropName))
            {
                GD.PushError("Invalid internal property name");
                return value;
            }
            
            //The reason why its a ushort, is because bytes have a max size of 255 and the price can reach 400 on some circumstances.
            value = CostData[internalPropName]["Costs"]["PurchasePrice"];
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
            if(!CostData.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");

            switch (ID)
            {
                case(1):
                    value = CostData[internalPropName]["Costs"]["BuildCost"];
                    break;
                case(2):
                    value = CostData[internalPropName]["Costs"]["Mortgage"];
                    break;
                default:
                    GD.PushError("GetPropertyCosts Invalid ID");
                    GD.Print("Value: ", value, " ID: ", ID);
                    break;
            }
            
            return value;
        }

        ///<summary>
        ///Gets the property rent values and purchasing info in the form of a numerical value from the parsed .json, Upgrade levels start from 0
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
            if(!RentData.ContainsKey(internalPropName) || upgradeLevel > 5)
            {
                GD.PushError("Invalid internal property name or upgradelevel", upgradeLevel);
                return value;
            }

            value = RentData[internalPropName][upgradeLevel];
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
            byte byteValue = 0;
            if(!IDdata.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");
            
            switch (ID)
            {
                case 1:
                    value = (string)IDdata[internalPropName]["Name"];
                    break;
                case 2:
                    byteValue = RentData[internalPropName][0];
                    break;
                case 3:
                    byteValue = RentData[internalPropName][1];
                    break;
                case 4:   
                    byteValue = RentData[internalPropName][2];
                    break;
                case 5:
                    byteValue = RentData[internalPropName][3];
                    break;
                case 6:
                    byteValue = RentData[internalPropName][4];
                    break;
                case 7:
                    byteValue = RentData[internalPropName][5];
                    break;
                case 8:
                    byteValue = CostData[internalPropName]["Costs"]["Mortgage"];
                    break;
                case 9:
                    byteValue = CostData[internalPropName]["Costs"]["BuildCost"];
                    break;
                case 10:
                    byteValue = CostData[internalPropName]["Costs"]["PurchasePrice"];
                    break;
                default:
                    GD.PushError("GetTextForCard Invalid ID");
                    GD.Print("Value: ", value, " ID: ", ID);
                break;
            }

            if(ID >= 2)
                value = byteValue.ToString();
            
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
            if(!IDdata.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");
            
            byte value = 0;
            switch (ID)
            {
                case 1:
                    value = (byte)IDdata[internalPropName]["CardID"];
                    break;
                case 2:
                    value = (byte)IDdata[internalPropName]["BoardID"];
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
        // This entire part down here is for the tree data structure!
        ///<summary> The player class stores an ID and the property struct (OwnedProperties) </summary>
        struct Player
        {
            public byte ID {get; set;}
            public Dictionary<byte, Property> OwnedProperties {get; set;}
            public int Cash {get; set;}
            public bool inPrison = false;
            public string Name {get; set;}

            public Player(byte id, int cash, string name)
            {
                ID = id;
                Cash = cash;
                OwnedProperties = new ();
                Name = name;
            }
        }

        ///<summary> The property class stores a BoardID and a property's attributes (upgradeLevel and mortgage status) </summary>
        struct Property
        {
            public byte ID {get; set;}
            public byte UpgradeLevel {get; set;}
            public bool IsMortgaged {get; set;}

            public Property(byte id, byte upgradeLevel, bool isMortgaged = false)
            {
                ID = id;
                UpgradeLevel = upgradeLevel;
                IsMortgaged = isMortgaged;
            }
        }


        // This is the functions part

        ///<summary> root is the player tree node, from here we can access the entire tree structure </summary>
        private readonly Dictionary<byte, Player> root = new();
        public void AddPlayers(byte ID, ushort startingCash, string username)
        {
            if(!root.ContainsKey(ID))
                root[ID] = new Player(ID, startingCash, username);
            else
                GD.Print("There's already an ID of the same kind");
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

            property.UpgradeLevel = newUpgradeLevel;
        }

        public void UpdateMortgageStatus(byte playerID, byte PropertyID, bool newMortgageStatus)
        {
            Player player = root[playerID];
            Property property = root[playerID].OwnedProperties[PropertyID];

            if(!root.ContainsKey(playerID))
                GD.PushError("Invalid PlayerID");

            if(!player.OwnedProperties.ContainsKey(PropertyID))
                GD.PushError("Invalid PropertyID");

            property.IsMortgaged = newMortgageStatus;
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

    public class UsernamesManager
    {
        public static string[] StoreUsernames(string P1, string P2, string P3 = null, string P4 = null)
        {
            string[] data = new string[4];

            data.SetValue(P1, 0);
            data.SetValue(P2, 1);

            if(P3 != null)
                data.SetValue(P3, 2);
            
            if(P4 != null)
                data.SetValue(P4, 3);
            
            SaveUsernamesToLocal(data);
            return data;
        }

        const string USERNAMES_FILE = "Data/Registry.JSON";
        private static void SaveUsernamesToLocal(string[] usernameArray)
        {
            string data = JsonSerializer.Serialize(usernameArray);
            File.WriteAllText(USERNAMES_FILE, data);
        }

        public static string LoadUsername(byte AgentID)
        {
            string[] Jsondata = JsonSerializer.Deserialize<string[]>(USERNAMES_FILE);

            string data = Jsondata[AgentID];
            return data;
        }
    }
}