using System;
using Godot;

namespace HolyBread.scripts.Paths;

public partial class Map : CanvasLayer
{
    [Export] PathConnection[] Connections { get; set; } = [];
    [Export] public BaseButton[] Buttons { get; set; } = [];
    [Export] public int Position { get; set; }
    [Export] PackedScene GMScene { get; set; }
    [Export] Texture2D EnabledTexture { get; set; }
    [Export] Texture2D CurrentTexture { get; set; }
    [Export] SaveManager SaveManager { get; set; }

    public Path GetPath(BaseButton a, BaseButton b)
    {
        foreach (var conn in Connections)
        {
            if (conn.Has(a) && conn.Has(b))
                return conn.Path;
        }

        throw new Exception();
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

                this.Hide();
                var gameManager = GMScene.Instantiate() as GameManager;
                GetParent().AddChild(gameManager);

                gameManager.Path = GetPath(Buttons[Position], Buttons[index1]);
                gameManager.OnPlayerDefeated += () =>
                {
                    gameManager.Path.Position = -1;
                    SaveManager.Entity.Recover();
                    gameManager.QueueFree();
                    Show();
                };
                gameManager.Path.OnPathFinished += () =>
                {
                    Position = index1;
                    EnableConnections();
                    gameManager.QueueFree();
                    Show();
                };
                gameManager.NextChoices();
                

                GD.Print("Moving To ", index1);
            };
        }

        EnableConnections();
    }


    public void EnableConnections()
    {
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