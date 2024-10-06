using System.Linq;
using Godot;
using ldjam_2024;

public class main : Node2D
{
	public ProjectileManager ProjManager { get; set; }
	private Sprite _helpScreen;
	
	private Sprite _gameOverBruv;
		Player player = null;

	public override void _Ready()
	{
		var playerNodes = GetTree().GetNodesInGroup("PlayerCharacter");
		ProjManager = new ProjectileManager();
		AddChild(ProjManager);
		EnemySpawner spawner = new EnemySpawner();
		AddChild(spawner);

		if (playerNodes.Count > 0) player = (Player)playerNodes[0];

		if (player != null)
		{
			var enemyNodes = GetTree().GetNodesInGroup("EnemyCharacter");

			spawner.Player = player;
			foreach (AI enemy in enemyNodes) enemy.Player = player;
		}

		_helpScreen = GetChildren().Cast<Node>().First(x => x.Name == "HelpScreen") as Sprite;
		_gameOverBruv = GetChildren().Cast<Node>().First(x => x.Name == "GameOverBruv") as Sprite;
	}

	public override void _Process(float delta)
	{
		if (player.Dead)
		{
			_gameOverBruv.Visible = true;
			GetTree().Paused = true;
		}
	}

	// public override void _UnhandledInput(InputEvent @event)
	// {
	// 	if (@event.IsAction("show_help"))
	// 	{
	// 		_helpScreen.Visible = !_helpScreen.Visible;
	// 		if (!_helpScreen.Visible)
	// 		{
	// 			GetTree().Paused = false;
	// 		}
	// 		else
	// 		{
	// 			GetTree().Paused = true;
	// 		}
	// 	}
	// }
}
