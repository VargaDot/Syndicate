using Godot;

namespace Registry
{
    public partial class Property : Resource
    {
        public byte ID {get; set;}
        public byte UpgradeLevel = 0;
        public bool IsMortgaged = false;
        public Property(byte id) 
        {
            ID = id;
        }

        public void UpdateMortgageStatus()
        {
            IsMortgaged = !IsMortgaged;
        }

        public void UpdateUpgradeLevel(byte newLvl)
        {
            UpgradeLevel = newLvl;
        }
    }
}