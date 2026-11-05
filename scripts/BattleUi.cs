using Godot;
using System;
using HolyBread.scripts.battle;

public partial class BattleUi : CanvasLayer
{
    [Export] public BattleManager Manager { get; set; }
    [Export] public Panel Panel { get; set; }
    [Export] public Container MainMenu { get; set; }
    [Export] public Container AttackMenu { get; set; }
    [Export] public Container BreadMenu { get; set; }

    [Export] public Button Attack1Button { get; set; }
    [Export] public Button Attack2Button { get; set; }
    [Export] public Button Attack3Button { get; set; }

    [Export] public VBoxContainer BreadContainer { get; set; }


    public override void _Ready()
    {
        GD.Print("Ready");
        MainMenu.Show();
        AttackMenu.Hide();

        Attack1Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 0);
        Attack2Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 1);
        Attack3Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 2);

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
        AttackMenu.Show();
        BreadMenu.Hide();
    }
}