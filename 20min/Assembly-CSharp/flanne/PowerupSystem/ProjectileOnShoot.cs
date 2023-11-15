using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x0200024F RID: 591
	public class ProjectileOnShoot : AttackOnShoot
	{
		// Token: 0x06000CE0 RID: 3296 RVA: 0x0002EDD8 File Offset: 0x0002CFD8
		protected override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.projectilePrefab.name, this.projectilePrefab, 50, true);
			this.SC = ShootingCursor.Instance;
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.gun = componentInParent.gun;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0002EE30 File Offset: 0x0002D030
		public override void Attack()
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 b = base.transform.position;
			Vector2 v = a - b;
			float num = -1f * this.inaccuracy / 2f;
			float max = -1f * num;
			if (this.numProjectiles > 1)
			{
				for (int i = 0; i < this.numProjectiles; i++)
				{
					float degrees = num + (float)i / (float)(this.numProjectiles - 1) * this.inaccuracy;
					Vector2 direction = v.Rotate(degrees);
					this.SpawnProjectile(direction);
				}
				return;
			}
			Vector2 direction2 = v.Rotate(Random.Range(num, max));
			this.SpawnProjectile(direction2);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0002EEF0 File Offset: 0x0002D0F0
		private void SpawnProjectile(Vector2 direction)
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.projectilePrefab.name);
			pooledObject.SetActive(true);
			Projectile component = pooledObject.GetComponent<Projectile>();
			component.vector = this.speed * direction.normalized;
			component.angle = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
			if (this.setToGunDamage)
			{
				component.damage = this.percentOfGunDamage * this.gun.damage;
			}
			Vector2 vector = direction.normalized * 0.7f;
			pooledObject.transform.position = base.transform.position + new Vector3(vector.x, vector.y, 0f);
			if (this.projHasPeriodicDamage)
			{
				PeriodicGameObjectActivator component2 = pooledObject.GetComponent<PeriodicGameObjectActivator>();
				if (component2 != null)
				{
					component2.timeBetweenActivations = this.periodicDamageFrequency;
				}
			}
		}

		// Token: 0x0400091C RID: 2332
		[SerializeField]
		private GameObject projectilePrefab;

		// Token: 0x0400091D RID: 2333
		[SerializeField]
		private float speed = 20f;

		// Token: 0x0400091E RID: 2334
		[SerializeField]
		private float inaccuracy = 45f;

		// Token: 0x0400091F RID: 2335
		public int numProjectiles = 1;

		// Token: 0x04000920 RID: 2336
		[SerializeField]
		private bool setToGunDamage;

		// Token: 0x04000921 RID: 2337
		[SerializeField]
		private float percentOfGunDamage;

		// Token: 0x04000922 RID: 2338
		[SerializeField]
		private bool projHasPeriodicDamage;

		// Token: 0x04000923 RID: 2339
		public float periodicDamageFrequency;

		// Token: 0x04000924 RID: 2340
		private ObjectPooler OP;

		// Token: 0x04000925 RID: 2341
		private ShootingCursor SC;

		// Token: 0x04000926 RID: 2342
		private Gun gun;
	}
}
