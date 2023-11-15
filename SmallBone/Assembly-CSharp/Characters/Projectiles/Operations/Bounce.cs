using System;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000771 RID: 1905
	public class Bounce : HitOperation
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002758 RID: 10072 RVA: 0x0007600C File Offset: 0x0007420C
		// (set) Token: 0x06002759 RID: 10073 RVA: 0x00076014 File Offset: 0x00074214
		public Collider2D lastCollision { get; set; }

		// Token: 0x0600275A RID: 10074 RVA: 0x00076020 File Offset: 0x00074220
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			if (raycastHit.collider == this.lastCollision)
			{
				return;
			}
			this.lastCollision = raycastHit.collider;
			Vector2 point = raycastHit.point;
			float speed = projectile.speed;
			Vector2 direction = new Vector2(-projectile.direction.x, projectile.direction.y);
			Vector2 vector = new Vector2(projectile.direction.x, -projectile.direction.y);
			if (Physics2D.Raycast(point, direction, speed, this._terrainLayer))
			{
				if (vector.y > 0f)
				{
					this._terrainBottomHitEffect.Spawn(raycastHit.point, projectile.owner, 0f, 1f);
				}
				else
				{
					this._terrainTopHitEffect.Spawn(raycastHit.point, projectile.owner, 0f, 1f);
				}
				projectile.direction = vector;
				return;
			}
			if (!Physics2D.Raycast(point, vector, speed, this._terrainLayer))
			{
				projectile.direction = new Vector2(-projectile.direction.x, -projectile.direction.y);
				return;
			}
			projectile.direction = direction;
			if (vector.x > 0f)
			{
				this._terrainRightHitEffect.Spawn(raycastHit.point, projectile.owner, 0f, 1f);
				return;
			}
			this._terrainLeftHitEffect.Spawn(raycastHit.point, projectile.owner, 0f, 1f);
		}

		// Token: 0x04002172 RID: 8562
		[SerializeField]
		private LayerMask _terrainLayer = Layers.terrainMaskForProjectile;

		// Token: 0x04002173 RID: 8563
		[SerializeField]
		private EffectInfo _terrainLeftHitEffect;

		// Token: 0x04002174 RID: 8564
		[SerializeField]
		private EffectInfo _terrainRightHitEffect;

		// Token: 0x04002175 RID: 8565
		[SerializeField]
		private EffectInfo _terrainTopHitEffect;

		// Token: 0x04002176 RID: 8566
		[SerializeField]
		private EffectInfo _terrainBottomHitEffect;
	}
}
