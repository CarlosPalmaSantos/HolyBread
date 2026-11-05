using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

public class Player : IBinary<Player>
{
    private Dictionary<DoughType, Entity> DoughVariants { get; init; } = [];
    public DoughType DoughType { get; set; }
    public Entity Entity => DoughVariants[DoughType];

    public void ChangeDoughType(DoughType doughType)
    {
        DoughType = doughType;
    }

    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(DoughType);
        DoughType.WriteBinary(writer);
    }

    public static Player ReadBinary(BinaryReader reader) => new()
    {
        DoughVariants = reader.ReadDictionary<DoughType, Entity>(),
        DoughType = reader.Read<DoughType>(),
    };
}