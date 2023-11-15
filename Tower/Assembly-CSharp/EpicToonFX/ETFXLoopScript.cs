using System;
using System.Collections;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x02000063 RID: 99
	public class ETFXLoopScript : MonoBehaviour
	{
		// Token: 0x06000146 RID: 326 RVA: 0x0000662D File Offset: 0x0000482D
		private void Start()
		{
			this.PlayEffect();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006635 File Offset: 0x00004835
		public void PlayEffect()
		{
			base.StartCoroutine("EffectLoop");
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006643 File Offset: 0x00004843
		private IEnumerator EffectLoop()
		{
			GameObject effectPlayer = Object.Instantiate<GameObject>(this.chosenEffect, base.transform.position, base.transform.rotation);
			if (this.disableLights && effectPlayer.GetComponent<Light>())
			{
				effectPlayer.GetComponent<Light>().enabled = false;
			}
			if (this.disableSound && effectPlayer.GetComponent<AudioSource>())
			{
				effectPlayer.GetComponent<AudioSource>().enabled = false;
			}
			yield return new WaitForSeconds(this.loopTimeLimit);
			Object.Destroy(effectPlayer);
			this.PlayEffect();
			yield break;
		}

		// Token: 0x04000150 RID: 336
		public GameObject chosenEffect;

		// Token: 0x04000151 RID: 337
		public float loopTimeLimit = 2f;

		// Token: 0x04000152 RID: 338
		[Header("Spawn without")]
		public bool disableLights = true;

		// Token: 0x04000153 RID: 339
		public bool disableSound = true;
	}
}
