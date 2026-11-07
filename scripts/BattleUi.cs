using Godot;
using System;
using System.Collections;
using HCoroutines;
using HolyBread.scripts.battle;
using HolyBread.scripts.dialogues;

public partial class BattleUi : CanvasLayer
{
    [Export] public BattleManager Manager { get; set; }
    [Export] public Panel Panel { get; set; }
    [Export] public Container MainMenu { get; set; }
    [Export] public Container AttackMenu { get; set; }
    [Export] public Container BreadMenu { get; set; }

    [Export] public ProgressBar EnemyBar { get; set; }
    [Export] public ProgressBar PlayerBar { get; set; }
    [Export] public TextureRect EnemyRect { get; set; }
    [Export] public TextureRect PlayerRect { get; set; }

    [Export] public Color GoodHP { get; set; }
    [Export] public Color MediumHP { get; set; }
    [Export] public Color BadHP { get; set; }

    [Export] public RichTextLabel EnemyName { get; set; }

    [Export] public Button Attack1Button { get; set; }
    [Export] public Button Attack2Button { get; set; }
    [Export] public Button Attack3Button { get; set; }

    [Export] public VBoxContainer BreadContainer { get; set; }


    public override void _Ready()
    {
        GD.Print("Ready");
        ShowMainMenu();

        Manager.Start();

        Attack1Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 0);
        Attack2Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 1);
        Attack3Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 2);


        UpdateInfo();
    }

    public void UpdateHpBar(ProgressBar bar, Entity entity)
    {
        bar.MaxValue = entity.Hp;
        bar.Value = entity.CurrentHp;
        bar.AddThemeStyleboxOverride("fill",
            new StyleBoxFlat()
            {
                BgColor = entity.PercentualHP > 0.5f ? GoodHP :
                    entity.PercentualHP > 0.25f ? MediumHP : BadHP
            });
    }

    public void UpdateHpBar(Entity entity)
    {
        if (entity == Manager.Player) UpdateHpBar(PlayerBar, entity);
        else if (entity == Manager.Enemy) UpdateHpBar(EnemyBar, entity);
    }

    public IEnumerator AnimHpBar(Entity entity)
    {
        var bar = entity == Manager.Player ? PlayerBar : EnemyBar;

        for (var hp = bar.Value; hp >= entity.CurrentHp; hp--)
        {
            bar.Value = hp;
            yield return Co.Wait(0.02f);
        }
    }

    public void UpdateInfo()
    {
        UpdateHpBar(Manager.Player);
        UpdateHpBar(Manager.Enemy);

        EnemyName.Text = Manager.Enemy.Name;
        EnemyRect.Texture = Manager.Enemy.Texture;

        var atk1 = Manager.Player.GetAttack(0);
        Attack1Button.Text = atk1 != null ? atk1.Name : "---";
        Attack1Button.Disabled = atk1 == null;

        var atk2 = Manager.Player.GetAttack(1);
        Attack2Button.Text = atk2 != null ? atk2.Name : "---";
        Attack2Button.Disabled = atk2 == null;

        var atk3 = Manager.Player.GetAttack(2);
        Attack3Button.Text = atk3 != null ? atk3.Name : "---";
        Attack3Button.Disabled = atk3 == null;

        for (var i = 0; i < Manager.Player.BreadList.Length; i++)
        {
            var bread = Manager.Player.BreadList[i];
            var b = new Button();
            b.Text = bread.Name;

            var i1 = i;
            b.Pressed += () => Manager.PlayerAction(ActionType.BREAD, i1);
            BreadContainer.AddChild(b);
            i++;
        }
    }

    public void HideMenu()
    {
        MainMenu.Hide();
        AttackMenu.Hide();
        BreadMenu.Hide();
        Panel.Hide();
    }

    public void ShowMainMenu()
    {
        Panel.Show();
        MainMenu.Show();
        AttackMenu.Hide();
        BreadMenu.Hide();
    }

    public void ShowBasicAttackMenu()
    {
        Panel.Show();
        MainMenu.Hide();
        AttackMenu.Show();
        BreadMenu.Hide();
    }

    public void ShowBreadAttackMenu()
    {
        Panel.Show();
        MainMenu.Hide();
        AttackMenu.Hide();
        BreadMenu.Show();
    }

    public void RunAway()
    {
        HideMenu();
        var dialog = new Dialogue()
        {
            Lines =
            [
                new DialogueLine()
                {
                    Text = "NO PUEDES HUIR DE TU [b]DESTINO[/b]",
                    Centered = true,
                }
            ],
        };
        dialog.OnDialogueEnd += () => ShowMainMenu();
        DialogManager.Play(dialog);

        GetViewport().GuiReleaseFocus();
    }
}