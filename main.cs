using Godot;
using ldjam_2024;

public class main : Node2D
{
	public ProjectileManager ProjManager { get; set; }

	public override void _Ready()
	{
		var playerNodes = GetTree().GetNodesInGroup("PlayerCharacter");
        		Player player = null;
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
	}
}
