using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.RuneSystem
{
	// Token: 0x02000144 RID: 324
	public class ActivateGameObjOnLastAmmoRune : Rune
	{
		// Token: 0x0600086F RID: 2159 RVA: 0x00023B6D File Offset: 0x00021D6D
		protected override void Init()
		{
			this.ammo = this.player.ammo;
			this.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00023B9C File Offset: 0x00021D9C
		private void OnDestroy()
		{
			this.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00023BBA File Offset: 0x00021DBA
		private void OnAmmoChanged(int ammoAmount)
		{
			if (ammoAmount == 0)
			{
				base.StartCoroutine(this.DelayToActivateCR());
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00023BCC File Offset: 0x00021DCC
		private IEnumerator DelayToActivateCR()
		{
			yield return new WaitForSeconds(this.delayAfterLastAmmo);
			this.harm.damageAmount = Mathf.FloorToInt(this.percentBulletDamagePerLevel * this.player.gun.damage * (float)this.level);
			this.harm.gameObject.SetActive(true);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			yield break;
		}

		// Token: 0x0400063A RID: 1594
		[Range(0f, 1f)]
		[SerializeField]
		private float percentBulletDamagePerLevel;

		// Token: 0x0400063B RID: 1595
		[SerializeField]
		private Harmful harm;

		// Token: 0x0400063C RID: 1596
		[SerializeField]
		private float delayAfterLastAmmo;

		// Token: 0x0400063D RID: 1597
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x0400063E RID: 1598
		private Ammo ammo;
	}
}
