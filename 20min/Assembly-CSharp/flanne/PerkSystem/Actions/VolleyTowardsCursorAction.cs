using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001DA RID: 474
	public class VolleyTowardsCursorAction : Action
	{
		// Token: 0x06000A6D RID: 2669 RVA: 0x000289E6 File Offset: 0x00026BE6
		public override void Init()
		{
			this.PF = ProjectileFactory.SharedInstance;
			this.myGun = PlayerController.Instance.gun;
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00028A10 File Offset: 0x00026C10
		public override void Activate(GameObject target)
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 b = this.myGun.transform.position;
			Vector2 v = (a - b).Rotate(-1f * (this.spread / 2f));
			for (int i = 0; i < this.numOfBullets; i++)
			{
				float degrees = (float)i / (float)this.numOfBullets * this.spread;
				Vector2 vector = v.Rotate(degrees);
				ProjectileRecipe projectileRecipe = this.myGun.GetProjectileRecipe();
				projectileRecipe.size = this.sizeMultiplier;
				projectileRecipe.knockback *= this.knockbackMultiplier;
				projectileRecipe.projectileSpeed *= this.projSpeedMultiplier;
				Vector3 position = this.myGun.transform.position;
				this.PF.SpawnProjectile(projectileRecipe, vector, position + vector * this.bulletSpawnOffset, this.damageMultiplier, this.unretrievableByMagicBow);
			}
		}

		// Token: 0x0400076D RID: 1901
		[SerializeField]
		private float bulletSpawnOffset;

		// Token: 0x0400076E RID: 1902
		[SerializeField]
		private float spread;

		// Token: 0x0400076F RID: 1903
		[SerializeField]
		private int numOfBullets = 1;

		// Token: 0x04000770 RID: 1904
		[SerializeField]
		private float damageMultiplier = 1f;

		// Token: 0x04000771 RID: 1905
		[SerializeField]
		private float sizeMultiplier = 1f;

		// Token: 0x04000772 RID: 1906
		[SerializeField]
		private float knockbackMultiplier = 1f;

		// Token: 0x04000773 RID: 1907
		[SerializeField]
		private float projSpeedMultiplier = 1f;

		// Token: 0x04000774 RID: 1908
		[SerializeField]
		private bool unretrievableByMagicBow;

		// Token: 0x04000775 RID: 1909
		private ProjectileFactory PF;

		// Token: 0x04000776 RID: 1910
		private Gun myGun;

		// Token: 0x04000777 RID: 1911
		private ShootingCursor SC;

		// Token: 0x04000778 RID: 1912
		private int _activationCount;
	}
}
