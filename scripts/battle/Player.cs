using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Player : Resource, IBinary<Player>
{
    [Export] private Entity[] doughVariantList = [];
    [Export] public Bread[] BreadSet { get; set; } = [];
    private Dictionary<DoughType, Entity> doughVariants;

    private Dictionary<DoughType, Entity> DoughVariants
    {
        get
        {
            doughVariants ??= doughVariantList.ToDictionary(d => d.DoughType);
            return doughVariants;
        }
    }

    [Export] public DoughType DoughType { get; set; }
    public Entity Entity
    {
        get
        {
            var res = DoughVariants[DoughType];
            res.BreadSet = BreadSet; // No es lo mas óptimo, pero se va a quedar así
            return res;
        }
    }

    public void ChangeDoughType(DoughType doughType)
    {
        DoughType = doughType;
    }

    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(DoughType);
        writer.Write(doughVariantList);
        writer.Write(BreadSet);
    }

    public static Player ReadBinary(BinaryReader reader) => new()
    {
        DoughType = reader.Read<DoughType>(),
        doughVariants = null,
        doughVariantList = reader.ReadArray<Entity>(),
        BreadSet = reader.ReadArray<Bread>()
    };
}