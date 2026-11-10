using Godot;
using HolyBread.scripts.battle;

namespace HolyBread.scripts;

public partial class SaveManager : Node
{
    [Export] public Player Player { get; set; } = new();
    public Entity Entity => Player.Entity;
}