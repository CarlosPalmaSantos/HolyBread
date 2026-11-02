using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Bread : Resource
{
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Attack[] Attacks { get; set; }
}