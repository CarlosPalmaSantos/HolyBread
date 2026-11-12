using System.Collections;
using Godot;
using HCoroutines;
using HolyBread.scripts.battle;
using HolyBread.scripts.dialogues;

namespace HolyBread.scripts;

public partial class BattleUi : CanvasLayer
{
    [Export] public BattleManager Manager { get; set; }
    [Export] public TextureRect Panel { get; set; }
    [Export] public Container MainMenu { get; set; }
    [Export] public Container AttackMenu { get; set; }
    [Export] public Container BreadMenu { get; set; }

    [Export] public Range EnemyBar { get; set; }
    [Export] public Range PlayerBar { get; set; }
    [Export] public TextureRect EnemyRect { get; set; }
    [Export] public TextureRect PlayerRect { get; set; }

    [Export] public Color GoodHP { get; set; }
    [Export] public Color BadHP { get; set; }
    [Export] public Texture2D GoodHPTexture { get; set; }
    [Export] public Texture2D MediumHPTexture { get; set; }
    [Export] public Texture2D BadHPTexture { get; set; }

    [Export] public RichTextLabel EnemyName { get; set; }
    [Export] public RichTextLabel PlayerName { get; set; }

    [Export] public Button Attack1Button { get; set; }
    [Export] public Button Attack2Button { get; set; }
    [Export] public Button Attack3Button { get; set; }

    [Export] public VBoxContainer BreadContainer { get; set; }
    [Export] public Theme Theme { get; set; }


    public override void _Ready()
    {
        Attack1Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 0);
        Attack2Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 1);
        Attack3Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 2);
    }

    public void UpdateHpBar(Range bar, Entity entity)
    {
        bar.MaxValue = entity.Hp;
        bar.Value = entity.CurrentHp;
        if (bar is TextureProgressBar textureBar)
        {
            textureBar.TextureProgress = entity.PercentualHp > 0.5f ? GoodHPTexture :
                entity.PercentualHp > 0.25f ? MediumHPTexture : BadHPTexture;
        }
    }

    public void UpdateHpBar(Entity entity)
    {
        if (entity == Manager.Player) UpdateHpBar(PlayerBar, entity);
        else if (entity == Manager.Enemy) UpdateHpBar(EnemyBar, entity);
    }

    public IEnumerator AnimHpBar(Entity entity)
    {
        var bar = entity == Manager.Player ? PlayerBar : EnemyBar;
        if ((int)bar.Value == entity.CurrentHp) yield break;

        var step = bar.Value > entity.CurrentHp ? -1 : 1;

        for (var hp = bar.Value; step * hp <= step * entity.CurrentHp; hp += step)
        {
            bar.Value = hp;
            var percent = hp / entity.Hp;

            if (bar is TextureProgressBar textureBar)
            {
                textureBar.TextureProgress = percent > 0.5f ? GoodHPTexture :
                    percent > 0.25f ? MediumHPTexture : BadHPTexture;
            }

            yield return Co.Wait(0.02f);
        }
    }

    public void UpdateInfo()
    {
        UpdateHpBar(Manager.Player);
        UpdateHpBar(Manager.Enemy);

        PlayerName.Text = $"{Manager.Player.Name}   [m. {Manager.Player.Level}]";

        EnemyName.Text = $"{Manager.Enemy.Name}   [m. {Manager.Enemy.Level}]";
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

        foreach (var child in BreadContainer.GetChildren())
        {
            BreadContainer.RemoveChild(child);
        }

        for (var i = 0; i < Manager.Player.BreadSet.Length; i++)
        {
            var bread = Manager.Player.BreadSet[i];
            if (bread.Durability <= 0)
                continue;

            var b = new Button();
            b.CustomMinimumSize = new Vector2(200, 86);
            b.CustomMaximumSize = new Vector2(200, 86);
            b.Alignment = HorizontalAlignment.Center;
            b.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;

            b.Theme = Theme;
            b.Text = bread.Name + $" [{bread.Durability}/{bread.MaxDurability}]";

            var i1 = i;
            b.Pressed += () => Manager.PlayerAction(ActionType.BREAD, i1);
            BreadContainer.AddChild(b);
        }

        for (var i = 0; i < Manager.Player.BreadSet.Length; i++)
        {
            var bread = Manager.Player.BreadSet[i];
            if (bread.Durability > 0)
                continue;

            var b = new Button();
            b.Theme = Theme;
            b.Text = bread.Name + $" [{bread.Durability}/{bread.MaxDurability}]";
            b.Disabled = true;

            BreadContainer.AddChild(b);
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
        if (Manager.TutorialPart == 2)
        {
            return;
        }

        if (Manager.TutorialPart == 3)
        {
            Co.Run(Manager.Tutorial3());
        }

        if (Manager.TutorialPart == 4)
        {
            DialogManager.Play(Manager.ThirdDialogue);
            return;
        }

        if (Manager.TutorialPart == 5)
        {
            Co.Run(Manager.Tutorial4());
            return;
        }

        Panel.Show();
        MainMenu.Show();
        AttackMenu.Hide();
        BreadMenu.Hide();
    }

    public void ShowBasicAttackMenu()
    {
        if (Manager.TutorialPart == 4)
        {
            DialogManager.Play(Manager.ThirdDialogue);
            return;
        }


        Panel.Show();
        MainMenu.Hide();
        AttackMenu.Show();
        BreadMenu.Hide();

        if (Manager.TutorialPart == 2)
        {
            Co.Run(Manager.Tutorial2());
        }
    }

    public void ShowBreadAttackMenu()
    {
        if (Manager.TutorialPart == 2)
        {
            DialogManager.Play(Manager.NoCorrectButtonAttackDialogue);
            return;
        }

        Panel.Show();
        MainMenu.Hide();
        AttackMenu.Hide();
        BreadMenu.Show();
    }

    public void RunAway()
    {
        if (Manager.TutorialPart == 2)
        {
            DialogManager.Play(Manager.NoCorrectButtonAttackDialogue);
            return;
        }

        if (Manager.TutorialPart == 4)
        {
            DialogManager.Play(Manager.ThirdDialogue);
            return;
        }

        HideMenu();
        var dialog = new Dialogue()
        {
            Lines =
            [
                new DialogueLine()
                {
                    Text = "NO PUEDES HUIR DE TU [g]DESTINO[/g]",
                    Centered = true,
                }
            ],
        };
        dialog.OnDialogueEnd += ShowMainMenu;
        DialogManager.Play(dialog);

        GetViewport().GuiReleaseFocus();
    }

    public void Load(Entity player, Enemy enemy, bool tutorial = false)
    {
        Show();
        ShowMainMenu();
        Manager.Load(player, enemy, tutorial);
        UpdateInfo();
    }
}