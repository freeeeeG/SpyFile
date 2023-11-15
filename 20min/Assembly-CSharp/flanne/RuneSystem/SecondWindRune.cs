using System;
using System.Collections;
using flanne.Core;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.RuneSystem
{
	// Token: 0x02000157 RID: 343
	public class SecondWindRune : Rune
	{
		// Token: 0x060008D1 RID: 2257 RVA: 0x00024DD3 File Offset: 0x00022FD3
		private void OnHealthChange(int hp)
		{
			if (hp == 1)
			{
				base.StartCoroutine(this.HealAnimationCR());
				this.player.playerHealth.onHealthChangedTo.RemoveListener(new UnityAction<int>(this.OnHealthChange));
			}
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00024E07 File Offset: 0x00023007
		protected override void Init()
		{
			this.player.playerHealth.onHealthChangedTo.AddListener(new UnityAction<int>(this.OnHealthChange));
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00024E2A File Offset: 0x0002302A
		private IEnumerator HealAnimationCR()
		{
			this.player.playerHealth.isInvincible.Flip();
			PauseController.SharedInstance.Pause();
			yield return new WaitForSecondsRealtime(0.3f);
			this.knockbackObj.SetActive(true);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			this.particleFX.Play();
			while (this.particleFX.isPlaying)
			{
				yield return null;
			}
			this.player.playerHealth.Heal(this.amountHealed);
			PauseController.SharedInstance.UnPause();
			yield return new WaitForSeconds(0.1f);
			this.player.playerHealth.isInvincible.UnFlip();
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x0400068A RID: 1674
		[SerializeField]
		private int amountHealed;

		// Token: 0x0400068B RID: 1675
		[SerializeField]
		private GameObject knockbackObj;

		// Token: 0x0400068C RID: 1676
		[SerializeField]
		private ParticleSystem particleFX;

		// Token: 0x0400068D RID: 1677
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
