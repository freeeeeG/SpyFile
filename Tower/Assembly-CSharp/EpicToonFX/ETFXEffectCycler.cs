using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x02000061 RID: 97
	public class ETFXEffectCycler : MonoBehaviour
	{
		// Token: 0x0600013C RID: 316 RVA: 0x000063DB File Offset: 0x000045DB
		private void Start()
		{
			base.Invoke("PlayEffect", this.startDelay);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000063EE File Offset: 0x000045EE
		public void PlayEffect()
		{
			base.StartCoroutine("EffectLoop");
			if (this.effectIndex < this.listOfEffects.Count - 1)
			{
				this.effectIndex++;
				return;
			}
			this.effectIndex = 0;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006427 File Offset: 0x00004627
		private IEnumerator EffectLoop()
		{
			GameObject instantiatedEffect = Object.Instantiate<GameObject>(this.listOfEffects[this.effectIndex], base.transform.position, base.transform.rotation * Quaternion.Euler(0f, 0f, 0f));
			if (this.disableLights && instantiatedEffect.GetComponent<Light>())
			{
				instantiatedEffect.GetComponent<Light>().enabled = false;
			}
			if (this.disableSound && instantiatedEffect.GetComponent<AudioSource>())
			{
				instantiatedEffect.GetComponent<AudioSource>().enabled = false;
			}
			yield return new WaitForSeconds(this.loopLength);
			Object.Destroy(instantiatedEffect);
			this.PlayEffect();
			yield break;
		}

		// Token: 0x04000144 RID: 324
		public List<GameObject> listOfEffects;

		// Token: 0x04000145 RID: 325
		private int effectIndex;

		// Token: 0x04000146 RID: 326
		[Header("Spawn Settings")]
		[SerializeField]
		[Space(10f)]
		public float loopLength = 1f;

		// Token: 0x04000147 RID: 327
		public float startDelay = 1f;

		// Token: 0x04000148 RID: 328
		public bool disableLights = true;

		// Token: 0x04000149 RID: 329
		public bool disableSound = true;
	}
}
