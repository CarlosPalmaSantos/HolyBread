using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Bread : Resource
{
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Attack[] AttackSet { get; set; }
    
    [Export] public int Durability { get; set; }
    [Export] public int MaxDurability { get; set; }

    public void Recover()
    {
        Durability = MaxDurability;
    }
}