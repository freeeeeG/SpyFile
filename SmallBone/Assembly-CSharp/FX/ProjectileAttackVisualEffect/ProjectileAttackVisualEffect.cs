using System;
using Characters;
using Characters.Projectiles;
using UnityEditor;
using UnityEngine;

namespace FX.ProjectileAttackVisualEffect
{
	// Token: 0x02000284 RID: 644
	public abstract class ProjectileAttackVisualEffect : VisualEffect
	{
		// Token: 0x06000C95 RID: 3221
		public abstract void SpawnDespawn(IProjectile projectile);

		// Token: 0x06000C96 RID: 3222
		public abstract void SpawnExpire(IProjectile projectile);

		// Token: 0x06000C97 RID: 3223
		public abstract void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		// Token: 0x06000C98 RID: 3224
		public abstract void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target);

		// Token: 0x04000AD9 RID: 2777
		public static readonly Type[] types = new Type[]
		{
			typeof(SpawnOnHitPoint)
		};

		// Token: 0x02000285 RID: 645
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000C9B RID: 3227 RVA: 0x000292B1 File Offset: 0x000274B1
			public SubcomponentAttribute() : base(true, ProjectileAttackVisualEffect.types)
			{
			}
		}

		// Token: 0x02000286 RID: 646
		[Serializable]
		public class Subcomponents : SubcomponentArray<ProjectileAttackVisualEffect>
		{
			// Token: 0x06000C9C RID: 3228 RVA: 0x000292C0 File Offset: 0x000274C0
			public void SpawnDespawn(IProjectile projectile)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].SpawnDespawn(projectile);
				}
			}

			// Token: 0x06000C9D RID: 3229 RVA: 0x000292F0 File Offset: 0x000274F0
			public void SpawnExpire(IProjectile projectile)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].SpawnExpire(projectile);
				}
			}

			// Token: 0x06000C9E RID: 3230 RVA: 0x00029320 File Offset: 0x00027520
			public void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Spawn(projectile, origin, direction, distance, raycastHit);
				}
			}

			// Token: 0x06000C9F RID: 3231 RVA: 0x00029354 File Offset: 0x00027554
			public void Spawn(IProjectile projectile, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Damage damage, ITarget target)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Spawn(projectile, origin, direction, distance, raycastHit, damage, target);
				}
			}
		}
	}
}
