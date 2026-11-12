using System.Collections.Generic;
using System.Linq;
using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Enemy : Entity
{
    public List<DoughType> Posibles { get; set; }

    public void loadPosibles()
    {
        if (Posibles != null) return;

        Posibles = [];
        using var dir = DirAccess.Open("res://resources/Doughs");
        
        dir.ListDirBegin();
        var filename = dir.GetNext();

        while (!string.IsNullOrEmpty(filename))
        {
            if (!dir.CurrentIsDir() && filename.EndsWith(".tres"))
            {
                var resource = GD.Load<DoughType>("res://resources/Doughs/" + filename);
                if (resource != null)
                    Posibles.Add(resource);
            }

            filename = dir.GetNext();
        }
    }

    public (ActionType, int) GetEnemyAction(BattleManager battle)
    {
        loadPosibles();
        var attack = GetBestAttack(battle);
        battle.Player.DoughType.GetMultiplier(attack.GetDoughType(this, battle.Player), out var effValidation);

        foreach (var pos in Posibles.ToArray())
        {
            pos.GetMultiplier(attack.GetDoughType(this, battle.Player), out var eff);
            if (eff != effValidation) Posibles.Remove(pos);
        }

        return (ActionType.ATTACK, AttackSet.ToList().IndexOf(attack));
    }

    public Attack GetBestAttack(BattleManager battle)
    {
        (Attack, float) max = (null, -1);

        foreach (var atk in AttackSet)
        {
            var maxMultiplier = Posibles.Max(p => p.GetMultiplier(atk.GetDoughType(this, battle.Player), out _));
            var val = maxMultiplier * atk.Power;

            if (max.Item2 < val)
            {
                max = (atk, val);
            } 
        }

        return max.Item1;
    }
}