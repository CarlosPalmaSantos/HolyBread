using Godot;

namespace HolyBread.scripts.dialogues;

[GlobalClass]
public partial class Dialogue : Resource
{
    [Export] public DialogueLine[] Lines { get; set; }
    [Export] public bool Shadow { get; set; }

    [Signal]
    public delegate void OnDialogueEndEventHandler();

    public void FinishDialogue()
    {
        EmitSignalOnDialogueEnd();
    }
}