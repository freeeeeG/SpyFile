using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.RuneSystem
{
	// Token: 0x02000152 RID: 338
	public class LastStandRune : Rune
	{
		// Token: 0x060008BC RID: 2236 RVA: 0x00024C00 File Offset: 0x00022E00
		private void OnHealthChange(int hp)
		{
			if (hp == 1 && !this.active)
			{
				base.StartCoroutine(this.InvulnCR());
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00024C1B File Offset: 0x00022E1B
		protected override void Init()
		{
			this.player.playerHealth.onHealthChangedTo.AddListener(new UnityAction<int>(this.OnHealthChange));
			this.active = false;
			this.playerFlasher = this.player.GetComponentInChildren<PlayerFlasher>();
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00024C56 File Offset: 0x00022E56
		private void OnDestroy()
		{
			this.player.playerHealth.onHealthChangedTo.RemoveListener(new UnityAction<int>(this.OnHealthChange));
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00024C79 File Offset: 0x00022E79
		private IEnumerator InvulnCR()
		{
			this.active = true;
			this.player.playerHealth.isInvincible.Flip();
			this.playerFlasher.Flash();
			this.knockbackObj.SetActive(true);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			yield return new WaitForSeconds(this.invulnDurationPerLevel * (float)this.level);
			this.active = false;
			this.player.playerHealth.isInvincible.UnFlip();
			this.playerFlasher.StopFlash();
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x04000675 RID: 1653
		[SerializeField]
		private float invulnDurationPerLevel;

		// Token: 0x04000676 RID: 1654
		[SerializeField]
		private GameObject knockbackObj;

		// Token: 0x04000677 RID: 1655
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000678 RID: 1656
		private PlayerFlasher playerFlasher;

		// Token: 0x04000679 RID: 1657
		private bool active;
	}
}
