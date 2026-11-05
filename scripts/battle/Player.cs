using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

// TODO: Agregar IBinary
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
        DoughVariants.WriteBinary(writer);
        DoughType.WriteBinary(writer);
    }

    public static Player ReadBinary(BinaryReader reader) => new()
    {
        DoughVariants = Binary.ReadDictionary<DoughType, Entity>(reader),
        DoughType = DoughType.ReadBinary(reader),
    };
}