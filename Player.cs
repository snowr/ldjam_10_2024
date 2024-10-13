using System;
using Godot;

namespace ldjam_2024
{
	[Tool]
	public class Player : KinematicBody2D
	{
		protected Gun _primaryGun;
		protected Gun _weapon1;
		protected Gun _weapon2;

		public Vector2 Velocity = Vector2.Zero;

		public Player()
		{
			AddToGroup("PlayerCharacter");
		}

		// public Vector2 LookAtPosition { get; set; }
		public Vector2 TargetPosition { get; set; }
		public float Speed { get; set; } = 50f;
		public float TargetThresh { get; set; } = 2f;
		public float RotationSpeed { get; set; } = 5.0f;

		// public Vector2 RotOffset { get; set; } = new Vector2(1, 0);
		public Vector2 RotOffset { get; set; } = new Vector2(0, 0);

		[Export] public NodePath Weapon1Path { get; set; }
		[Export] public NodePath Weapon2Path { get; set; }
		[Export] public NodePath HealthBarComponent { get; set; }

		private HealthBar HealthBar { get; set; }

		private bool InMotion { get; set; }

		public bool Dead => _health <= 0;

		private int _health;
		private int _maxHealth;

		public override void _Ready()
		{
			if (Engine.EditorHint)
				return;
			_health = 100;
			_maxHealth = 100;

			if (HealthBarComponent != null)
			{
				HealthBar = GetNode<HealthBar>(HealthBarComponent);
			}

			if (Weapon1Path != null)
			{
				_weapon1 = GetNode<Gun>(Weapon1Path);
				if (_weapon1 == null) throw new Exception("Failed to load the primary gun moron.");
				_weapon1.PlayerOwned = true;
				_primaryGun = _weapon1;
			}

			if (Weapon2Path != null)
			{
				_weapon2 = GetNode<Gun>(Weapon2Path);
				if (_weapon2 == null) throw new Exception("Failed to load weapon 2.");
				_weapon2.PlayerOwned = true;
				_weapon2.Visible = false;
			}
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (Engine.EditorHint)
				return;

			if (Dead)
				return;

			// if (@event.IsActionPressed("move_to"))
			// {
			// 	TargetPosition = GetGlobalMousePosition();
			// 	InMotion = true;
			// 	UpdateDirection();
			// }

			if (@event is InputEventKey keyEvent)
			{
				UpdateVelocity(keyEvent.IsAction("move_right"), ref Velocity.x, 1, keyEvent.Pressed);
				UpdateVelocity(keyEvent.IsAction("move_left"), ref Velocity.x, -1, keyEvent.Pressed);
				UpdateVelocity(keyEvent.IsAction("move_down"), ref Velocity.y, 1, keyEvent.Pressed);
				UpdateVelocity(keyEvent.IsAction("move_up"), ref Velocity.y, -1, keyEvent.Pressed);
			}

			// if(@event.IsAction("move_right") && Velocity.x 

			if (@event.IsAction("primary_fire"))
			{
				if (_primaryGun != null)
					_primaryGun.Fire();
			}

			if (@event.IsActionPressed("weapon_slot_1"))
			{
				_primaryGun.Visible = false;
				_primaryGun = _weapon1;
				_primaryGun.Visible = true;
			}

			if (@event.IsActionPressed("weapon_slot_2"))
			{
				_primaryGun.Visible = false;
				_primaryGun = _weapon2;
				_primaryGun.Visible = true;
			}
		}

		private void UpdateVelocity(bool isKeyPressed, ref float velocityComponent, float direction, bool isPressed)
		{
			if (isKeyPressed)
			{
				velocityComponent = isPressed ? direction : 0;
			}
		}

		public override void _Process(float delta)
		{
		}

		public override void _PhysicsProcess(float delta)
		{
			if (Engine.EditorHint)
				return;

			if (Dead)
				return;
			// if (InMotion)
			// {
			// 	var direction = (TargetPosition - GlobalPosition).Normalized();
			// 	var velocity = direction * Speed;
			//
			// 	velocity = MoveAndSlide(velocity);
			//
			// 	var offsetPosition = GlobalPosition + RotOffset.Rotated(Rotation);
			//
			// 	if (GlobalPosition.DistanceTo(TargetPosition) <= TargetThresh) InMotion = false;
			// }
			MoveAndSlide(Velocity.Normalized() * Speed);
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
			DrawLine(Vector2.Zero, RotOffset.Rotated(Rotation), Colors.Red, 2);
		}

		public override void _EnterTree()
		{
			Update();
		}

		public void Hurt(int damage)
		{
			if (Dead)
				return;
			var oldHealth = _health;
			_health = Mathf.Clamp(_health - damage, 0, _maxHealth);
			// GD.Print($"took {damage} damage and health is {_health} prev value {oldHealth}.");
			if (HealthBar != null)
			{
				for (var i = 100; i > _health; i -= 10)
				{
					var idx = (i - 10) / 10;
					HealthBar.Ticks[idx].Visible = false;
				}
			}
		}
	}
}
