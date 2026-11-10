using System;
using Godot;

namespace HolyBread.scripts;

[GlobalClass]
public partial class NameSet : Resource
{
    [Export] public string[] Names { get; set; } = [];
    public string GetRandomName() => Names[Random.Shared.Next(Names.Length)];
}