using Godot;

namespace ldjam_2024
{
	[Tool]
	public class Player : KinematicBody2D
	{
		public Player()
		{
			AddToGroup("PlayerCharacter");
		}

		public Vector2 LookAtPosition { get; set; }
		public Vector2 TargetPosition { get; set; }
		public float Speed { get; set; } = 100f;
		public float TargetThresh { get; set; } = 2f;

		[Export] public NodePath PrimaryGunPath { get; set; }
		private Gun _primaryGun;

		private bool InMotion { get; set; }

		public override void _Ready()
		{
			if (PrimaryGunPath != null)
			{
				_primaryGun = GetNode<Gun>(PrimaryGunPath);
				if (_primaryGun == null)
				{
					GD.PushError("Failed to load the primary gun moron.");
				}
			}
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("move_to"))
			{
				TargetPosition = GetGlobalMousePosition();
				InMotion = true;
				GD.Print("New target position set: ", TargetPosition);
			}

			if (@event.IsAction("primary_fire"))
			{
				if (_primaryGun != null)
				{
					_primaryGun.Fire();
				}
			}
		}

		public override void _Process(float delta)
		{
		}

		public override void _PhysicsProcess(float delta)
		{
			if (InMotion)
			{
				var direction = (TargetPosition - GlobalPosition).Normalized();
				var velocity = direction * Speed;

				velocity = MoveAndSlide(velocity);

				if (GlobalPosition.DistanceTo(TargetPosition) <= TargetThresh) InMotion = false;

				LookAtPosition = GlobalPosition * velocity.Normalized();
			}
		}

		public override void _EnterTree()
		{
			Update();
		}
	}
}
