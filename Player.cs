using Godot;

namespace ldjam_2024
{
	[Tool]
	public class Player : KinematicBody2D
	{
		private Gun _primaryGun;

		public Player()
		{
			AddToGroup("PlayerCharacter");
		}

		public Vector2 LookAtPosition { get; set; }
		public Vector2 TargetPosition { get; set; }
		public float Speed { get; set; } = 100f;
		public float TargetThresh { get; set; } = 2f;

		[Export] public NodePath PrimaryGunPath { get; set; }

		private bool InMotion { get; set; }
		bool _directionUpdated;

		public override void _Ready()
		{
			if (PrimaryGunPath != null)
			{
				_primaryGun = GetNode<Gun>(PrimaryGunPath);
				if (_primaryGun == null) GD.PushError("Failed to load the primary gun moron.");
			}
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("move_to"))
			{
				TargetPosition = GetGlobalMousePosition();
				InMotion = true;
				GD.Print("New target position set: ", TargetPosition);
				_directionUpdated = false;
				UpdateDirection();
			}

			if (@event.IsAction("primary_fire"))
				if (_primaryGun != null)
					_primaryGun.Fire();
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

		private void UpdateDirection()
		{
			if (TargetPosition.x < GlobalPosition.x)
			{
				GD.Print($"Clicked to the left, before trans {Scale}");
				Scale = new Vector2(-1, 1);
				Rotation = 0;
			}
			else
			{
				var oldRot = Rotation;
				GD.Print($"after left flip now flipping back to right Rotation {Rotation}");
				Scale = new Vector2(1, 1);
				Rotation = 0;
			}

			GD.Print($"Player Scale: {Scale}, Rotation: {Rotation}");
		}

		public override void _EnterTree()
		{
			Update();
		}
	}
}
