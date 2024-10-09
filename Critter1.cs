using Godot;

namespace ldjam_2024
{
	public class Critter1 : AI
	{
		public Vector2 RotOffset { get; set; } = new Vector2(16, 0);

		public float RotationSpeed { get; set; } = 5.0f;

		public override void _Ready()
		{
			// GD.Print("Critter waking up");
			base._Ready();
			// GetRandomTarget();
			Speed = 10f;
		}

		public override void _PhysicsProcess(float delta)
		{
			if (Engine.EditorHint)
				return;
			if (lines.Count == 0) InitCasts();

			UpdateAttackLOS();
			if (HasAttackLOSAndRange())
			{
				// GD.Print($"Attacking player with range {AttackRange}");
				State = AIState.Attacking;
			}
			else
				State = AIState.Idle;

			if (State == AIState.Attacking && _primaryGun != null)
			{
				_primaryGun.Fire();
			}

			if (InMotion)
			{
				// GD.Print($"Target is {TargetPosition}");
				var direction = (GetTargetPosition() - GlobalPosition).Normalized();
				var velocity = direction * Speed;

				velocity = MoveAndSlide(velocity);

				// if (State != AIState.Attacking)
				if (GlobalPosition.DistanceTo(GetTargetPosition()) <= TargetThresh) InMotion = false;
			}

			Crawl(delta);
		}
	}
}
