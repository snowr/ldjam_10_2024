using Godot;

namespace ldjam_2024
{
	[Tool]
	public class Shotgun : Gun
	{
		public override void Fire()
		{
			if (CanFire())
			{
				_isFiring = true;
				_lastFireTime = OS.GetTicksMsec();
				if (_sprite == null) return;
				_sprite.Play("fire");
				Vector2 muzzleOffset = new Vector2(0, 5);
				if (ProjectileScene != null)
				{
					Transform2D currTransform = GlobalTransform;
					Vector2 direction = (currTransform.x).Normalized();
					float rotationAngle = 15;
					Vector2 direction2 = direction.Rotated(Mathf.Deg2Rad(rotationAngle));
					Vector2 direction3 = direction.Rotated(Mathf.Deg2Rad(-rotationAngle));

					// If facing left
					if (direction.x < 0)
					{
						direction2 = direction.Rotated(Mathf.Deg2Rad(-rotationAngle));
						direction3 = direction.Rotated(Mathf.Deg2Rad(rotationAngle));
					}

					Vector2 muzzleGlobal = ToGlobal(MuzzlePos);

					ProjectileManager.Instance.SpawnProjectile(ProjectileScene,
						muzzleGlobal, direction,
						PlayerOwned, this);
					ProjectileManager.Instance.SpawnProjectile(ProjectileScene,
						muzzleGlobal + muzzleOffset, direction2,
						PlayerOwned, this);
					ProjectileManager.Instance.SpawnProjectile(ProjectileScene,
						muzzleGlobal - muzzleOffset, direction3,
						PlayerOwned, this);
				}
			}
			else
			{
				var t = OS.GetTicksMsec() - _lastFireTime >= RateOfFire;
				// GD.Print($"Cant Fire {_isFiring == false}  {t} {OS.GetTicksMsec()} {_lastFireTime} {RateOfFire}");
			}
		}
	}
}
