using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000122 RID: 290
	public class BulletGlyph : Spawn
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00021C71 File Offset: 0x0001FE71
		private float damageMultiplier
		{
			get
			{
				return this.player.stats[StatType.SummonDamage].Modify(this.baseDamageMultiplier);
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00021C8F File Offset: 0x0001FE8F
		private void Start()
		{
			this.PF = ProjectileFactory.SharedInstance;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00021C9C File Offset: 0x0001FE9C
		private void OnEnable()
		{
			base.StartCoroutine(this.LifetimeCR());
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00021CAC File Offset: 0x0001FEAC
		private void ShootBullets()
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
				projectileRecipe.size *= 0.5f;
				projectileRecipe.knockback *= 0.1f;
				this.PF.SpawnProjectile(projectileRecipe, direction, base.transform.position, this.damageMultiplier, true);
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00021D76 File Offset: 0x0001FF76
		private IEnumerator LifetimeCR()
		{
			yield return new WaitForSeconds(this.timeToActivate - 0.1f);
			this.knockbackObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			this.ShootBullets();
			this.knockbackObject.SetActive(false);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x040005C8 RID: 1480
		[SerializeField]
		private float timeToActivate;

		// Token: 0x040005C9 RID: 1481
		[SerializeField]
		private int numOfBullets;

		// Token: 0x040005CA RID: 1482
		[SerializeField]
		private float baseDamageMultiplier;

		// Token: 0x040005CB RID: 1483
		[SerializeField]
		private GameObject knockbackObject;

		// Token: 0x040005CC RID: 1484
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040005CD RID: 1485
		private ProjectileFactory PF;
	}
}
