using System;
using System.Collections;
using Godot;
using HCoroutines;
using HolyBread.scripts.battle;
using HolyBread.scripts.dialogues;
using HolyBread.scripts.Paths;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts;

public partial class GameManager : Node
{
    [Export] public BattleUi BattleUi { get; set; }
    [Export] public SaveManager SaveManager { get; set; }

    [Export] public Panel ChoicePanel { get; set; }
    [Export] public TextureRect LeftChoice { get; set; }
    [Export] public Button LeftChoiceButton { get; set; }

    [Export] public TextureRect RightChoice { get; set; }
    [Export] public Button RightChoiceButton { get; set; }
    [Export] public ProgressBar PathProgressBar { get; set; }
    [Export] public TextureRect BackgroundRect { get; set; }

    [Signal]
    public delegate void OnPlayerDefeatedEventHandler();

    public Path Path { get; set; }

    public Enemy RandomEnemy(EnemyPreset[] arr)
    {
        return arr[Random.Shared.Next(arr.Length)].GetRandom();
    }


    public void GenChoice(Spot spot, Button button, TextureRect rect)
    {
        var r = Random.Shared.NextSingle();

        r -= spot.BattleChance;
        if (r < 0)
        {
            var enemy = Path.GetRandomEnemy();
            button.Text = $"{enemy.Name}\nMaestría: {enemy.Level}";
            GD.Print($"{enemy.Name} ({enemy.DoughType.Name})");
            rect.Texture = enemy.Texture;
            button.ClearPressed();
            button.Pressed += () =>
            {
                StartBattle(SaveManager.Player, enemy);
                ChoicePanel.Hide();
                BattleUi.Show();
            };
            return;
        }

        r -= spot.HardBattleChance;
        if (r < 0)
        {
            var enemy = Path.GetRandomEnemy();
            enemy.Level++;
            button.Text = $"{enemy.Name}\nMaestría: {enemy.Level}";
            GD.Print($"{enemy.Name} ({enemy.DoughType.Name})");
            rect.Texture = enemy.Texture;
            button.ClearPressed();
            button.Pressed += () =>
            {
                StartBattle(SaveManager.Player, enemy);
                ChoicePanel.Hide();
                BattleUi.Show();
            };
            return;
        }

        r -= spot.TreasureChance;
        if (r < 0)
        {
            button.Text = "Investigar";
            button.ClearPressed();
            button.Pressed += () =>
            {
                GD.Print("HOLAAA");
                ChoicePanel.Hide();
                BattleUi.Hide();
            };
        }
    }

    public void NextChoices()
    {
        var spot = Path.NextSpot();

        if (spot == null) return;

        PathProgressBar.Value = Path.Position;
        PathProgressBar.MaxValue = Path.Spots.Length;

        BackgroundRect.Texture = BattleUi.Backgrounds[BattleUi.Background];
        ChoicePanel.Show();
        GenChoice(spot, LeftChoiceButton, LeftChoice);
        GenChoice(spot, RightChoiceButton, RightChoice);
    }

    public void StartBattle(Player player, Enemy enemy, bool tutorial = false)
    {
        BattleUi.Load(player, enemy, Path.Background, tutorial);
    }

    public void PlayerDefeated()
    {
        BattleUi.Hide();
        var d = new Dialogue
        {
            Lines = [new DialogueLine { Centered = true, Text = "[g]HAS MUERTO[/g]" }]
        };
        d.OnDialogueEnd += EmitSignalOnPlayerDefeated;
        DialogManager.Play(d);
    }

    public void OnEnemyDefeated()
    {
        NextChoices();
        SaveManager.LevelUp();
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SaveManager = GetNode<SaveManager>("/root/Test/SaveManager");
        ChoicePanel.Hide();

        BattleUi.Manager.OnEnemyDefeated += OnEnemyDefeated; // TODO: Modificar esto
        BattleUi.Manager.OnPlayerDefeated += PlayerDefeated;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}