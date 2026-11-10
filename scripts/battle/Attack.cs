using System.IO;
using Godot;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Attack : Resource, IBinary<Attack>
{
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }
    [Export] private DoughType DoughType { get; set; }
    [Export] public int Power { get; set; }

    public virtual DoughType GetDoughType(Entity user, Entity target) => DoughType;

    public virtual void Act(Entity user, Entity target, out Effectiveness effectiveness)
    {
        var type = GetDoughType(user, target);
        target.TakeDamage(target.DoughType.GetMultiplier(type, out effectiveness) * Power * user.Atk /
                          (target.Def * (target.DefensePlus ? 2 : 1)));
    }

    public enum Effectiveness
    {
        NONE,
        NORMAL,
        WEAK,
        RESIST
    }

    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(ResourceLoader.GetResourceUid(ResourcePath));
    }

    public static Attack ReadBinary(BinaryReader reader)
    {
        var uid = reader.ReadInt64();
        return GD.Load<Attack>($"uid://{uid}");
    }
}