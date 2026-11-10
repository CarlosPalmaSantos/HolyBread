using Godot;

namespace HolyBread.scripts.battle.Attacks;

[GlobalClass]
public partial class HealOnlyAttack: Attack
{
    [Export] public float Percentage;
    public override void Act(Entity user, Entity target, out Effectiveness effectiveness)
    {
        user.CurrentHp = Mathf.Clamp(user.CurrentHp + Mathf.CeilToInt(user.Hp * Percentage), 0, user.Hp);
        effectiveness = Effectiveness.NONE;
    }
}