using Godot;
using ldjam_2024;

public class main : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var playerNodes = GetTree().GetNodesInGroup("PlayerCharacter");
        Player player = null;
        if (playerNodes.Count > 0) player = (Player)playerNodes[0];

        if (player != null)
        {
            var enemyNodes = GetTree().GetNodesInGroup("EnemyCharacter");
            foreach (AI enemy in enemyNodes) enemy.Player = player;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}