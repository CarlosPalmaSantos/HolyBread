using System.IO;
using Godot;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Bread : Resource, IBinary<Bread>
{
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Attack Attack { get; set; }
    
    [Export] public int Durability { get; set; }
    [Export] public int MaxDurability { get; set; }

    public void Recover()
    {
        Durability = MaxDurability;
    }

    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(ResourceLoader.GetResourceUid(ResourcePath));
    }
    
    public static Bread ReadBinary(BinaryReader reader)
    {
        var uid = reader.ReadInt64();
        return GD.Load<Bread>($"uid://{uid}");
    }
}