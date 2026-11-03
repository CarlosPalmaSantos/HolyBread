namespace HolyBread.scripts.battle.Attacks;

public partial class SameTypeAttack : Attack
{
    public override DoughType GetDoughType(Entity user, Entity target) => user.DoughType;
}