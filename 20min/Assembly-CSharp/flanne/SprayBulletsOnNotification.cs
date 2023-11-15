using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000117 RID: 279
	public class SprayBulletsOnNotification : MonoBehaviour
	{
		// Token: 0x060007BC RID: 1980 RVA: 0x000213D0 File Offset: 0x0001F5D0
		private void OnNotification(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			this.ShootBullets(gameObject.transform.position);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000213F5 File Offset: 0x0001F5F5
		private void Start()
		{
			this.PF = ProjectileFactory.SharedInstance;
			this.player = base.GetComponentInParent<PlayerController>();
			this.AddObserver(new Action<object, object>(this.OnNotification), this.notification);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00021426 File Offset: 0x0001F626
		private void OnDisable()
		{
			this.RemoveObserver(new Action<object, object>(this.OnNotification), this.notification);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00021440 File Offset: 0x0001F640
		private void ShootBullets(Vector3 spawnPos)
		{
			Vector2 zero = Vector2.zero;
			while (zero == Vector2.zero)
			{
				zero = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			}
			for (int i = 0; i < this.numOfBullets; i++)
			{
				float degrees = (float)i / (float)this.numOfBullets * 360f;
				Vector2 direction = zero.Rotate(degrees);
				ProjectileRecipe projectileRecipe = this.player.gun.GetProjectileRecipe();
				projectileRecipe.knockback *= 0.1f;
				projectileRecipe.piercing++;
				this.PF.SpawnProjectile(projectileRecipe, direction, spawnPos, this.damageMultiplier, false);
			}
		}

		// Token: 0x04000592 RID: 1426
		[SerializeField]
		private string notification;

		// Token: 0x04000593 RID: 1427
		[SerializeField]
		private int numOfBullets;

		// Token: 0x04000594 RID: 1428
		[SerializeField]
		private float damageMultiplier;

		// Token: 0x04000595 RID: 1429
		private ProjectileFactory PF;

		// Token: 0x04000596 RID: 1430
		private PlayerController player;
	}
}
