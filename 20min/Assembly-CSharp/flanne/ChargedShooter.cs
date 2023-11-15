using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x02000069 RID: 105
	public class ChargedShooter : Shooter
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00017356 File Offset: 0x00015556
		public override bool fireOnStop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001735C File Offset: 0x0001555C
		public override void OnStopShoot(ProjectileRecipe recipe, Vector2 pointDirection, int numProjectiles, float spread, float inaccuracy)
		{
			recipe.damage *= 1f + (float)this._charge * 0.5f;
			recipe.size *= 1f + (float)this._charge * 0.25f;
			if (this._charge == this.maxCharge)
			{
				recipe.piercing += 999;
			}
			else
			{
				recipe.piercing += this._charge;
			}
			base.Shoot(recipe, pointDirection, numProjectiles, spread, inaccuracy);
			this._charge = -1;
			this.chargeUpSprite.localScale = Vector3.zero;
			this.maxChargeAnimationObj.SetActive(false);
			UnityEvent onShoot = this.onShoot;
			if (onShoot == null)
			{
				return;
			}
			onShoot.Invoke();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00017420 File Offset: 0x00015620
		public override void Shoot(ProjectileRecipe recipe, Vector2 pointDirection, int numProjectiles, float spread, float inaccuracy)
		{
			if (this._charge == this.maxCharge)
			{
				return;
			}
			this._charge++;
			this.chargeUpSprite.localScale = Vector3.one * ((float)this._charge / (float)this.maxCharge);
			if (this._charge == 0)
			{
				SoundEffectSO soundEffectSO = this.chargeUpSFX;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
			}
			if (this._charge == this.maxCharge)
			{
				this.maxChargeAnimationObj.SetActive(true);
				SoundEffectSO soundEffectSO2 = this.maxChargeSFX;
				if (soundEffectSO2 == null)
				{
					return;
				}
				soundEffectSO2.Play(null);
			}
		}

		// Token: 0x0400028B RID: 651
		[SerializeField]
		private Transform chargeUpSprite;

		// Token: 0x0400028C RID: 652
		[SerializeField]
		private GameObject maxChargeAnimationObj;

		// Token: 0x0400028D RID: 653
		[SerializeField]
		private int maxCharge;

		// Token: 0x0400028E RID: 654
		[SerializeField]
		private SoundEffectSO chargeUpSFX;

		// Token: 0x0400028F RID: 655
		[SerializeField]
		private SoundEffectSO maxChargeSFX;

		// Token: 0x04000290 RID: 656
		private int _charge = -1;
	}
}
