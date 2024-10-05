using Godot;

namespace ldjam_2024
{
    [Tool]
    public class Projectile : KinematicBody2D
    {
        [Export] public float Speed { get; set; }
        [Export] public float Damage { get; set; }
        [Export] private NodePath SpritePath { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2 Direction { get; set; }
        public bool InMotion { get; set; }

        public override void _Ready()
        {
            InMotion = true;
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Engine.EditorHint)
                return;
            if (InMotion)
            {
                // GD.Print($"Target is {TargetPosition}");
                // var direction = (Direction - GlobalPosition).Normalized();
                // var direction = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
                var velocity = Direction * Speed;
                velocity = MoveAndSlide(velocity);
            }
        }

        public override void _EnterTree()
        {
            Update();
        }
    }
}