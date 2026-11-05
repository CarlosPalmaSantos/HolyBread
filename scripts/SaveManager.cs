using Godot;
using HolyBread.scripts.battle;

namespace HolyBread.scripts;

public partial class SaveManager : Node
{
	public Player Player { get; } = new();
	public Entity Entity => Player.Entity;
	
}