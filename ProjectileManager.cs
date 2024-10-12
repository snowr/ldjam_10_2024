using System;
using Godot;

namespace ldjam_2024
{
    public class ProjectileManager : Node
    {
        private static ProjectileManager _instance;
        public static ProjectileManager Instance => _instance;

        public override void _Ready()
        {
            if (_instance == null)
                _instance = this;
        }

        public void SpawnProjectile(PackedScene scene, Vector2 globalPos, Vector2 direction, bool playerFired,
            Node2D firedFrom)
        {
            var projectile = scene.Instance() as Projectile;
            if (projectile == null)
                throw new Exception("Failed to instantiate projectile");
            AddChild(projectile);
            projectile.PlayerFired = playerFired;
            projectile.GlobalPosition = globalPos;
            projectile.Direction = direction.Normalized();
            projectile.GlobalRotation = firedFrom.GlobalRotation;
        }
    }
}
