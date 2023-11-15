using System;
using System.Collections;
using UnityEngine;

namespace ARPGFX
{
	// Token: 0x0200006C RID: 108
	public class ARPGFXLoopScript : MonoBehaviour
	{
		// Token: 0x06000186 RID: 390 RVA: 0x000072A3 File Offset: 0x000054A3
		private void Start()
		{
			this.PlayEffect();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000072AB File Offset: 0x000054AB
		public void PlayEffect()
		{
			base.StartCoroutine("EffectLoop");
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000072B9 File Offset: 0x000054B9
		private IEnumerator EffectLoop()
		{
			GameObject effectPlayer = Object.Instantiate<GameObject>(this.chosenEffect);
			effectPlayer.transform.position = base.transform.position;
			yield return new WaitForSeconds(this.loopTimeLimit);
			Object.Destroy(effectPlayer);
			this.PlayEffect();
			yield break;
		}

		// Token: 0x0400017F RID: 383
		public GameObject chosenEffect;

		// Token: 0x04000180 RID: 384
		public float loopTimeLimit = 2f;
	}
}
