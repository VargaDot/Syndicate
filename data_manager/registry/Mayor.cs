using System.Text.Json;
using System.IO;
using Godot;

namespace Registry
{
    public partial class Mayor : Node
    {
        [Signal]
        public delegate void DataSavedEventHandler();
        public void SaveGameData(string filename)
        {
            string json = JsonSerializer.Serialize(Daftar);
            File.WriteAllText(filename, json);
            EmitSignal(SignalName.DataSaved);
        }

        [Signal]
        public delegate void DataLoadedEventHandler();
        public void LoadGameData(string filename)
        {
            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                Daftar = JsonSerializer.Deserialize<List<Agent>>(json);
                EmitSignal(SignalName.DataLoaded);
            }
            else
            {
                GD.PrintErr("Invalid Save File");
            }
        }
    }
}