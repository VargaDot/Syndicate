using System.Collections.Generic;
using Godot;

public partial class Khana : Node
{
  struct Agent
  {
    public byte ID {get; set;}
    public List<Property> Portfolio { get; set; }
    public int Cash {get; set;}
    public string Name {get; set;}

    public Agent(byte id, int cash, string name)
    {
      ID = id;
      Cash = cash;
      Name = name;
      Portfolio = new ();
    }
  }

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
  public static void AddAgent(byte ID, ushort startingCash, string username)
  {
    root[ID] = new Agent(ID, startingCash, username);
  }

  public static void RemoveAgent(byte AgentID) 
  { 
    CheckIDValidity(AgentID);
    root.Remove(AgentID); 
  }

  public static void CheckIDValidity(byte AgentID)
  {
    if (!root.ContainsKey(AgentID)) GD.PushError("Agent ID is not valid");
  }

  public static void ConductTransaction(byte AgentID, short amount)
  {
    CheckIDValidity(AgentID);
    Agent agent = root[AgentID]; agent.Cash += amount;
  }

  public static int GetAgentCash(byte AgentID)
  {
    CheckIDValidity(AgentID);
    return root[AgentID].Cash;
  }

  public static void AddProperty(byte AgentID, byte PropertyID, byte upgradeLevel = 1, bool isMortgaged = false)
  {
    CheckIDValidity(AgentID);
    Agent agent = root[AgentID];
    agent.Portfolio.Add(new Property(PropertyID, upgradeLevel, isMortgaged));
  }

  public static void RemoveProperty(byte AgentID, byte PropertyID)
  {
    CheckIDValidity(AgentID);

    Agent agent = root[AgentID];
    Property propertyToRemove = agent.Portfolio.Find(p => p.ID == PropertyID);
    
    agent.Portfolio.Remove(propertyToRemove);
  }

  public static byte[] GetAgentPortfolio(byte AgentID)
  {
    CheckIDValidity(AgentID);

    List<byte> propertyIDs = new();
    foreach (Property property in root[AgentID].Portfolio) { propertyIDs.Add(property.ID);}

    return propertyIDs.ToArray();
  }

  public static void ToggleMortgageStatus(byte AgentID, byte PropertyID, bool newStatus)
  {
    CheckIDValidity(AgentID);

    Agent agent = root[AgentID];
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);

    property.IsMortgaged = newStatus;
  }

  public static void ModifyUpgradeLvl(byte AgentID, byte PropertyID, byte newLvl)
  {
    CheckIDValidity(AgentID);

    Agent agent = root[AgentID];
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);

    property.UpgradeLevel = newLvl;
  }
}
