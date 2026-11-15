using Godot;
using HCoroutines;
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
	
	[Export] public bool Debug_SkipTutorial { get; set; }
	[Export] public Enemy TutorialEnemy { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Panel.Show();
	}

	public void Exit()
	{
		GetTree().Quit();
	}

	public void StartMainMenu()
	{
		Panel.Hide();
		//TODO: Mejorar

		if (Debug_SkipTutorial)
		{
			Map.Show();
			Map.ShowMapTutorial();
			return;
		}
		
		IntroDialogue.OnDialogueEnd += () =>
		{
			//Map.Show();
			var battle = GMScene.Instantiate() as BattleUi;
			GetParent().AddChild(battle);
			battle.Load(SaveManager.Player, TutorialEnemy, 0,true);

			battle.Manager.OnEnemyDefeated += () =>
			{
				battle.QueueFree();
				Map.Show();
				Map.ShowMapTutorial();
			};
			Hide();
		};
        
		DialogManager.Play(IntroDialogue);
	}
}