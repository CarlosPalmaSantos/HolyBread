using Godot;

namespace HolyBread.scripts.Paths;

[GlobalClass]
public partial class Spot : Resource
{
    [Export] public float BattleChance { get; set; }
    [Export] public float HardBattleChance { get; set; }
    [Export] public float TreasureChance { get; set; }
    [Export] public float UniqueSpot { get; set; }
}