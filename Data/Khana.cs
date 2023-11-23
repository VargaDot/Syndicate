using System.Collections.Generic;
using System.Linq;
using Godot;

[GlobalClass]
public partial class Khana : Node
{
  struct Agent
  {
    public byte ID { get; set; }
    public List<Property> Portfolio { get; set; }
    public int Cash { get; set; }
    public string Name { get; set; }

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
    public byte ID { get; set; }
    public byte UpgradeLevel { get; set; }
    public bool IsMortgaged { get; set; }

    public Property(byte id, byte upgradeLevel, bool isMortgaged = false)
    {
      ID = id;
      UpgradeLevel = upgradeLevel;
      IsMortgaged = isMortgaged;
    }
  }
  
  private static readonly List<Agent> Daftar = new();
  public static void AddAgent(byte ID, ushort startingCash, string username)
  {
    Daftar.Add(new Agent(ID, startingCash, username));
  }

  public static void RemoveAgent(byte AgentID) 
  {
    Agent agentToRemove = Daftar.Find(agent => agent.ID == AgentID);
    Daftar.Remove(agentToRemove);
  }

  public static void ConductTransaction(byte AgentID, short amount)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    selectedAgent.Cash += amount;
  }

  public static void ExchangeLand(byte BuyerID, byte SellerID, byte PropertyID)
  {
    RemoveProperty(SellerID, PropertyID);
    AddProperty(BuyerID, PropertyID);
  }

  public static int GetAgentCash(byte AgentID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    return selectedAgent.Cash;
  }

  public static string GetAgentName(byte AgentID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    return selectedAgent.Name;
  }

  public static void AddProperty(byte AgentID, byte PropertyID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    selectedAgent.Portfolio.Add(new Property(PropertyID, 0));
  }

  public static void RemoveProperty(byte AgentID, byte PropertyID)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property propertyToRemove = agent.Portfolio.Find(p => p.ID == PropertyID);
    agent.Portfolio.Remove(propertyToRemove);
  }

  public static byte CheckForOwnership(byte PropertyID)
  {
    foreach (Agent agent in Daftar) if (agent.Portfolio.Any(property => property.ID == PropertyID)) { return agent.ID; }
    
    // Default if no owner was found
    return 69;
  }

  public static void ToggleMortgageStatus(byte AgentID, byte PropertyID, bool newStatus)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);

    property.IsMortgaged = newStatus;
  }

  public static void ModifyUpgradeLvl(byte AgentID, byte PropertyID, byte newLvl)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);

    property.UpgradeLevel = newLvl;
  }
  
  public static byte[] GetAgentPortfolio(byte AgentID)
  {
    List<byte> propertyIDs = new();
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);

    foreach (Property property in agent.Portfolio) { propertyIDs.Add(property.ID);}
    return propertyIDs.ToArray();
  }
}
