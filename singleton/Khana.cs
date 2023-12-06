using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Khana : Node
{
  struct Agent
  {
    public byte ID { get; set; }
    public List<Property> Portfolio { get; set; }
    public int Cash = 2500;
    public string Name { get; set; }
    public byte Position = 0;
    public bool inPrison = false;
    public byte doublesCount = 0;

    public Agent(byte id, string name)
    {
      ID = id;
      Name = name;
      Portfolio = new ();
    }
  }

  struct Property
  {
    public byte ID { get; set; }
    public byte UpgradeLevel = 0;
    public bool IsMortgaged = false;

    public Property(byte id) { ID = id; }
  }
  
  private readonly List<Agent> Daftar = new();
  public void AddAgent(byte ID, string username)
  {
    Daftar.Add(new Agent(ID, username));
  }

  public void RemoveAgent(byte AgentID) 
  {
    Agent agentToRemove = Daftar.Find(agent => agent.ID == AgentID);
    Daftar.Remove(agentToRemove);
  }

  public void MoveAgent(byte AgentID, byte newPos)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    agent.Position = (byte)((agent.Position += newPos) % 39);
  }

  public void ModifyDoubleCount(byte AgentID, bool is_even)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    if (is_even) selectedAgent.doublesCount++;
    else selectedAgent.doublesCount = 0;
  }

  public void ToggleAgentFreedom(byte AgentID)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    agent.inPrison = !agent.inPrison;
    agent.Position = 10;
  }

  public void ConductTransaction(byte AgentID, int amount)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    selectedAgent.Cash += amount;
  }

  public void ExchangeLand(byte BuyerID, byte SellerID, byte PropertyID)
  {
    RemoveProperty(SellerID, PropertyID);
    AddProperty(BuyerID, PropertyID);
  }

  public byte AgentCount() { return (byte)Daftar.Count; }

  public byte[] GetAgentIDs()
  {
    List<byte> allAgentIDs = new();
    
    foreach (var agentEntry in Daftar) { allAgentIDs.Add(agentEntry.ID); }
    return allAgentIDs.ToArray();
  }

  public int GetAgentCash(byte AgentID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    return selectedAgent.Cash;
  }

  public string GetAgentName(byte AgentID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    return selectedAgent.Name;
  }

  public byte GetAgentPosition(byte AgentID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    return selectedAgent.Position;
  }

  public byte GetAgentDoublesCount(byte AgentID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    return selectedAgent.doublesCount; 
  }

  public bool GetAgentStatus(byte AgentID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    return selectedAgent.inPrison;
  }

  public void AddProperty(byte AgentID, byte PropertyID)
  {
    Agent selectedAgent = Daftar.Find(agent => agent.ID == AgentID);
    selectedAgent.Portfolio.Add(new Property(PropertyID));
  }

  public void RemoveProperty(byte AgentID, byte PropertyID)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property propertyToRemove = agent.Portfolio.Find(p => p.ID == PropertyID);
    agent.Portfolio.Remove(propertyToRemove);
  }

  public byte GetUpgradeLevel(byte AgentID, byte PropertyID)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);
    return property.UpgradeLevel;
  }

  public bool GetMortgageStatus(byte AgentID, byte PropertyID)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);
    return property.IsMortgaged;
  }

  public byte CheckForOwnership(byte PropertyID)
  {
    foreach (Agent agent in Daftar) if (agent.Portfolio.Any(property => property.ID == PropertyID)) { return agent.ID; }
    return 69; // Default if no owner was found
  }

  public void ToggleMortgageStatus(byte AgentID, byte PropertyID)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);

    property.IsMortgaged = !property.IsMortgaged;
  }

  public void ModifyUpgradeLvl(byte AgentID, byte PropertyID, byte newLvl)
  {
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);
    Property property = agent.Portfolio.Find(p => p.ID == PropertyID);

    property.UpgradeLevel = newLvl;
  }
  
  public byte[] GetAgentPortfolio(byte AgentID)
  {
    List<byte> propertyIDs = new();
    Agent agent = Daftar.Find(agent => agent.ID == AgentID);

    foreach (Property property in agent.Portfolio) { propertyIDs.Add(property.ID);}
    return propertyIDs.ToArray();
  }
}
