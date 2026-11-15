using System.Linq;
using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class BreadGodEnemy : Enemy
{
    [Export] Attack hostion;
    public bool hostionReload;

    public override Attack[] ValidAttackSet()
    {
        return hostionReload
            ? [.. AttackSet.Where(a => a != hostion)]
            : AttackSet;
    }

    public override (ActionType, int) GetEnemyAction(BattleManager battle)
    {
        var r = base.GetEnemyAction(battle);

        if (hostionReload)
        {
            hostionReload = false;
        }

        if (AttackSet[r.Item2] == hostion)
        {
            hostionReload = true;
        }
        
        return r;
    }
}