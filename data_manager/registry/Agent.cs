using System.Collections.Generic;
using Godot;

namespace Registry
{
    public partial class Agent : Resource
    {
        public byte ID {get; set;}
        public List<Property> Portfolio {get; set;}
        public int Cash {get; set;} = 2500;
        public string Name {get; set;}
        public byte Position {get; set;} = 0;
        public bool InPrison {get; set;} = false;
        public byte DoublesCount {get; set;} = 0;

        public Agent(byte id, string name)
        {
            ID = id;
            Name = name;
            Portfolio = new();
        }

        public void UpdateCash(int amount)
        {
            Cash += amount;
        }

        public void UpdatePosition(byte newPos)
        {
            Position = newPos;
        }

        public void UpdateDoublesCount(bool is_even)
        {
            if (is_even) DoublesCount++;
            else DoublesCount = 0;
        }

        public void UpdatePrisonStatus()
        {
            InPrison = !InPrison;
        }

        public void AddProperty(byte PropertyID)
        {
            Portfolio.Add(new Property(PropertyID));
        }

        public void RemoveProperty(byte PropertyID)
        {
            Portfolio.RemoveAll(p => p.ID == PropertyID);
        }
    }
}