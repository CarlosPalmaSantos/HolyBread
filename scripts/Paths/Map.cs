using System;
using System.Collections;
using Godot;
using HCoroutines;
using HolyBread.scripts.battle;
using HolyBread.scripts.dialogues;

namespace HolyBread.scripts.Paths;

public partial class Map : CanvasLayer
{
    [Export] PathConnection[] Connections { get; set; } = [];
    [Export] public BaseButton[] Buttons { get; set; } = [];
    [Export] public int Position { get; set; }
    [Export] PackedScene GMScene { get; set; }
    [Export] PackedScene BattleScene { get; set; }
    [Export] Texture2D EnabledTexture { get; set; }
    [Export] Texture2D CurrentTexture { get; set; }
    [Export] SaveManager SaveManager { get; set; }

    [Export] public TextureRect BackgroundRect { get; set; }
    [Export] public Texture2D[] Backgrounds { get; set; }
    [Export] public int Background { get; set; }
    [Export] public Control Layer { get; set; }

    [Export] public Dialogue MapTutorial { get; set; }
    [Export] public Dialogue SantuarioDialogue { get; set; }
    [Export] public Dialogue EndDialogue { get; set; }
    [Export] public Control ChoiceDough { get; set; }

    [Export] public DoughType[] Types { get; set; }

    public Path GetPath(BaseButton a, BaseButton b)
    {
        foreach (var conn in Connections)
        {
            if (conn.Has(a) && conn.Has(b))
                return conn.Path;
        }

        throw new Exception();
    }

    public void ShowMapTutorial()
    {
        Co.Run(DialogManager.PlayCoroutine(MapTutorial));
    }

    public override void _Ready()
    {
        for (var index = 0; index < Buttons.Length; index++)
        {
            var button = Buttons[index];

            var index1 = index;
            button.Pressed += () =>
            {
                if (Position == index1) return;

                Hide();
                var gameManager = GMScene.Instantiate() as GameManager;
                GetParent().AddChild(gameManager);

                var path = GetPath(Buttons[Position], Buttons[index1]);
                gameManager.Path = path;

                Path.OnPathFinishedEventHandler onFinished = null;
                onFinished = () =>
                {
                    path.OnPathFinished -= onFinished;

                    Position = index1;
                    SaveManager.Entity.Recover();
                    EnableConnections();
                    if (GodotObject.IsInstanceValid(gameManager))
                        gameManager.QueueFree();
                    Show();
                };
                path.OnPathFinished += onFinished;

                gameManager.OnPlayerDefeated += () =>
                {
                    path.OnPathFinished -= onFinished;
                    path.Position = -1;
                    SaveManager.Entity.Recover();
                    if (GodotObject.IsInstanceValid(gameManager))
                        gameManager.QueueFree();
                    Show();
                };

                gameManager.NextChoices();
                GD.Print("Moving To ", index1);
            };
        }

        EnableConnections();
    }

    [Export] private Dialogue[] FinalBattlePrevDialogue { get; set; }
    [Export] private TextureRect BreadGod { get; set; }
    [Export] private Enemy BreadGodEnemy { get; set; }

    public IEnumerator PlayerDefeated()
    {
        BreadGod.Hide();

        yield return Co.Wait(0.1f);
        var d = new Dialogue
        {
            Lines = [new DialogueLine { Centered = true, Text = "[g]HAS MUERTO[/g]" }]
        };

        yield return DialogManager.PlayCoroutine(d);
        yield return FinalBattleCoroutine();
    }

    public IEnumerator FinalBattleCoroutine()
    {
        BreadGod.Hide();
        yield return DialogManager.PlayCoroutine(FinalBattlePrevDialogue[0]);
        yield return Co.Wait(0.25f);
        BreadGod.Show();
        yield return DialogManager.PlayCoroutine(FinalBattlePrevDialogue[1]);

        SaveManager.Entity.Recover();
        var battle = BattleScene.Instantiate() as BattleUi;
        battle.Manager.OnEnemyDefeated += () =>
        {
            battle.QueueFree();
            BreadGod.Show();
            EndDialogue.OnDialogueEnd += () => GetTree().Quit();
            DialogManager.Play(EndDialogue);
        };
        battle.Manager.OnPlayerDefeated += () =>
        {
            battle.QueueFree();
            Co.Run(PlayerDefeated());
        }; //TODO

        GetParent().AddChild(battle);
        battle.Load(SaveManager.Player, BreadGodEnemy, 3, false, true);
    }


    public void ChangeElement()
    {
        SaveManager.Player.ChangeDoughType(Types[Position]);
        Continue();
    }

    public void Continue()
    {
        ChoiceDough.Hide();
        Layer.Show();
    }


    public void EnableConnections()
    {
        Layer.Show();
        ChoiceDough.Hide();

        if (Position == Buttons.Length - 1)
        {
            Layer.Hide();
            BackgroundRect.Texture = Backgrounds[3];
            Co.Run(FinalBattleCoroutine());
            return;
        }

        if (Position is 0 or 1 or 2 or 3 or 5 or 6 && Types[Position] != SaveManager.Entity.DoughType)
        {
            Layer.Hide();
            BackgroundRect.Texture = Backgrounds[3];
            SantuarioDialogue.OnDialogueEnd += () => ChoiceDough.Show();
            DialogManager.Play(SantuarioDialogue);
        }

        BackgroundRect.Texture = Backgrounds[Position < 4 ? 0 : Position < 8 ? 1 : 2];

        if (Buttons[Position] is TextureButton tx)
        {
            tx.TextureNormal = CurrentTexture;
        }

        foreach (var button in Buttons)
        {
            button.Disabled = Buttons[Position] != button;
        }

        foreach (var conn in Connections)
        {
            if (!conn.Has(Buttons[Position])) continue;

            var o = conn.GetOther(Buttons[Position]);
            if (o is TextureButton txo)
            {
                txo.TextureNormal = EnabledTexture;
            }

            o.Disabled = false;
        }
    }
}