using Godot;
using System;
using System.Collections;
using HCoroutines;
using HolyBread.scripts.dialogues;

public partial class Test : Node
{
    [Export] public Dialogue Dialogue;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Co.Run(StartCoroutine());
    }

    public IEnumerator StartCoroutine()
    {
        yield return Co.Wait(2);
        DialogManager.Play(Dialogue);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}