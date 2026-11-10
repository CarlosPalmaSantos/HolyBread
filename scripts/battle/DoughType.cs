using System.IO;
using System.Linq;
using Godot;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class DoughType : Resource, IBinary<DoughType>
{
    [Export] public string Name { get; set; }
    [Export] public string Id { get; set; }
    [Export] public string[] Weak { get; set; } = [];
    [Export] public string[] Resist { get; set; } = [];

    [Export] public float HpAff { get; set; } = 1.25f;
    [Export] public float AtkAff { get; set; } = 1.25f;
    [Export] public float DefAff { get; set; } = 1.25f;
    [Export] public float SpdAff { get; set; } = 1.25f;

    public float GetMultiplier(DoughType other, out Attack.Effectiveness effectiveness)
    {
        effectiveness = Attack.Effectiveness.NORMAL;
        if (other == null) return 1;

        if (Weak.Contains(other.Id))
        {
            effectiveness = Attack.Effectiveness.WEAK;
            return 2;
        }

        if (Resist.Contains(other.Id))
        {
            effectiveness = Attack.Effectiveness.RESIST;
            return 0.5f;
        }
        
        return 1;
    }

    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(Id);
    }

    public static DoughType ReadBinary(BinaryReader reader)
    {
        var uid = reader.ReadInt64();
        return GD.Load<DoughType>($"uid://${uid}");
    }
}