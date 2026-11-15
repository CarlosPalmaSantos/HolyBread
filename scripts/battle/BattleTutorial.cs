using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using HolyBread.scripts.dialogues;

namespace HolyBread.scripts.battle;

public partial class BattleManager
{
    [Export] public bool Tutorial { get; set; } = false;
    [Export] public Dialogue FirstDialogue { get; set; }
    [Export] public Dialogue SecondDialogue { get; set; }
    [Export] public Dialogue NoCorrectButtonAttackDialogue { get; set; }
    
    [Export] public Bread VienaBread { get; set; }
    [Export] public Bread InitialBread { get; set; }
    
    [Export] public Attack NextAttack { get; set; }

    [Export] public Dialogue ThirdDialogue { get; set; }

    [Export] public Dialogue FourthDialogue { get; set; }
    public int _tutorialPart = 0;

    public int TutorialPart
    {
        get => _tutorialPart;
        set
        {
            _tutorialPart = value;
            GD.Print("Tutorial: ", _tutorialPart);
        }
    }

    public IEnumerator Tutorial1()
    {
        TutorialPart = 1;
        yield return DialogManager.PlayCoroutine(FirstDialogue);

        Ui.ShowMainMenu();
        TutorialPart = 2;
    }

    public IEnumerator Tutorial2()
    {
        yield return DialogManager.PlayCoroutine(SecondDialogue);
        TutorialPart = 3;
    }

    public IEnumerator Tutorial3()
    {
        if (!Player.BreadSet.Contains(VienaBread))
            Player.BreadSet = [.. Player.BreadSet, VienaBread];
        
        if (!PlayerEntity.BreadSet.Contains(VienaBread))
            PlayerEntity.BreadSet = [.. PlayerEntity.BreadSet, VienaBread];
        
        Ui.UpdateInfo();

        yield return DialogManager.PlayCoroutine(ThirdDialogue);
        TutorialPart = 4;
    }

    public IEnumerator Tutorial4()
    {
        if (!Player.BreadSet.Contains(InitialBread))
            Player.BreadSet = [.. Player.BreadSet, InitialBread];
        
        if (!PlayerEntity.BreadSet.Contains(InitialBread))
            PlayerEntity.BreadSet = [.. PlayerEntity.BreadSet, InitialBread];

        PlayerEntity.AttackSet = [.. PlayerEntity.AttackSet, NextAttack];
        Ui.UpdateInfo();
        
        yield return DialogManager.PlayCoroutine(FourthDialogue);
        Player.Entity.Recover();
        Player.Entity.LevelUp();
        
        EmitSignalOnEnemyDefeated();
    }
}