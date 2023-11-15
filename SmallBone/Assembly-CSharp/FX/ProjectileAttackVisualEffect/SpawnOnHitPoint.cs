using System;
using Characters;
using Characters.Projectiles;
using UnityEngine;

namespace FX.ProjectileAttackVisualEffect
{
	// Token: 0x02000287 RID: 647
	public class SpawnOnHitPoint : ProjectileAttackVisualEffect
	{
		// Token: 0x06000CA1 RID: 3233 RVA: 0x00029394 File Offset: 0x00027594
		private void Awake()
		{
			if (this._critical.effect == null)
			{
				this._critical = this._normal;
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x000293B8 File Offset: 0x000275B8
		public override void SpawnDespawn(IProjectile projectile)
		{
			if (!this._spawnOnDespawn)
			{
				return;
			}
			Vector3 position = (this._spawnPosition == null) ? projectile.transform.position : this._spawnPosition.position;
			this._normal.Spawn(position, 0f, 1f);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002940C File Offset: 0x0002760C
		public override void SpawnExpire(IProjectile projectile)
		{
			if (!this._spawnOnExpire)
			{
				return;
			}
			Vector3 position = (this._spawnPosition == null) ? projectile.transform.position : this._spawnPosition.position;
			this._normal.Spawn(position, 0f, 1f);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00029460 File Offset: 0x00027660
		public override void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			if (!this._spawnOnTerrainHit)
			{
				return;
			}
			this._normal.Spawn(raycastHit.point, projectile.owner, 0f, 1f).transform.localScale.Scale(projectile.transform.localScale);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x000294BC File Offset: 0x000276BC
		public override void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
		{
			if ((this._spawnOnCharacterHit && target.character != null) || (this._spawnOnDamageableHit && target.damageable != null))
			{
				EffectPoolInstance effectPoolInstance = (damage.critical ? this._critical : this._normal).Spawn(raycastHit.point, projectile.owner, 0f, 1f);
				if (effectPoolInstance != null)
				{
					effectPoolInstance.transform.localScale.Scale(projectile.transform.localScale);
				}
			}
		}

		// Token: 0x04000ADA RID: 2778
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04000ADB RID: 2779
		[Header("Spawn map")]
		[SerializeField]
		private bool _spawnOnDespawn;

		// Token: 0x04000ADC RID: 2780
		[SerializeField]
		private bool _spawnOnExpire = true;

		// Token: 0x04000ADD RID: 2781
		[SerializeField]
		private bool _spawnOnTerrainHit = true;

		// Token: 0x04000ADE RID: 2782
		[SerializeField]
		private bool _spawnOnCharacterHit = true;

		// Token: 0x04000ADF RID: 2783
		[SerializeField]
		private bool _spawnOnDamageableHit = true;

		// Token: 0x04000AE0 RID: 2784
		[Header("Effects")]
		[SerializeField]
		private EffectInfo _normal;

		// Token: 0x04000AE1 RID: 2785
		[SerializeField]
		private EffectInfo _critical;
	}
}
