using System;
using System.IO;
using Godot;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Entity : Resource, IBinary<Entity>
{
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }

    [Export] public DoughType DoughType { get; set; }

    [Export] public int CurrentHp { get; set; }
    public float PercentualHP => (float)CurrentHp / Hp;
    [Export] public int Level { get; set; }

    [Export] private int BaseHp { get; set; }
    public int Hp => Mathf.FloorToInt(Mathf.Pow(DoughType.HpAff, Level) * BaseHp);

    [Export] private int BaseAtk { get; set; }
    public int Atk => Mathf.FloorToInt(Mathf.Pow(DoughType.AtkAff, Level) * BaseAtk);

    [Export] private int BaseDef { get; set; }
    public int Def => Mathf.FloorToInt(Mathf.Pow(DoughType.DefAff, Level) * BaseDef);

    [Export] private int BaseSpd { get; set; }
    public int Spd => Mathf.FloorToInt(Mathf.Pow(DoughType.SpdAff, Level) * BaseSpd);

    [Export] public Texture2D Texture { get; set; }
    [Export] public Attack[] AttackSet { get; set; }
    [Export] public Bread[] BreadList { get; set; }

    public Attack GetAttack(int attack) => AttackSet.Length > attack ? AttackSet[attack] : null;
    public Attack GetBreadAttack(int attack) => BreadList.Length > attack ? BreadList[attack].Attack : null;

    public void TakeDamage(float damage)
    {
        CurrentHp = Mathf.FloorToInt(Mathf.Clamp(CurrentHp - damage, 0, Hp));
        GD.Print($"Current: {CurrentHp}, Damage: {damage}");
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
        writer.Write(BreadList);
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
            BreadList = reader.ReadArray<Bread>(),
        };
    }
}