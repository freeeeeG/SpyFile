using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x02000110 RID: 272
	public class Shooter : MonoBehaviour
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00020F79 File Offset: 0x0001F179
		public virtual bool fireOnStop
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00020F7C File Offset: 0x0001F17C
		private void Start()
		{
			this.PF = ProjectileFactory.SharedInstance;
			this.OP = ObjectPooler.SharedInstance;
			this.Init();
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00002F51 File Offset: 0x00001151
		public virtual void Init()
		{
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00002F51 File Offset: 0x00001151
		public virtual void OnStopShoot(ProjectileRecipe recipe, Vector2 pointDirection, int numProjectiles, float spread, float inaccuracy)
		{
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00020F9C File Offset: 0x0001F19C
		public virtual void Shoot(ProjectileRecipe recipe, Vector2 pointDirection, int numProjectiles, float spread, float inaccuracy)
		{
			this.PostNotification(Shooter.TweakProjectileRecipe, recipe);
			pointDirection = this.RandomizeDirection(pointDirection, inaccuracy);
			if (numProjectiles > 1)
			{
				spread = Mathf.Max(spread, 5f);
				float num = -1f * (spread / 2f);
				for (int i = 0; i < numProjectiles; i++)
				{
					float degrees = num + (float)i / (float)(numProjectiles - 1) * spread;
					Vector2 direction = pointDirection.Rotate(degrees);
					Projectile e = this.PF.SpawnProjectile(recipe, direction, base.transform.position, 1f, false);
					this.PostNotification(Shooter.BulletShotEvent, e);
				}
			}
			else
			{
				Projectile e2 = this.PF.SpawnProjectile(recipe, pointDirection, base.transform.position, 1f, false);
				this.PostNotification(Shooter.BulletShotEvent, e2);
			}
			if (this.muzzleFlashObject != null)
			{
				GameObject gameObject = this.muzzleFlashObject;
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			UnityEvent unityEvent = this.onShoot;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00021094 File Offset: 0x0001F294
		private Vector3 RandomizeDirection(Vector2 direction, float degrees)
		{
			float num = -1f * degrees / 2f;
			float min = -1f * num;
			Vector2 vector = new Vector2(direction.x, direction.y);
			vector = vector.Rotate(Random.Range(min, num));
			return new Vector3(vector.x, vector.y, 0f);
		}

		// Token: 0x0400057D RID: 1405
		public static string BulletShotEvent = "Shooter.BulletShotEvent";

		// Token: 0x0400057E RID: 1406
		public static string TweakProjectileRecipe = "Shooter.TweakProjectileRecipe";

		// Token: 0x0400057F RID: 1407
		[SerializeField]
		private GameObject muzzleFlashObject;

		// Token: 0x04000580 RID: 1408
		public UnityEvent onShoot;

		// Token: 0x04000581 RID: 1409
		protected ProjectileFactory PF;

		// Token: 0x04000582 RID: 1410
		protected ObjectPooler OP;
	}
}
