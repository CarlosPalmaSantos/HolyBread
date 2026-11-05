using System;
using System.Collections.Generic;
using System.IO;

namespace HolyBread.scripts.utiles;

public interface IBinary
{
    public void WriteBinary(BinaryWriter writer);
}

public interface IBinary<T> : IBinary
{
    public static abstract T ReadBinary(BinaryReader reader);
}

public static class Binary
{
    public static void WriteBinary<T>(this T[] array, BinaryWriter writer) where T : IBinary
    {
        writer.Write(array.Length);
        foreach (var item in array)
            item.WriteBinary(writer);
    }

    public static void WriteBinary<TK, TV>(this Dictionary<TK, TV> array, BinaryWriter writer)
        where TK : IBinary where TV : IBinary
    {
        writer.Write(array.Keys.Count);
        foreach (var item in array)
        {
            item.Key.WriteBinary(writer);
            item.Value.WriteBinary(writer);
        }
    }
    
    
    public static T[] ReadArray<T>(BinaryReader reader) where T : IBinary<T>
    {
        int length = reader.ReadInt32();
        T[] array = new T[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = T.ReadBinary(reader);
        }
        return array;
    }

    public static Dictionary<TK, TV> ReadDictionary<TK, TV>(BinaryReader reader)
        where TK : IBinary<TK> 
        where TV : IBinary<TV>
    {
        int count = reader.ReadInt32();
        var dictionary = new Dictionary<TK, TV>(count);
        for (int i = 0; i < count; i++)
        {
            TK key = TK.ReadBinary(reader);
            TV value = TV.ReadBinary(reader);
            dictionary.Add(key, value);
        }
        return dictionary;
    }
}