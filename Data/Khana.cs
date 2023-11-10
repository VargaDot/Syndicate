using System.Collections.Generic;
using Godot;

// Based on the tree data structure
public partial class Khana : Node
{
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

  ///<summary> root is the Agent tree node, from here we can access the entire tree structure </summary>
  private static readonly Dictionary<byte, Agent> root = new();
  public static void AddAgents(byte ID, ushort startingCash, string username)
  {
    if (!root.ContainsKey(ID)) root[ID] = new Agent(ID, startingCash, username);
    else GD.Print("Username overrided");
  }

  public static void RemoveAgent(byte AgentID) { root.Remove(AgentID); }

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
    if(!root.ContainsKey(AgentID)) GD.PushError("Agent ID is not valid");

    Agent agent = root[AgentID];
    agent.OwnedProperties[PropertyID] = new Property(PropertyID, upgradeLevel, isMortgaged);
  }

  public static void RemoveProperty(byte agentID, byte PropertyID)
  {
    Agent agent = root[agentID];

    if(!root.ContainsKey(agentID)) GD.PushError("Invalid agentID");
    if(!agent.OwnedProperties.ContainsKey(PropertyID)) GD.PushError("Invalid PropertyID");

    root[agentID].OwnedProperties.Remove(PropertyID);
  }

  public static void UpdatePropertyLevel(byte AgentID, byte PropertyID, byte newUpgradeLevel)
  {
    Agent agent = root[AgentID];
    Property property = agent.OwnedProperties[PropertyID];

    if(!root.ContainsKey(AgentID)) GD.PushError("Invalid AgentID");
    if(!agent.OwnedProperties.ContainsKey(PropertyID)) GD.PushError("Invalid PropertyID");

    property.UpgradeLevel = newUpgradeLevel;
  }

  public static void UpdateMortgageStatus(byte AgentID, byte PropertyID, bool newMortgageStatus)
  {
    Agent agent = root[AgentID];
    Property property = root[AgentID].OwnedProperties[PropertyID];

    if(!root.ContainsKey(AgentID)) GD.PushError("Invalid AgentID");
    if(!agent.OwnedProperties.ContainsKey(PropertyID)) GD.PushError("Invalid PropertyID");

    property.IsMortgaged = newMortgageStatus;
  }

  ///<summary> Returns an array with a agent's owned properties </summary>
  ///<returns> Array<byte> </returns>
  public static byte[] GetAgentOwnedProperties(byte AgentID)
  {
    if(!root.ContainsKey(AgentID)) GD.PushError("Invalid AgentID");

    byte[] data = new byte[root[AgentID].OwnedProperties.Count];
    return data;
  }
}
