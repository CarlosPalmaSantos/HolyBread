using Godot;
using HolyBread.scripts.battle;
using HolyBread.scripts.dialogues;
using HolyBread.scripts.Paths;

namespace HolyBread.scripts;

public partial class MainMenu : CanvasLayer
{
	[Export] public Panel Panel { get; set; }
	[Export] public Dialogue IntroDialogue { get; set; }
	[Export] public Map Map { get; set; }
	[Export] PackedScene GMScene { get; set; }
	[Export] SaveManager SaveManager { get; set; }
	
	[Export] public Enemy TutorialEnemy { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Panel.Show();
	}

	public void StartMainMenu()
	{
		Panel.Hide();
		//TODO: Mejorar

		IntroDialogue.OnDialogueEnd += () =>
		{
			//Map.Show();
			var battle = GMScene.Instantiate() as BattleUi;
			GetParent().AddChild(battle);
			battle.Load(SaveManager.Player.Entity, TutorialEnemy, true);

			battle.Manager.OnEnemyDefeated += () =>
			{
				battle.QueueFree();
				Map.Show();
			};
			Hide();
		};
        
		DialogManager.Play(IntroDialogue);
	}
}