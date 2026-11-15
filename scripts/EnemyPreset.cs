using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using HolyBread.scripts.battle;

namespace HolyBread.scripts;

[GlobalClass]
public partial class EnemyPreset : Resource
{
    [Export] public int MinLevel { get; set; } = 1;
    [Export] public int MaxLevel { get; set; } = 1;

    [Export] public DoughType DoughType { get; set; }
    [Export] public NameSet Names { get; set; }
    [Export] public Texture2D[] Textures { get; set; } = [];
    [Export] public Attack[] Attacks { get; set; } = [];
    [Export] public Bread[] Breads { get; set; } = [];

    private T RandomElement<T>(T[] array) => array[Random.Shared.Next(array.Length)];

    private T RandomElement<T>(List<T> array) => array[Random.Shared.Next(array.Count)];

    public T[] PickRandomElements<T>(int n, T[] array)
    {
        var r = array.ToList();
        var res = new List<T>();

        for (var i = 0; i < n; i++)
        {
            if (r.Count == 0)
                break;

            var e = RandomElement(r);
            r.Remove(e);
            res.Add(e);
        }

        return res.ToArray();
    }

    public Enemy GetRandom() => new()
    {
        Name = Names.GetRandomName(),
        Level = Random.Shared.Next(MinLevel, MaxLevel + 1),
        DoughType = DoughType,
        Texture = RandomElement(Textures),
        AttackSet = PickRandomElements(3, Attacks),
        BreadSet = Breads.Length > 0 ? [RandomElement(Breads)] : [],
        
        BaseHp = 100,
        BaseAtk = 1,
        BaseDef = 1,
        BaseSpd = 1,
    };
}