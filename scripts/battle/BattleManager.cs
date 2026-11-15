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
    [Export] public Player Player { get; set; }
    [Export] public Entity PlayerEntity { get; set; }
    [Export] public Enemy Enemy { get; set; }

    [Signal]
    public delegate void OnEnemyDefeatedEventHandler();

    [Signal]
    public delegate void OnPlayerDefeatedEventHandler();

    private static void UseAttack(Entity user, Entity target, int attack, out Attack.Effectiveness effectiveness)
    {
        user.AttackSet[attack].Act(user, target, out effectiveness);
    }

    private static void UseBreadAttack(Entity user, Entity target, int attack,
        out Attack.Effectiveness effectiveness)
    {
        var bread = user.BreadSet[attack];
        bread.Attack.Act(user, target, out effectiveness);

        bread.Durability--;
    }

    public void PlayerAction(ActionType actionType, int arg)
    {
        if (TutorialPart == 4 && actionType == ActionType.BREAD) 
            TutorialPart = 5;
        
        Co.Run(PlayerActionCoroutine(actionType, arg));
    }

    public IEnumerator PlayerActionCoroutine(ActionType actionType, int arg)
    {
        var enemyAction = Enemy.GetEnemyAction(this);

        Entity first = PlayerEntity;
        var firstAction = (actionType, arg);
        Entity last = Enemy;
        var lastAction = enemyAction;

        if (PlayerEntity.Spd < Enemy.Spd || (PlayerEntity.Spd == Enemy.Spd && Random.Shared.NextSingle() < 0.5))
        {
            first = Enemy;
            firstAction = enemyAction;

            last = PlayerEntity;
            lastAction = (actionType, arg);
        }

        Ui.HideMenu();
        yield return ExecuteActionCoroutine(firstAction, first, last);
        yield return Co.Wait(0.25f);
        yield return ExecuteActionCoroutine(lastAction, last, first);

        if (Enemy.CurrentHp <= 0)
            EmitSignalOnEnemyDefeated();

        if (PlayerEntity.CurrentHp <= 0)
            EmitSignalOnPlayerDefeated();

        Ui.ShowMainMenu();
        Ui.UpdateInfo();
    }


    private IEnumerator ExecuteActionCoroutine((ActionType, int) action, Entity user, Entity target)
    {
        if (user.CurrentHp <= 0)
            yield break;

        var eff = Attack.Effectiveness.NONE;

        switch (action.Item1)
        {
            case ActionType.NONE:
                yield return DialogManager.PlayCoroutine(
                    $"[b][color={Ui.BadHPTexture}]{user.Name}[/color][/b] te mira fijamente", keepPanel: true);
                break;
            case ActionType.ATTACK:
                yield return DialogManager.PlayCoroutine(
                    $"[b]{user.Name}[/b] usó [g]{user.AttackSet[action.Item2].Name}[/g]", keepPanel: true);
                UseAttack(user, target, action.Item2, out eff);
                break;
            case ActionType.BREAD:
                yield return DialogManager.PlayCoroutine(
                    $"[b]{user.Name}[/b] usó [g]{user.BreadSet[action.Item2].Name}[/g]", keepPanel: true);
                UseBreadAttack(user, target, action.Item2, out eff);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        yield return Ui.AnimHpBar(user);
        yield return Ui.AnimHpBar(target);

        yield return Co.Wait(0.2f);

        if (eff == Attack.Effectiveness.NONE) yield break;
        var dialogText = eff switch
        {
            Attack.Effectiveness.NORMAL => "Fue un golpe normal",
            Attack.Effectiveness.WEAK => $"Fue un golpe [b][color={Ui.GoodHP.ToHtml(false)}]muy eficaz[/color][/b]",
            Attack.Effectiveness.RESIST => $"Fue un golpe [b][color={Ui.BadHP.ToHtml(false)}]poco eficaz[/color][/b]",
            _ => throw new ArgumentOutOfRangeException()
        };

        yield return DialogManager.PlayCoroutine(dialogText, keepPanel: true);
    }

    public void Load(Player player, Enemy enemy, bool tutorial)
    {
        Tutorial = tutorial;
        
        Player = player;
        PlayerEntity = player.Entity;
        Enemy = enemy;
        if (PlayerEntity.CurrentHp < 0) PlayerEntity.CurrentHp = PlayerEntity.Hp;
        if (Enemy.CurrentHp < 0) Enemy.CurrentHp = Enemy.Hp;

        if (!Tutorial) return;
        TutorialPart = 1;
        Ui.HideMenu();
        Co.Run(Tutorial1());
    }
}

public enum ActionType
{
    NONE,
    ATTACK,
    BREAD,
}