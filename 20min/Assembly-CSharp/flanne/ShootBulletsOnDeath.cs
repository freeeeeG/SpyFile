using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000EA RID: 234
	public class ShootBulletsOnDeath : MonoBehaviour
	{
		// Token: 0x060006D7 RID: 1751 RVA: 0x0001E74F File Offset: 0x0001C94F
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
			this.PF = ProjectileFactory.SharedInstance;
			this.myGun = base.GetComponentInParent<PlayerController>().gun;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001E784 File Offset: 0x0001C984
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001E7A0 File Offset: 0x0001C9A0
		private void OnDeath(object sender, object args)
		{
			GameObject gameObject = (sender as Health).gameObject;
			if (ShootBulletsOnDeath.currentBullets < 3)
			{
				base.StartCoroutine(this.ShootOnDeathCR(gameObject));
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001E7CF File Offset: 0x0001C9CF
		private IEnumerator ShootOnDeathCR(GameObject deathObj)
		{
			ShootBulletsOnDeath.currentBullets++;
			yield return null;
			if (deathObj.tag == "Enemy")
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
					ProjectileRecipe projectileRecipe = this.myGun.GetProjectileRecipe();
					projectileRecipe.size = 0.5f;
					projectileRecipe.knockback *= 0.1f;
					projectileRecipe.projectileSpeed *= 0.35f;
					this.PF.SpawnProjectile(projectileRecipe, direction, deathObj.transform.position, this.damageMultiplier, true);
				}
			}
			yield return null;
			yield return null;
			ShootBulletsOnDeath.currentBullets--;
			yield break;
		}

		// Token: 0x040004A1 RID: 1185
		private const int maxBullets = 3;

		// Token: 0x040004A2 RID: 1186
		private static int currentBullets;

		// Token: 0x040004A3 RID: 1187
		[SerializeField]
		private string bulletOPTag;

		// Token: 0x040004A4 RID: 1188
		[SerializeField]
		private int numOfBullets;

		// Token: 0x040004A5 RID: 1189
		[SerializeField]
		private float damageMultiplier;

		// Token: 0x040004A6 RID: 1190
		private ProjectileFactory PF;

		// Token: 0x040004A7 RID: 1191
		private Gun myGun;
	}
}
