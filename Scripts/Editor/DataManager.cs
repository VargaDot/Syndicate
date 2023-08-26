using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Godot;

///Namespace for loading and saving data, if you're seeing this then intellisense wise, everything's fine
namespace DataManager
{
    public static class BoardLoader
    {
        const string BOARD_PATH = "Data/Board.JSON";
        static readonly string BOARD_FILE = File.ReadAllText(BOARD_PATH, Encoding.UTF8);
        readonly static Dictionary<string, object>[] BoardData = JsonSerializer.Deserialize<Dictionary<string, object>[]>(BOARD_FILE);
        
        /// <summary>
        /// <list type = "number">
        /// <item><description><para><em> Property Type </em></para></description></item>
        /// <item><description><para><em> CardID </em></para></description></item>
        /// </list>
        /// </summary>
        /// <returns> byte </returns>
        public static byte LoadTileNumData(int BoardID, byte functionID)
        {  
            byte value = 0;
            switch (BoardID)
            {
                case 1:
                    value = (byte)BoardData[BoardID]["Type"];
                    break;
                case 2:
                    value = (byte)BoardData[BoardID]["CardID"];
                    break;
                default:
                    GD.PrintErr("Invalid Operation");
                    break;
            }

            return value;
        }
        
        /// <summary>
        /// <list type = "number">
        /// <item><description><para><em> Property Name </em></para></description></item>
        /// <item><description><para><em> InternalPropertyName </em></para></description></item>
        /// </list>
        /// </summary>
        /// <returns> string </returns>
        public static string LoadTileStrData(int BoardID, byte functionID)
        {
            string value = "";

            switch (BoardID)
            {
                case 1:
                    value = (string)BoardData[BoardID]["Name"];
                    break;
                case 2:
                    value = (string)BoardData[BoardID]["ID"];
                    break;
                default:
                    GD.PrintErr("Invalid Operation");
                    break;
            }

            return value;
        }
    }

    ///<summary> Class for requesting data from Properties.JSON </summary>
    public static class PropertyLoader
    {
        const string PROPERTY_PATH = "Data/Properties.JSON";
        readonly static string PROPERTY_FILE = File.ReadAllText(PROPERTY_PATH, Encoding.UTF8);

        // This whole portion here is just defining what and how to read the JSON file's structures
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
            if(!CostData.ContainsKey(internalPropName))
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
            if(!RentData.ContainsKey(internalPropName))
                GD.PushError("Invalid internal property name");

            switch (ID)
            {
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



    }

    ///<summary> The data saver, sorter and search engine for the board's registry </summary>
    public static class TheRegistry
    {
        // This entire part down here is for the tree data structure!
        ///<summary> The Agent class stores an ID and the property struct (OwnedProperties) </summary>
        struct Agent
        {
            public byte ID {get; set;}
            public Dictionary<byte, Property> OwnedProperties {get; set;}
            public int Cash {get; set;}
            public bool inPrison = false;
            public string Name {get; set;}
            public byte position = 0;

            public Agent(byte id, int cash, string name)
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

        ///<summary> root is the Agent tree node, from here we can access the entire tree structure </summary>
        private static readonly Dictionary<byte, Agent> root = new();
        public static void AddAgents(byte ID, ushort startingCash, string username)
        {
            if(!root.ContainsKey(ID))
                root[ID] = new Agent(ID, startingCash, username);
            else
                GD.Print("Username overrided");
        }

        public static void RemoveAgent(byte AgentID)
        {
            root.Remove(AgentID);
        }

        public static void LoadAgents(Godot.Collections.Dictionary<byte, byte> AgentList = null)
        {
            foreach(Agent Agent in root.Values)
            {
                AgentList.Add(Agent.ID, Agent.position);

                GD.Print(Agent.ID, Agent.Name);
            }
        }

        public static void ChangeAgentPosition(byte AgentID, byte diceRoll)
        {
            Agent agent = root[AgentID];
            agent.position += diceRoll;
        }

        public static void ConductTransaction(byte agentID, int amount)
        {
            Agent agent = root[agentID];
            agent.Cash += amount;
        }

        public static void AddProperty(byte AgentID, byte PropertyID, byte upgradeLevel = 1, bool isMortgaged = false)
        {
            if(!root.ContainsKey(AgentID))
                GD.PushError("Agent ID is not valid");

            Agent agent = root[AgentID];

            agent.OwnedProperties[PropertyID] = new Property(PropertyID, upgradeLevel, isMortgaged);
        }

        public static void RemoveProperty(byte agentID, byte PropertyID)
        {
            Agent agent = root[agentID];

            if(!root.ContainsKey(agentID))
                GD.PushError("Invalid agentID");

            if(!agent.OwnedProperties.ContainsKey(PropertyID))
                GD.PushError("Invalid PropertyID");

            root[agentID].OwnedProperties.Remove(PropertyID);
        }

        public static void UpdatePropertyLevel(byte AgentID, byte PropertyID, byte newUpgradeLevel)
        {
            Agent agent = root[AgentID];
            Property property = agent.OwnedProperties[PropertyID];

            if(!root.ContainsKey(AgentID))
                GD.PushError("Invalid AgentID");

            if(!agent.OwnedProperties.ContainsKey(PropertyID))
                GD.PushError("Invalid PropertyID");

            property.UpgradeLevel = newUpgradeLevel;
        }

        public static void UpdateMortgageStatus(byte AgentID, byte PropertyID, bool newMortgageStatus)
        {
            Agent agent = root[AgentID];
            Property property = root[AgentID].OwnedProperties[PropertyID];

            if(!root.ContainsKey(AgentID))
                GD.PushError("Invalid AgentID");

            if(!agent.OwnedProperties.ContainsKey(PropertyID))
                GD.PushError("Invalid PropertyID");

            property.IsMortgaged = newMortgageStatus;
        }

        ///<summary> Returns an array with a agent's owned properties </summary>
        ///<returns> Array<byte> </returns>
        public static byte[] GetAgentOwnedProperties(byte AgentID)
        {
            if(!root.ContainsKey(AgentID))
                GD.PushError("Invalid AgentID");

            byte[] data = new byte[root[AgentID].OwnedProperties.Count];
            return data;
        }

        

    }


}