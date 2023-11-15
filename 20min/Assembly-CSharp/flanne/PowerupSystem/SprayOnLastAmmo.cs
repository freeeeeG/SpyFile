using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystem
{
	// Token: 0x02000254 RID: 596
	public class SprayOnLastAmmo : MonoBehaviour
	{
		// Token: 0x06000CF5 RID: 3317 RVA: 0x0002F408 File Offset: 0x0002D608
		private void Start()
		{
			this.PF = ProjectileFactory.SharedInstance;
			this.player = base.GetComponentInParent<PlayerController>();
			this.myGun = this.player.gun;
			this.ammo = this.player.ammo;
			this.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002F46A File Offset: 0x0002D66A
		private void OnDestroy()
		{
			this.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0002F488 File Offset: 0x0002D688
		private void OnAmmoChanged(int ammoAmount)
		{
			if (ammoAmount == 0)
			{
				base.StartCoroutine(this.SprayBulletsCR());
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002F49A File Offset: 0x0002D69A
		private IEnumerator SprayBulletsCR()
		{
			Vector2 startDirection = Vector2.zero;
			while (startDirection == Vector2.zero)
			{
				startDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			}
			yield return new WaitForSeconds(this.myGun.shotCooldown);
			int num;
			for (int i = 0; i < this.numOfBullets; i = num + 1)
			{
				float degrees = (float)i / (float)this.numOfBullets * 360f;
				Vector2 direction = startDirection.Rotate(degrees);
				this.PF.SpawnProjectile(this.myGun.GetProjectileRecipe(), direction, base.transform.position, this.damageMultiplier, false);
				SoundEffectSO gunshotSFX = this.myGun.gunData.gunshotSFX;
				if (gunshotSFX != null)
				{
					gunshotSFX.Play(null);
				}
				yield return new WaitForSeconds(this.delayBetweenShots);
				num = i;
			}
			yield break;
		}

		// Token: 0x0400093A RID: 2362
		[SerializeField]
		private string bulletOPTag;

		// Token: 0x0400093B RID: 2363
		[SerializeField]
		private int numOfBullets;

		// Token: 0x0400093C RID: 2364
		[SerializeField]
		private float damageMultiplier;

		// Token: 0x0400093D RID: 2365
		[SerializeField]
		private float delayBetweenShots;

		// Token: 0x0400093E RID: 2366
		private ProjectileFactory PF;

		// Token: 0x0400093F RID: 2367
		private PlayerController player;

		// Token: 0x04000940 RID: 2368
		private Gun myGun;

		// Token: 0x04000941 RID: 2369
		private Ammo ammo;
	}
}
