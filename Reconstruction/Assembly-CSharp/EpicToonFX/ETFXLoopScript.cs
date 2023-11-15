using System;
using System.Collections;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x020002BC RID: 700
	public class ETFXLoopScript : MonoBehaviour
	{
		// Token: 0x06001129 RID: 4393 RVA: 0x00030CD5 File Offset: 0x0002EED5
		private void Start()
		{
			this.PlayEffect();
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00030CDD File Offset: 0x0002EEDD
		public void PlayEffect()
		{
			base.StartCoroutine("EffectLoop");
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00030CEB File Offset: 0x0002EEEB
		private IEnumerator EffectLoop()
		{
			GameObject effectPlayer = Object.Instantiate<GameObject>(this.chosenEffect, base.transform.position, base.transform.rotation);
			if (this.spawnWithoutLight = effectPlayer.GetComponent<Light>())
			{
				effectPlayer.GetComponent<Light>().enabled = false;
			}
			if (this.spawnWithoutSound = effectPlayer.GetComponent<AudioSource>())
			{
				effectPlayer.GetComponent<AudioSource>().enabled = false;
			}
			yield return new WaitForSeconds(this.loopTimeLimit);
			Object.Destroy(effectPlayer);
			this.PlayEffect();
			yield break;
		}

		// Token: 0x0400096C RID: 2412
		public GameObject chosenEffect;

		// Token: 0x0400096D RID: 2413
		public float loopTimeLimit = 2f;

		// Token: 0x0400096E RID: 2414
		[Header("Spawn without")]
		public bool spawnWithoutLight = true;

		// Token: 0x0400096F RID: 2415
		public bool spawnWithoutSound = true;
	}
}
