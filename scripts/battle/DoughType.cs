using System.Linq;
using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class DoughType : Resource
{
    [Export] public string Name { get; set; } 
    [Export] public string Id { get; set; }
    [Export] public string[] Weak { get; set; }
    [Export] public string[] Resist { get; set; }

    public float GetMultiplier(DoughType other)
    {
        if (Weak.Contains(other.Id)) return 2;
        if (Resist.Contains(other.Id)) return 0.5f;
        return 1;
    }
}