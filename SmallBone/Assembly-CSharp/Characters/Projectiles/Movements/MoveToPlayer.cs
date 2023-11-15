using System;
using System.Runtime.CompilerServices;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007BE RID: 1982
	public class MoveToPlayer : Movement
	{
		// Token: 0x06002851 RID: 10321 RVA: 0x0007A264 File Offset: 0x00078464
		public override void Initialize(IProjectile projectile, float direction)
		{
			this._projectile = projectile;
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._destination = new Vector2(player.transform.position.x, projectile.transform.position.y);
			base.Initialize(projectile, direction);
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x0007A2BC File Offset: 0x000784BC
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			if (base.directionVector.x > 0f && base.projectile.transform.position.x >= this._destination.x)
			{
				this._projectile.Despawn();
			}
			else if (base.directionVector.x < 0f && base.projectile.transform.position.x <= this._destination.x)
			{
				this._projectile.Despawn();
			}
			return new ValueTuple<Vector2, float>(base.directionVector, this._speed * base.projectile.speedMultiplier);
		}

		// Token: 0x040022A7 RID: 8871
		[SerializeField]
		private float _speed = 1f;

		// Token: 0x040022A8 RID: 8872
		private Vector2 _destination;

		// Token: 0x040022A9 RID: 8873
		private IProjectile _projectile;
	}
}
