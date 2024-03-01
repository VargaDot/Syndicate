using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Khana : Node
{
    private readonly List<Agent> Daftar = new();
    
    struct Agent
    {
        public byte ID {get; set;}
        public List<Property> Portfolio {get; set;}
        public int Cash = 2500;
        public string Name {get; set;}
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
        public byte ID {get; set;}
        public byte UpgradeLevel = 0;
        public bool IsMortgaged = false;
        public Property(byte id) 
        {
            ID = id;
        }
    }

    [Signal]
    public delegate void AgentAddedEventHandler(byte ID, string username);
    public void AddAgent(byte ID, string username)
    {
        Daftar.Add(new Agent(ID, username));
        EmitSignal(SignalName.AgentAdded, ID, username);
    }

    [Signal]
    public delegate void AgentRemovedEventHandler(byte ID);
    public void RemoveAgent(byte AgentID)
    {
        Daftar.Remove(FindAgent(AgentID));
        EmitSignal(SignalName.AgentRemoved, AgentID);
    }

    [Signal]
    public delegate void AgentMovedEventHandler(byte ID, byte newPosition);
    public void MoveAgent(byte AgentID, byte newPos)
    {
        Agent agent = FindAgent(AgentID);
        agent.Position = (byte)((agent.Position += newPos) % 39);
        EmitSignal(SignalName.AgentMoved, AgentID, newPos);
    }

    [Signal]
    public delegate void DoubleCountModifiedEventHandler(byte AgentID, bool is_even);
    public void ModifyDoubleCount(byte AgentID, bool is_even)
    {
        Agent agent = FindAgent(AgentID);
        if (is_even) agent.doublesCount++;
        else agent.doublesCount = 0;
        EmitSignal(SignalName.DoubleCountModified, AgentID, is_even);
    }

    [Signal]
    public delegate void AgentImprisonedEventHandler(byte ID);
    public void ToggleAgentFreedom(byte AgentID)
    {
        Agent agent = FindAgent(AgentID);
        agent.inPrison = !agent.inPrison;
        agent.Position = 10;
        EmitSignal(SignalName.AgentImprisoned, AgentID);
    }

    [Signal]
    public delegate void TransactionConductedEventHandler(byte ID, int amount);
    public void ConductTransaction(byte AgentID, int amount)
    {
        Agent agent = FindAgent(AgentID);
        agent.Cash += amount;
        EmitSignal(SignalName.TransactionConducted, AgentID, amount);
    }

    public void ExchangeLand(byte BuyerID, byte SellerID, byte PropertyID)
    {
        RemoveProperty(SellerID, PropertyID);
        AddProperty(BuyerID, PropertyID);
    }

    public byte AgentCount()
    { 
        return (byte)Daftar.Count;
    }

    public byte[] GetAgentIDs()
    {
        List<byte> allAgentIDs = new();

        foreach (var agentEntry in Daftar) 
        {
            allAgentIDs.Add(agentEntry.ID);
        }
        
        return allAgentIDs.ToArray();
    }

    public int GetAgentCash(byte AgentID)
    {
        return FindAgent(AgentID).Cash;
    }

    public string GetAgentName(byte AgentID)
    {
        return FindAgent(AgentID).Name;
    }

    public byte GetAgentPosition(byte AgentID)
    {
        return FindAgent(AgentID).Position;
    }

    public byte GetAgentDoublesCount(byte AgentID)
    {
        return FindAgent(AgentID).doublesCount; 
    }

    public bool GetAgentStatus(byte AgentID)
    {
        return FindAgent(AgentID).inPrison;
    }

    [Signal]
    public delegate void PropertyGrantedEventHandler(byte AgentID, byte PropID);
    public void AddProperty(byte AgentID, byte PropertyID)
    {
        Agent agent = FindAgent(AgentID);
        agent.Portfolio.Add(new Property(PropertyID));
        EmitSignal(SignalName.PropertyGranted, AgentID, PropertyID);
    }

    [Signal]
    public delegate void PropertyRemovedEventHandler(byte AgentID, byte PropID);
    public void RemoveProperty(byte AgentID, byte PropertyID)
    {
        Agent agent = FindAgent(AgentID);
        agent.Portfolio.RemoveAll(p => p.ID == PropertyID);
        EmitSignal(SignalName.PropertyRemoved, AgentID, PropertyID);
    }

    [Signal]
    public delegate void MortgageStatusChangedEventHandler(byte AgentID, byte PropID, bool MortgageStatus);
    public void ToggleMortgageStatus(byte AgentID, byte PropertyID)
    {
        Property property = FindProperty(AgentID, PropertyID);
        property.IsMortgaged = !property.IsMortgaged;
        EmitSignal(SignalName.MortgageStatusChanged, AgentID, PropertyID, property.IsMortgaged);
    }

    [Signal]
    public delegate void UpgradeLevelChangedEventHandler(byte AgentID, byte PropID, byte PropertyLevel);
    public void ModifyUpgradeLvl(byte AgentID, byte PropertyID, byte newLvl)
    {
        Property property = FindProperty(AgentID, PropertyID);
        property.UpgradeLevel = newLvl;
        EmitSignal(SignalName.UpgradeLevelChanged, AgentID, PropertyID, property.UpgradeLevel);
    }

    public byte CheckForOwnership(byte PropertyID)
    {
        foreach (Agent agent in Daftar)
        {
            if (agent.Portfolio.Any(property => property.ID == PropertyID))
            {
                return agent.ID;
            }
        }

        return 69; // Default if no owner was found
    }

    public byte GetUpgradeLevel(byte AgentID, byte PropertyID)
    {
        return FindProperty(AgentID, PropertyID).UpgradeLevel;
    }

    public bool GetMortgageStatus(byte AgentID, byte PropertyID)
    {
        return FindProperty(AgentID, PropertyID).IsMortgaged;
    }

    public byte[] GetAgentPortfolio(byte AgentID)
    {
        List<byte> propertyIDs = new();
        Agent agent = FindAgent(AgentID);

        foreach (Property property in agent.Portfolio)
        {
            propertyIDs.Add(property.ID);
        }

        return propertyIDs.ToArray();
    }

    private Agent FindAgent(byte AgentID)
    {
        return Daftar.Find(agent => agent.ID == AgentID);
    }

    private Property FindProperty(byte AgentID, byte PropertyID)
    {
        Agent agent = FindAgent(AgentID);
        return agent.Portfolio.Find(property => property.ID == PropertyID);
    }
}
