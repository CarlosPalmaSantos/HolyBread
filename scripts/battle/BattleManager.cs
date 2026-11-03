using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using HCoroutines;
using HolyBread.scripts.dialogues;

namespace HolyBread.scripts.battle;

public partial class BattleManager : Node
{
    [Export] public BattleUi Ui { get; set; }
    [Export] public Entity Player { get; set; }
    [Export] public Enemy Enemy { get; set; }

    private static void UseAttack(Entity user, Entity target, int attack, out Attack.Effectiveness effectiveness) =>
        user.AttackSet[attack].Act(user, target, out effectiveness);

    private static void UseBreadAttack(Entity user, Entity target, int attack,
        out Attack.Effectiveness effectiveness) =>
        target.Bread.AttackSet[attack].Act(user, target, out effectiveness);

    public void PlayerAction(ActionType actionType, int arg)
    {
        Co.Run(PlayerActionCoroutine(actionType, arg));
    }

    public IEnumerator PlayerActionCoroutine(ActionType actionType, int arg)
    {
        var enemyAction = Enemy.GetEnemyAction(this);

        Entity first = Player;
        var firstAction = (actionType, arg);
        Entity last = Enemy;
        var lastAction = enemyAction;

        if (Player.Spd < Enemy.Spd || (Player.Spd == Enemy.Spd && Random.Shared.NextSingle() < 0.5))
        {
            first = Enemy;
            firstAction = enemyAction;

            last = Player;
            lastAction = (actionType, arg);
        }

        Ui.HideMenu();
        yield return ExecuteActionCoroutine(firstAction, first, last);
        yield return ExecuteActionCoroutine(lastAction, last, first);
        Ui.ShowMainMenu();
    }


    private IEnumerator ExecuteActionCoroutine((ActionType, int) action, Entity user, Entity target)
    {
        switch (action.Item1)
        {
            case ActionType.NONE:
                break;
            case ActionType.ATTACK:
                yield return DialogManager.PlayCoroutine($"{user.Name} usó [b]{user.AttackSet[action.Item2].Name}[/b]");
                UseAttack(user, target, action.Item2, out var basicEff);
                break;
            case ActionType.BREAD:
                yield return DialogManager.PlayCoroutine($"{user.Name} usó [b]{user.AttackSet[action.Item2].Name}[/b]");
                UseBreadAttack(user, target, action.Item2, out var breadEff);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum ActionType
{
    NONE,
    ATTACK,
    BREAD,
}