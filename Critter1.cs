using Godot;

namespace ldjam_2024
{
    public class Critter1 : AI
    {
        public override void _Ready()
        {
            base._Ready();
            GetRandomTarget();
            AttackRange = 100f;
        }

        private void GetRandomTarget()
        {
            var playArea = GetViewport().GetVisibleRect();
            // Next double behaviour is to generate between 0 and 1 and then we scale it by the dimensions
            var rX = (float)(Rng.NextDouble() * playArea.Size.x) + playArea.Position.x;
            var rY = (float)(Rng.NextDouble() * playArea.Size.y) + playArea.Position.y;
            TargetPosition = new Vector2(rX, rY);
            InMotion = true;
        }

        public override void _PhysicsProcess(float delta)
        {
            UpdateAttackLOS();
            if (HasAttackLOS())
            {
                // GD.Print("Attacking player");
            }

            if (!InMotion) GetRandomTarget();
            if (InMotion)
            {
                var direction = (TargetPosition - GlobalPosition).Normalized();
                var velocity = direction * Speed;

                velocity = MoveAndSlide(velocity);

                if (GlobalPosition.DistanceTo(TargetPosition) <= TargetThresh) InMotion = false;

                LookAtPosition = GlobalPosition * velocity.Normalized();
            }
        }
    }
}