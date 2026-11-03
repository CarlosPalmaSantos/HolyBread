using System.Collections;
using System.Net.Mime;
using Godot;
using HCoroutines;

namespace HolyBread.scripts.dialogues;

public partial class DialogManager : CanvasLayer
{
    [Export] public Panel MainPanel;
    [Export] public Panel SpeakerPanel;
    [Export] public ColorRect Background;

    [Export] public RichTextLabel TextLabel;
    [Export] public RichTextLabel SpeakerLabel;

    public static DialogManager Instance { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        Instance = this;
        GD.Print("DialogManager: Instanced");
    }

    public override void _Ready()
    {
        MainPanel.Hide();
        SpeakerPanel.Hide();
        Background.Hide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            if (WaitingForContinue) Continue = true;
            else FastForward = true;
        }
    }


    public static void Play(string text, float speed = 1) => Co.Run(PlayCoroutine(text, speed));

    public static IEnumerator PlayCoroutine(string text, float speed = 1)
    {
        yield return Instance.PlayDialogueCoroutine(
            new Dialogue()
            {
                Lines =
                [
                    new DialogueLine
                    {
                        Text = text,
                        Speed = speed,
                    }
                ],
                Shadow = true,
            });
    }

    public static void Play(Dialogue dialogue) => Co.Run(PlayCoroutine(dialogue));

    public static IEnumerator PlayCoroutine(Dialogue dialogue)
    {
        yield return Instance.PlayDialogueCoroutine(dialogue);
    }

    [Export] public bool Continue { get; private set; }
    private bool WaitingForContinue { get; set; }

    private IEnumerator PlayDialogueCoroutine(Dialogue dialogue)
    {
        TextLabel.Text = "";
        MainPanel.Show();

        if (dialogue.Shadow) Background.Show();
        else Background.Hide();

        foreach (var line in dialogue.Lines)
        {
            if (!string.IsNullOrEmpty(line.Speaker))
            {
                SpeakerPanel.Show();
                SpeakerLabel.Text = line.Speaker;
            }

            yield return ShowTextCoroutine(line.Text, line.Speed);

            WaitingForContinue = true;
            yield return Co.WaitUntil(() => Continue);
            WaitingForContinue = false;

            Continue = false;

            line.FinishLine();
            TextLabel.Text = "";
            SpeakerPanel.Hide();
        }

        MainPanel.Hide();
        Background.Hide();

        dialogue.FinishDialogue();
    }

    private bool FastForward { get; set; }

    private IEnumerator ShowTextCoroutine(string text, float speed)
    {
        var richFlag = false;
        var str = "";
        foreach (var c in text)
        {
            if (c == '[')
                richFlag = true;

            str += c;

            if (!richFlag)
            {
                TextLabel.Text = str;
                yield return FastForward ? null : Co.Wait(0.05f / speed);
            }
            else if (c == ']') richFlag = false;
        }

        FastForward = false;
    }

    public void OnMainPanelGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
        }
    }
}