using Godot;

namespace ldjam_2024
{
    public class Critter1 : AI
    {
        public Vector2 RotOffset { get; set; } = new Vector2(16, 0);

        public float RotationSpeed { get; set; } = 5.0f;

        public override void _Ready()
        {
            base._Ready();
            // GetRandomTarget();
            Speed = 10f;
        }

        // private void GetRandomTarget()
        // {
        // 	var playArea = GetViewport().GetVisibleRect();
        // 	// Next double behaviour is to generate between 0 and 1 and then we scale it by the dimensions
        // 	var rX = (float)(Rng.NextDouble() * playArea.Size.x) + playArea.Position.x;
        // 	var rY = (float)(Rng.NextDouble() * playArea.Size.y) + playArea.Position.y;
        // 	TargetPosition = new Vector2(rX, rY);
        // 	InMotion = true;
        // }

        public override void _PhysicsProcess(float delta)
        {
            if (lines.Count == 0) InitCasts();

            UpdateAttackLOS();
            if (HasAttackLOSAndRange())
            {
                GD.Print($"Attacking player with range {AttackRange}");
                State = AIState.Attacking;
            }
            else
                State = AIState.Idle;

            if (InMotion)
            {
                // GD.Print($"Target is {TargetPosition}");
                var direction = (GetTargetPosition() - GlobalPosition).Normalized();
                var velocity = direction * Speed;

                velocity = MoveAndSlide(velocity);

                // if (State != AIState.Attacking)
                if(false)
                // if (true)
                {
                    var offsetPosition = GlobalPosition + RotOffset.Rotated(Rotation);

                    var rotAngle = (Player.GlobalPosition - offsetPosition).Angle();
                    // var rotAngle = (Player.GlobalPosition).Angle();
                    if (GetSlideCount() > 0)
                    {
                        // Rotation = Mathf.Clamp(Mathf.LerpAngle(Rotation, rotAngle, 0), -Mathf.Pi/4, Mathf.Pi/4);
                        Rotation = Mathf.LerpAngle(Rotation, rotAngle, 0);
                    }
                    else
                    {
                        // Rotation = Mathf.Clamp(Mathf.LerpAngle(Rotation, rotAngle, RotationSpeed * delta), -Mathf.Pi/4, Mathf.Pi/4);
                        Rotation = Mathf.LerpAngle(Rotation, rotAngle, RotationSpeed * delta);
                    }
                }

                if (GlobalPosition.DistanceTo(GetTargetPosition()) <= TargetThresh) InMotion = false;
            }

            Crawl(delta);
        }
    }
}