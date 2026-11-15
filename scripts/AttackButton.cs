using Godot;
using HolyBread.scripts.battle;

namespace HolyBread.scripts;

public partial class AttackButton : Button
{
	[Export] public RichTextLabel Description { get; set; }
	[Export] public Attack Attack { get; set; }

	public void OnMouseEntered()
	{
		if (Attack == null) return;
		(Description.GetParent() as Panel)?.Show();
		Description.Text = Attack.Description;
	}

	public void OnMouseExited()
	{
		
		Description.Text = "";
		(Description.GetParent() as Panel)?.Hide();
	}
	
	public override void _Ready()
	{
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
	}
}