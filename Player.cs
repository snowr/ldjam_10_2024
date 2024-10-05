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
        private bool InMotion { get; set; }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("move_to"))
            {
                TargetPosition = GetGlobalMousePosition();
                InMotion = true;
                GD.Print("New target position set: ", TargetPosition);
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