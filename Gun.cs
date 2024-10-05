using Godot;

namespace ldjam_2024
{
    [Tool]
    public class Gun : Node2D
    {
        private bool _isFiring;
        private ulong _lastFireTime;
        private AnimatedSprite _sprite;

        [Export] public float RateOfFire { get; set; } = 1000f; // milliseconds

        [Export] public NodePath SpritePath { get; set; }

        public override void _Ready()
        {
            if (SpritePath == null)
                return;

            _sprite = GetNode<AnimatedSprite>(SpritePath);
            if (_sprite == null)
                GD.PushError("Failed to load sprite for Gun. You idiot.");
            else
                _sprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
        }

        public bool CanFire()
        {
            var time = OS.GetTicksMsec();
            if (_lastFireTime == 0)
                return true;
            return _isFiring == false && time - _lastFireTime >= RateOfFire;
        }

        public void Fire()
        {
            if (CanFire())
            {
                GD.Print("Firing ============");
                _isFiring = true;
                _lastFireTime = OS.GetTicksMsec();
                if (_sprite == null) return;
                _sprite.Play("fire");
            }
            else
            {
                var t = OS.GetTicksMsec() - _lastFireTime >= RateOfFire;
                GD.Print($"Cant Fire {_isFiring == false}  {t} {OS.GetTicksMsec()} {_lastFireTime} {RateOfFire}");
            }
        }

        private void OnAnimationFinished()
        {
            if (_sprite.Animation == "fire")
            {
                _isFiring = false;
                // TODO: If we want idle animation, switch to it here and switch the state
                _sprite.Play("idle");
            }
        }

        public override void _EnterTree()
        {
            Update();
        }
    }
}