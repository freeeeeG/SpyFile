using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200012F RID: 303
	public class SummonEgg : MonoBehaviour
	{
		// Token: 0x06000827 RID: 2087 RVA: 0x000228BF File Offset: 0x00020ABF
		private void Start()
		{
			this.summon.gameObject.SetActive(false);
			base.StartCoroutine(this.WaitToHatchCR());
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000228DF File Offset: 0x00020ADF
		private IEnumerator WaitToHatchCR()
		{
			yield return new WaitForSeconds(this.secondsToHatch);
			int num;
			for (int i = 0; i < 3; i = num + 1)
			{
				this.hatchFlasher.Flash();
				yield return new WaitForSeconds(0.2f);
				num = i;
			}
			this.hatchParticles.Play();
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			this.summon.gameObject.SetActive(true);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x040005FB RID: 1531
		[SerializeField]
		private Summon summon;

		// Token: 0x040005FC RID: 1532
		[SerializeField]
		private ParticleSystem hatchParticles;

		// Token: 0x040005FD RID: 1533
		[SerializeField]
		private FlashSprite hatchFlasher;

		// Token: 0x040005FE RID: 1534
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040005FF RID: 1535
		[SerializeField]
		private float secondsToHatch;
	}
}
