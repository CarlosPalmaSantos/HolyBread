using System;
using System.IO;
using Godot;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Entity : Resource, IBinary<Entity>
{
    public Entity()
    {
    }

    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }

    [Export] public DoughType DoughType { get; set; }

    [Export] public int CurrentHp { get; set; } = -1;
    public float PercentualHp => (float)CurrentHp / Hp;
    [Export] public int Level { get; set; }
    [Export] public int XP { get; set; }
    [Export] public int LevelUpXP { get; set; }


    [Export] public int BaseHp { private get; set; } = 100;
    public int Hp => Mathf.FloorToInt(Mathf.Pow(DoughType.HpAff, Level - 1) * BaseHp);

    [Export] public int BaseAtk { private get; set; } = 1;
    public int Atk => Mathf.CeilToInt(Mathf.Pow(DoughType.AtkAff, Level - 1)) * BaseAtk;

    [Export] public int BaseDef { private get; set; } = 1;
    public int Def => Mathf.CeilToInt(Mathf.Pow(DoughType.DefAff, Level - 1)) * BaseDef;

    [Export] public int BaseSpd { private get; set; } = 1;
    public int Spd => Mathf.CeilToInt(Mathf.Pow(DoughType.SpdAff, Level - 1)) * BaseSpd;

    [Export] public bool DefensePlus { get; set; }

    [Export] public Texture2D Texture { get; set; }
    [Export] public Attack[] AttackSet { get; set; }
    [Export] public Bread[] BreadSet { get; set; }

    public Attack GetAttack(int attack) => AttackSet.Length > attack ? AttackSet[attack] : null;
    public Attack GetBreadAttack(int attack) => BreadSet.Length > attack ? BreadSet[attack].Attack : null;

    public void TakeDamage(float damage)
    {
        CurrentHp = Mathf.FloorToInt(Mathf.Clamp(CurrentHp - damage, 0, Hp));
    }

    public void Recover()
    {
        CurrentHp = Hp;
        foreach (var bread in BreadSet)
            bread.Recover();
    }

    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(Name);
        writer.Write(Description);
        writer.Write(DoughType);
        writer.Write(CurrentHp);
        writer.Write(Hp);
        writer.Write(Atk);
        writer.Write(Def);
        writer.Write(Spd);

        writer.Write(AttackSet);
        writer.Write(BreadSet);
    }

    public static Entity ReadBinary(BinaryReader reader)
    {
        return new()
        {
            Name = reader.ReadString(),
            Description = reader.ReadString(),
            DoughType = DoughType.ReadBinary(reader),
            CurrentHp = reader.ReadInt32(),

            BaseHp = reader.ReadInt32(),
            BaseAtk = reader.ReadInt32(),
            BaseDef = reader.ReadInt32(),
            BaseSpd = reader.ReadInt32(),

            AttackSet = reader.ReadArray<Attack>(),
            BreadSet = reader.ReadArray<Bread>(),
        };
    }
}