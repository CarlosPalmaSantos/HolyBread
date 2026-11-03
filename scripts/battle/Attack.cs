using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Attack : Resource
{
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }
    [Export] private DoughType DoughType { get; set; }
    [Export] public int Power { get; set; }

    public virtual DoughType GetDoughType(Entity user, Entity target) => DoughType;

    public virtual void Act(Entity user, Entity target, out Effectiveness effectiveness)
    {
        var type = GetDoughType(user, target);
        target.TakeDamage(target.DoughType.GetMultiplier(type, out effectiveness) * Power * user.Atk / target.Def);
    }

    public enum Effectiveness
    {
        NORMAL,
        WEAK,
        RESIST
    }
}