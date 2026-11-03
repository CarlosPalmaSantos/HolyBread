using Godot;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Enemy : Entity
{
    public (ActionType, int) GetEnemyAction(BattleManager battle)
    {
        return (ActionType.NONE, 0);
    }    
}