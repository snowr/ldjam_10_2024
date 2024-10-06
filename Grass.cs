using Godot;

namespace ldjam_2024
{
	[Tool]
	public class Grass : Node2D
	{

		private AnimatedSprite _sprite;

		[Export] public NodePath SpritePath { get; set; }

		public override void _Ready()
		{
			if (Engine.EditorHint)
				return;

			if (SpritePath == null)
				return;

			_sprite = GetNode<AnimatedSprite>(SpritePath);
			_sprite.Play("idle");
		}

	}
}
