using Godot;

namespace HolyBread.scripts.battle.Attacks;

[GlobalClass]
public partial class HalfLifeAttack : Attack
{
	public override DoughType GetDoughType(Entity user, Entity target)
	{
		return user.DoughType;
	}

	public override void Act(Entity user, Entity target, out Effectiveness effectiveness)
	{
		effectiveness = Effectiveness.NORMAL;
		target.TakeDamage(target.Hp / 2);
	}
}