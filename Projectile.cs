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
        public bool PlayerFired { get; set; }

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
                var velocity = Direction * Speed;
                var col = MoveAndCollide(velocity * delta);

                if (col != null)
                {
                    object body = col.Collider;
                    if (body is StaticBody2D)
                    {
                        // We hit a wall. Peace out of this scene
                    }

                    if (body is AI aiBody && PlayerFired)
                    {
                        // We hit an enemy AI.
                        aiBody.QueueFree();
                    }

                    QueueFree();
                }
            }
        }

        public override void _EnterTree()
        {
            Update();
        }
    }
}