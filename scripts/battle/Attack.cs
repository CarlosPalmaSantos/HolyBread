using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Attack : Resource
{
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }
    [Export] public DoughType DoughType { get; set; }
    [Export] public int Power { get; set; }

    public virtual void Act(Entity user, Entity target)
    {
        target.TakeDamage((float)Power * user.Atk / target.Def);
    }
}