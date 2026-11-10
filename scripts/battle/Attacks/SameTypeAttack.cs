using Godot;

namespace HolyBread.scripts.battle.Attacks;

[GlobalClass]
public partial class SameTypeAttack : Attack
{
    public override DoughType GetDoughType(Entity user, Entity target) => user.DoughType;
}