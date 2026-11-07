using Godot;

namespace HolyBread.scripts.dialogues;

[GlobalClass]
public partial class DialogueLine : Resource
{
    [Export] public string Speaker { get; set; }
    [Export(PropertyHint.MultilineText)] public string Text { get; set; }
    [Export] public float Speed { get; set; } = 1;
    [Export] public bool Centered { get; set; }

    [Signal]
    public delegate void OnLineEndEventHandler();

    public void FinishLine()
    {
        EmitSignalOnLineEnd();
    }
}