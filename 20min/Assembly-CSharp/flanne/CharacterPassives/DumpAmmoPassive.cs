using System;
using System.Collections;
using UnityEngine;

namespace flanne.CharacterPassives
{
	// Token: 0x0200025C RID: 604
	public class DumpAmmoPassive : SkillPassive
	{
		// Token: 0x06000D19 RID: 3353 RVA: 0x0002FDA8 File Offset: 0x0002DFA8
		protected override void Init()
		{
			this.player = base.transform.root.GetComponent<PlayerController>();
			this.ammo = this.player.ammo;
			this.myGun = this.player.gun;
			this._isSpraying = false;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0002FDF4 File Offset: 0x0002DFF4
		protected override void PerformSkill()
		{
			if (!this.ammo.outOfAmmo && !this._isSpraying && this.player.playerHealth.hp != 0)
			{
				base.StartCoroutine(this.SprayCR(this.ammo.amount));
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0002FE40 File Offset: 0x0002E040
		private IEnumerator SprayCR(int amountShots)
		{
			this._isSpraying = true;
			this.player.disableAction.Flip();
			this.myGun.SetVisible(false);
			this.player.disableAnimation.Flip();
			this.player.playerAnimator.ResetTrigger("Idle");
			this.player.playerAnimator.ResetTrigger("Run");
			this.player.playerAnimator.ResetTrigger("Walk");
			this.player.playerAnimator.SetTrigger("Special");
			while (this.ammo.amount > 0)
			{
				this.ShootRandom();
				yield return new WaitForSeconds(this.myGun.shotCooldown * this.shotCDMultiplier);
			}
			this.player.disableAnimation.UnFlip();
			this.player.playerAnimator.ResetTrigger("Special");
			this.player.playerAnimator.SetTrigger("Idle");
			this.myGun.SetVisible(true);
			this._isSpraying = false;
			this.player.disableAction.UnFlip();
			yield break;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0002FE50 File Offset: 0x0002E050
		private void ShootRandom()
		{
			Vector2 zero = Vector2.zero;
			while (zero == Vector2.zero)
			{
				zero = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			}
			int count = this.myGun.shooters.Count;
			for (int i = 0; i < count; i++)
			{
				this.shooter.Shoot(this.myGun.GetProjectileRecipe(), zero.Rotate((float)(i * 10)), this.myGun.numOfProjectiles, this.myGun.spread, 0f);
				this.myGun.OnShoot.Invoke();
			}
			SoundEffectSO gunshotSFX = this.myGun.gunData.gunshotSFX;
			if (gunshotSFX == null)
			{
				return;
			}
			gunshotSFX.Play(null);
		}

		// Token: 0x0400097B RID: 2427
		public float shotCDMultiplier;

		// Token: 0x0400097C RID: 2428
		[SerializeField]
		private Shooter shooter;

		// Token: 0x0400097D RID: 2429
		private PlayerController player;

		// Token: 0x0400097E RID: 2430
		private Ammo ammo;

		// Token: 0x0400097F RID: 2431
		private Gun myGun;

		// Token: 0x04000980 RID: 2432
		private bool _isSpraying;
	}
}
