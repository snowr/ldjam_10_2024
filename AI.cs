using System;
using Godot;

namespace ldjam_2024
{
    [Tool]
    public class AI : KinematicBody2D
    {
        public AI()
        {
            Rng = new Random();
            AddToGroup("EnemyCharacter");
            AttackLOS = new RayCast2D();
        }

        public Random Rng { get; set; }
        public Vector2 LookAtPosition { get; set; }
        public Vector2 TargetPosition { get; set; }
        public Node2D Player { get; set; }
        public float Speed { get; set; } = 100f;
        public float TargetThresh { get; set; } = 2f;
        protected bool InMotion { get; set; }
        [Export] public float AttackRange { get; set; } = 1f;

        protected RayCast2D AttackLOS { get; set; }

        protected void UpdateAttackLOS()
        {
            if (Player == null) return;
            var dirToPlayer = (Player.GlobalPosition - GlobalPosition).Normalized();
            AttackLOS.CastTo = dirToPlayer * AttackRange;
            AttackLOS.ForceRaycastUpdate();
        }

        public override void _Ready()
        {
            AddChild(AttackLOS);
            AttackLOS.Enabled = true;
            AttackLOS.CollideWithAreas = true;
            AttackLOS.CollideWithBodies = true;
        }

        protected bool HasAttackLOS()
        {
            if (Player == null) return false;
            if (AttackLOS.IsColliding())
            {
                if (AttackLOS.GetCollider() == Player)
                {
                    var distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);
                    return distanceToPlayer <= AttackRange;
                }

                return false;
            }

            var distance = GlobalPosition.DistanceTo(Player.GlobalPosition);
            // GD.Print($"No collision. Distance to player: {distance}");
            return distance <= AttackRange;
        }

        public override void _ExitTree()
        {
            if (AttackLOS != null)
            {
                AttackLOS.QueueFree();
                AttackLOS = null;
            }
        }
    }
}