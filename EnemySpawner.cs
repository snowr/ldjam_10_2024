using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ldjam_2024
{
    public class EnemySpawner : Node
    {
        public EnemySpawner()
        {
            _rnd = new Random();
            _critterScene = ResourceLoader.Load<PackedScene>("res://Critter1.tscn");
        }

        private Random _rnd;
        protected Timer _timer;
        private int _maxAICount = 10;
        private PackedScene _critterScene;
        public Player Player { get; set; }

        public List<Vector2> SpawnPoints = new List<Vector2>()
        {
            new Vector2(352, 8),
            new Vector2(264, 88),
            new Vector2(184, 200),
            new Vector2(64, 200),
        };

        public override void _Ready()
        {
            _timer = new Timer();
            AddChild(_timer);
            _timer.WaitTime = 2f;
            _timer.OneShot = false;
            _timer.Connect("timeout", this, nameof(OnInterval));
            _timer.Start();

        }

        protected void OnInterval()
        {
            int aiCount = GetParent().GetChildren().Cast<Node>().Count(x => x is AI);
            if (aiCount >= _maxAICount)
                return;
            var instance = _critterScene.Instance<Critter1>();
            instance.Player = Player;
            if (instance == null)
                throw new Exception("oops");
            GetParent().AddChild(instance);
            instance.GlobalPosition = SpawnPoints[_rnd.Next(SpawnPoints.Count - 1)];
        }
    }
}
