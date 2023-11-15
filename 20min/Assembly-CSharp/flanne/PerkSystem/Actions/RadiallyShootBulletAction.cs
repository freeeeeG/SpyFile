using System;
using System.Collections;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001CA RID: 458
	public class RadiallyShootBulletAction : Action
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x00027FAD File Offset: 0x000261AD
		public override void Init()
		{
			this.PF = ProjectileFactory.SharedInstance;
			this.myGun = PlayerController.Instance.gun;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00027FCA File Offset: 0x000261CA
		public override void Activate(GameObject target)
		{
			if (!this.limitActivationsPerFrame || this._activationCount < this.activationLimit)
			{
				PlayerController.Instance.StartCoroutine(this.RadiallyShootCR(target.transform));
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00027FF9 File Offset: 0x000261F9
		private IEnumerator RadiallyShootCR(Transform center)
		{
			this._activationCount++;
			Vector2 startDirection = Vector2.zero;
			while (startDirection == Vector2.zero)
			{
				startDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
			}
			int num;
			for (int i = 0; i < this.numOfBullets; i = num + 1)
			{
				float degrees = (float)i / (float)this.numOfBullets * 360f;
				Vector2 vector = startDirection.Rotate(degrees);
				ProjectileRecipe projectileRecipe = this.myGun.GetProjectileRecipe();
				projectileRecipe.size = this.sizeMultiplier;
				projectileRecipe.knockback *= this.knockbackMultiplier;
				projectileRecipe.projectileSpeed *= this.projSpeedMultiplier;
				Vector3 position = center.position;
				this.PF.SpawnProjectile(projectileRecipe, vector, position + vector * this.bulletSpawnOffset, this.damageMultiplier, this.unretrievableByMagicBow);
				yield return new WaitForSeconds(this.delayBetweenShots);
				num = i;
			}
			this._activationCount--;
			yield break;
		}

		// Token: 0x04000736 RID: 1846
		[SerializeField]
		private float bulletSpawnOffset;

		// Token: 0x04000737 RID: 1847
		[SerializeField]
		private float delayBetweenShots;

		// Token: 0x04000738 RID: 1848
		[SerializeField]
		private int numOfBullets = 1;

		// Token: 0x04000739 RID: 1849
		[SerializeField]
		private float damageMultiplier = 1f;

		// Token: 0x0400073A RID: 1850
		[SerializeField]
		private float sizeMultiplier = 1f;

		// Token: 0x0400073B RID: 1851
		[SerializeField]
		private float knockbackMultiplier = 1f;

		// Token: 0x0400073C RID: 1852
		[SerializeField]
		private float projSpeedMultiplier = 1f;

		// Token: 0x0400073D RID: 1853
		[SerializeField]
		private bool unretrievableByMagicBow;

		// Token: 0x0400073E RID: 1854
		[SerializeField]
		private bool limitActivationsPerFrame;

		// Token: 0x0400073F RID: 1855
		[SerializeField]
		private int activationLimit;

		// Token: 0x04000740 RID: 1856
		private ProjectileFactory PF;

		// Token: 0x04000741 RID: 1857
		private Gun myGun;

		// Token: 0x04000742 RID: 1858
		private int _activationCount;
	}
}
