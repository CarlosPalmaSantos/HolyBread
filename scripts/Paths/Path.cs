using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using HolyBread.scripts.battle;

namespace HolyBread.scripts.Paths;

[GlobalClass]
public partial class Path : Resource 
{
    [Export] public Spot[] Spots { get; set; } = [];
    [Export] public EnemyPreset[] Enemies { get; set; } = [];
    [Export] public int Position { get; set; } = -1;
    
    [Signal]
    public delegate void OnPathFinishedEventHandler();

    public Spot NextSpot()
    {
        Position++;
        if (Position >= Spots.Length)
        {
            EmitSignalOnPathFinished();
            return null;
        }
        
        var res = Spots[Position];
        return res;
    }

    public Enemy GetRandomEnemy() => Enemies[Random.Shared.Next(Enemies.Length)].GetRandom();
}