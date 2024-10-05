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
        public float RotationSpeed { get; set; } = 5.0f;

        public Vector2 RotOffset { get; set; } = new Vector2(1, 0);

        [Export] public NodePath PrimaryGunPath { get; set; }

        private bool InMotion { get; set; }

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

                LookAtPosition = GlobalPosition * velocity.Normalized();
                var offsetPosition = GlobalPosition + RotOffset.Rotated(Rotation);

                // var rotAngle= GlobalPosition.AngleToPoint(LookAtPosition) - (0.35f * 2f);
                // var rotAngle = GlobalPosition.AngleToPoint(LookAtPosition);
                var rotAngle = (TargetPosition - offsetPosition).Angle();
                if (GetSlideCount() > 0)
                    Rotation = Mathf.LerpAngle(Rotation, rotAngle, 0);
                else
                    Rotation = Mathf.LerpAngle(Rotation, rotAngle, RotationSpeed * delta);


                if (GlobalPosition.DistanceTo(TargetPosition) <= TargetThresh) InMotion = false;
            }
        }

        private void UpdateDirection()
        {
            if (TargetPosition.x < GlobalPosition.x)
            {
                Scale = new Vector2(-1, 1);
                Rotation = 0;
            }
            else
            {
                var oldRot = Rotation;
                Scale = new Vector2(1, 1);
                Rotation = 0;
            }
        }

        public override void _Draw()
        {
            // Optional: Draw a debug line to show the rotation offset
            DrawLine(Vector2.Zero, RotOffset.Rotated(Rotation), Colors.Red, 2);
        }

        public override void _EnterTree()
        {
            Update();
        }
    }
}