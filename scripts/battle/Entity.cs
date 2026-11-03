using System;
using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Entity : Resource
{
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }

    [Export] public DoughType DoughType { get; set; }

    [Export] public int CurrentHp { get; set; }
    [Export] public int Hp { get; set; }
    [Export] public int Atk { get; set; }
    [Export] public int Def { get; set; }
    [Export] public int Spd { get; set; }

    [Export] public Texture2D Texture { get; set; }
    [Export] public Attack[] AttackSet { get; set; }
    [Export] public Bread Bread { get; set; }

    public Attack GetAttack(int attack) => AttackSet.Length > attack ? AttackSet[attack] : null;
    public Attack GetBreadAttack(int attack) => Bread.AttackSet.Length > attack ? Bread.AttackSet[attack] : null;

    public void TakeDamage(float damage)
    {
        CurrentHp = Mathf.FloorToInt(Mathf.Clamp(CurrentHp - damage, 0, Hp));
    }
}