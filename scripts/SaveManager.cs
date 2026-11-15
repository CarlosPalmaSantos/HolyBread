using Godot;
using HolyBread.scripts.battle;
using HolyBread.scripts.dialogues;

namespace HolyBread.scripts;

public partial class SaveManager : Node
{
    [Export] public Player Player { get; set; } = new();
    public Entity Entity => Player.Entity;

    public void LevelUp() => Entity.LevelUp();
}