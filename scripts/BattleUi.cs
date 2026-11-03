using Godot;
using System;
using HolyBread.scripts.battle;

public partial class BattleUi : CanvasLayer
{
    [Export] public BattleManager Manager { get; set; }
    [Export] public Container MainMenu { get; set; }
    [Export] public Container AttackMenu { get; set; }

    [Export] public Button Attack1Button { get; set; }
    [Export] public Button Attack2Button { get; set; }
    [Export] public Button Attack3Button { get; set; }

    public override void _Ready()
    {
        GD.Print("Ready");
        MainMenu.Show();
        AttackMenu.Hide();
    }

    public void HideMenu()
    {
        MainMenu.Hide();
        AttackMenu.Hide();
    }
    
    public void ShowMainMenu()
    {
        MainMenu.Show();
        AttackMenu.Hide();
    }

    public void ShowBasicAttackMenu()
    {
        MainMenu.Hide();
        AttackMenu.Show();

        var a1 = Manager.Player.GetAttack(0);
        if (a1 != null)
        {
            Attack1Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 0);
            Attack1Button.Text = a1.Name;
        }
        else Attack1Button.Text = "---";

        var a2 = Manager.Player.GetAttack(1);
        if (a2 != null)
        {
            Attack2Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 1);
            Attack2Button.Text = a2.Name;
        }
        else Attack2Button.Text = "---";

        var a3 = Manager.Player.GetAttack(2);
        if (a3 != null)
        {
            Attack3Button.Pressed += () => Manager.PlayerAction(ActionType.ATTACK, 2);
            Attack3Button.Text = a3.Name;
        }
        else Attack3Button.Text = "---";
    }

    public void ShowBreadAttackMenu()
    {
        MainMenu.Hide();
        AttackMenu.Show();

        var a1 = Manager.Player.GetBreadAttack(0);
        if (a1 != null)
        {
            Attack1Button.Pressed += () => Manager.PlayerAction(ActionType.BREAD, 0);
            Attack1Button.Text = a1.Name;
        }
        else Attack1Button.Text = "---";

        var a2 = Manager.Player.GetBreadAttack(1);
        if (a2 != null)
        {
            Attack2Button.Pressed += () => Manager.PlayerAction(ActionType.BREAD, 1);
            Attack2Button.Text = a2.Name;
        }
        else Attack2Button.Text = "---";

        var a3 = Manager.Player.GetBreadAttack(2);
        if (a3 != null)
        {
            Attack3Button.Pressed += () => Manager.PlayerAction(ActionType.BREAD, 2);
            Attack3Button.Text = a3.Name;
        }
        else Attack3Button.Text = "---";
    }
}