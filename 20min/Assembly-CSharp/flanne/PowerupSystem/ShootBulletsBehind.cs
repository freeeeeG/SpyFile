using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystem
{
	// Token: 0x02000251 RID: 593
	public class ShootBulletsBehind : MonoBehaviour
	{
		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002F0F0 File Offset: 0x0002D2F0
		private void Start()
		{
			this.PF = ProjectileFactory.SharedInstance;
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.myGun = componentInParent.gun;
			this.myGun.OnShoot.AddListener(new UnityAction(this.OnShoot));
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002F142 File Offset: 0x0002D342
		private void OnDestroy()
		{
			this.myGun.OnShoot.RemoveListener(new UnityAction(this.OnShoot));
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0002F160 File Offset: 0x0002D360
		private void OnShoot()
		{
			Vector2 b = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 v = base.transform.position - b;
			float num = -1f * this.spread / 2f;
			for (int i = 0; i < this.numOfBullets; i++)
			{
				float degrees = num + (float)i / (float)this.numOfBullets * this.spread;
				Vector2 direction = v.Rotate(degrees);
				this.PF.SpawnProjectile(this.myGun.GetProjectileRecipe(), direction, base.transform.position, this.damageMultiplier, false);
			}
		}

		// Token: 0x0400092C RID: 2348
		[SerializeField]
		private int numOfBullets;

		// Token: 0x0400092D RID: 2349
		[SerializeField]
		private float spread;

		// Token: 0x0400092E RID: 2350
		[SerializeField]
		private float damageMultiplier;

		// Token: 0x0400092F RID: 2351
		private ProjectileFactory PF;

		// Token: 0x04000930 RID: 2352
		private Gun myGun;

		// Token: 0x04000931 RID: 2353
		private ShootingCursor SC;
	}
}
