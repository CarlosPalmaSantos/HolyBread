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
        if (dir == null)
        {
            GD.PushError("No se pudo abrir res://resources/Doughs");
            return;
        }

        dir.ListDirBegin();
        var filename = dir.GetNext();

        while (!string.IsNullOrEmpty(filename))
        {
            if (!dir.CurrentIsDir())
            {
                // En el exportado los recursos aparecen como "X.tres.remap"
                if (filename.EndsWith(".remap"))
                    filename = filename.TrimSuffix(".remap");

                if (filename.EndsWith(".tres"))
                {
                    var resource = GD.Load<DoughType>("res://resources/Doughs/" + filename);
                    if (resource != null)
                        Posibles.Add(resource);
                }
            }

            filename = dir.GetNext();
        }

        dir.ListDirEnd();
    }

    public virtual (ActionType, int) GetEnemyAction(BattleManager battle)
    {
        loadPosibles();
        var attack = GetBestAttack(battle);
        battle.PlayerEntity.DoughType.GetMultiplier(attack.GetDoughType(this, battle.PlayerEntity),
            out var effValidation);

        foreach (var pos in Posibles.ToArray())
        {
            pos.GetMultiplier(attack.GetDoughType(this, battle.PlayerEntity), out var eff);
            if (eff != effValidation) Posibles.Remove(pos);
        }

        GD.Print("AYUDA");
        GD.Print(AttackSet.Length);

        return (ActionType.ATTACK, AttackSet.ToList().IndexOf(attack));
    }

    public virtual Attack[] ValidAttackSet()
    {
        return AttackSet;
    }

    public Attack GetBestAttack(BattleManager battle)
    {
        (Attack, float) max = (null, -1);

        foreach (var atk in ValidAttackSet())
        {
            var maxMultiplier = Posibles.Max(p => p.GetMultiplier(atk.GetDoughType(this, battle.PlayerEntity), out _));
            var val = maxMultiplier * atk.Power;

            if (max.Item2 < val)
            {
                max = (atk, val);
            }
        }

        return max.Item1;
    }
}