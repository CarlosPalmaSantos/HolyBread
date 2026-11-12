using System.Collections;
using System.Collections.Generic;
using Godot;
using HolyBread.scripts.dialogues;

namespace HolyBread.scripts.battle;

public partial class BattleManager
{
    [Export] public bool Tutorial { get; set; } = false;
    [Export] public Dialogue FirstDialogue { get; set; }
    [Export] public Dialogue SecondDialogue { get; set; }
    [Export] public Dialogue NoCorrectButtonAttackDialogue { get; set; }
    
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
        yield return DialogManager.PlayCoroutine(ThirdDialogue);
        TutorialPart = 4;
    }

    public IEnumerator Tutorial4()
    {
        yield return DialogManager.PlayCoroutine(FourthDialogue);
        EmitSignalOnEnemyDefeated();
    }
}