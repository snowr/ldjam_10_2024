using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ldjam_2024
{
    public enum AIState
    {
        Idle = 0,
        Attacking
    }

    [Tool]
    public class AI : KinematicBody2D
    {
        protected List<Line2D> lines;

        public AI()
        {
            Rng = new Random();
            AddToGroup("EnemyCharacter");
            AttackLOS = new RayCast2D();
            Casts = new List<RayCast2D>();
            lines = new List<Line2D>();
        }

        public Random Rng { get; set; }
        public Vector2 LookAtPosition { get; set; }
        private Vector2 TargetPosition { get; set; } = Vector2.Zero;
        public Node2D Player { get; set; }
        public float Speed { get; set; } = 100f;
        public float TargetThresh { get; set; } = 2f;
        protected bool InMotion { get; set; }
        [Export] public float AttackRange { get; set; } = 1f;

        public List<RayCast2D> Casts { get; set; }

        protected RayCast2D AttackLOS { get; set; }

        public AIState State { get; set; } = AIState.Idle;

        protected Timer _timer;
        private bool _updateDirection;
        protected Gun _primaryGun;

        [Export] public NodePath PrimaryGunPath { get; set; }

        protected void UpdateAttackLOS()
        {
            if (Player == null) return;
            var dirToPlayer = (Player.GlobalPosition - GlobalPosition).Normalized();
            AttackLOS.CastTo = dirToPlayer * AttackRange;
            AttackLOS.ForceRaycastUpdate();
        }

        public override void _Process(float delta)
        {
        }

        public override void _Ready()
        {
            if (Engine.EditorHint)
                return;
            _timer = new Timer();
            AddChild(_timer);
            _timer.WaitTime = .50f;
            _timer.OneShot = false;
            _timer.Connect("timeout", this, nameof(OnInterval));
            _timer.Start();

            AddChild(AttackLOS);
            AttackLOS.Enabled = true;
            AttackLOS.CollideWithBodies = true;

            if (PrimaryGunPath != null)
            {
                _primaryGun = GetNode<Gun>(PrimaryGunPath);
                if (_primaryGun == null) throw new Exception("Failed to load the primary gun moron.");
                _primaryGun.PlayerOwned = false;
            }
        }

        protected void InitCasts()
        {
            // GD.Print("Critter init casts");
            var step = Mathf.Pi / 8;
            float[] angles = { step, step * 2, step * 3, step * 4, -step, -step * 2, -step * 3, -step * 4 };
            if (lines.Count == 0)
            {
                // foreach (var ang in angles.Take(1))
                foreach (var ang in angles)
                {
                    var vec = CalcDirVector(ang);
                    InitRayCast(-vec, AttackRange);
                    InitRayCast(vec, AttackRange);
                }
            }
        }

        protected void InitRayCast(Vector2 direction, float range)
        {
            var line = new Line2D()
            {
                Width = 2,
                DefaultColor = Colors.Green,
                Points = new[] { Vector2.Zero, direction * range }
            };
            AddChild(line);
            lines.Add(line);
            line.Visible = false;

            var cast = new RayCast2D();
            AddChild(cast);
            Casts.Add(cast);
            cast.Enabled = true;
            cast.CollideWithBodies = true;
            cast.CastTo = direction * range;
            // GD.Print($"Adding cast to as {cast.CastTo}");
        }

        protected bool HasAttackLOSAndRange()
        {
            if (Player == null) return false;
            if (AttackLOS.IsColliding())
            {
                if (AttackLOS.GetCollider() == Player)
                {
                    var distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);
                    return distanceToPlayer <= AttackRange &&
                           Math.Abs(GlobalPosition.y - Player.GlobalPosition.y) <= 2f;
                }

                return false;
            }

            var distance = GlobalPosition.DistanceTo(Player.GlobalPosition);
            // GD.Print($"No collision. Distance to player: {distance}");
            return distance <= AttackRange;
        }

        public Vector2 CalcDirVector(float nRadians)
        {
            // godot auto clamps
            var totalRads = GlobalRotation + nRadians;

            var x = Mathf.Cos(totalRads);
            var y = Mathf.Sin(totalRads);

            return new Vector2(x, y);
        }

        public override void _ExitTree()
        {
            if (AttackLOS != null)
            {
                AttackLOS.QueueFree();
                AttackLOS = null;
            }
        }

        protected void Crawl(float delta)
        {
            if (Casts.Count == 0 || Player == null)
                return;

            var idxMin = 0;
            var found = false;
            for (var i = 0; i < Casts.Count; i++)
            {
                if (Casts[i].IsColliding())
                {
                    object collider = Casts[i].GetCollider();
                    // We colliding with walls bruv
                    if (collider is StaticBody2D) lines[i].DefaultColor = Colors.Red;
                    // GD.Print($"{i} colliding with {collider.GetType()}");
                }
                else
                {
                    if (i != idxMin)
                        lines[i].DefaultColor = Colors.Green;

                    var prevBestDistance = ToGlobal(Casts[idxMin].CastTo).DistanceTo(Player.GlobalPosition);
                    var currentDistance = ToGlobal(Casts[i].CastTo).DistanceTo(Player.GlobalPosition);
                    // GD.Print($"{idxMin}, {i} || {prevBestDistance} --- {currentDistance} ||| {prevBestDistance - currentDistance}");

                    // Does it make sense to you? No? Probably because you have more brain cells than me.
                    // If the current cast distance to the player is better than the current best distance,
                    // or they are the same index, OR if the previous best distance cast is now colliding with something,
                    // set the current cast index as the best. 
                    if (prevBestDistance - currentDistance >= 0f || idxMin == i || Casts[idxMin].IsColliding())
                    {
                        // GD.Print($"Unobstructed cast: {i}");
                        lines[idxMin].DefaultColor = Colors.Green;
                        idxMin = i;
                        lines[idxMin].DefaultColor = Colors.Blue;
                        found = true;
                    }
                }
            }

            if (found)
            {
                var target = Casts[idxMin].ToGlobal(Casts[idxMin].CastTo);
                // TargetPosition = Casts[idxMin].ToGlobal(Casts[idxMin].CastTo);
                if (_updateDirection)
                {
                    // GD.Print("====================");
                    SetTargetPosition(target);
                    _updateDirection = false;
                }

                InMotion = true;
            }
        }

        protected void SetTargetPosition(Vector2 target)
        {
            // GD.Print($"{Player.GlobalPosition.x}, {GlobalPosition.x}");
            TargetPosition = target;

            // below and left
            if (Player.GlobalPosition.x < GlobalPosition.x && Player.GlobalPosition.y > GlobalPosition.y)
            {
                // GD.Print("==1");
                Scale = new Vector2(-1, 1);
                Rotation = 0f;
            }
            // above and left
            else if (Player.GlobalPosition.x < GlobalPosition.x && Player.GlobalPosition.y < GlobalPosition.y)
            {
                // GD.Print("==2");
                Scale = new Vector2(-1, 1);
                Rotation = 0f;
            }
            else if (Player.GlobalPosition.x > GlobalPosition.x)
            {
                // GD.Print("==3");
                Scale = new Vector2(1, 1);
                Rotation = 0f;
            }
        }

        public Vector2 GetTargetPosition()
        {
            return TargetPosition;
        }

        protected void OnInterval()
        {
            _updateDirection = true;
        }
    }
}
