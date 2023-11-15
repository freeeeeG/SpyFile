using System;
using System.Collections;
using UnityEngine;

namespace PixelArsenal
{
	// Token: 0x020002B2 RID: 690
	public class PixelArsenalLoopScript : MonoBehaviour
	{
		// Token: 0x060010FF RID: 4351 RVA: 0x0003001E File Offset: 0x0002E21E
		private void Start()
		{
			this.PlayLoopingPEffect();
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00030026 File Offset: 0x0002E226
		public void PlayLoopingPEffect()
		{
			base.StartCoroutine("EffectLoop");
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00030034 File Offset: 0x0002E234
		private IEnumerator EffectLoop()
		{
			GameObject effectPlayer = Object.Instantiate<GameObject>(this.chosenEffect, base.transform.position, base.transform.rotation);
			yield return new WaitForSeconds(this.loopTimeLimit);
			Object.Destroy(effectPlayer);
			this.PlayLoopingPEffect();
			yield break;
		}

		// Token: 0x0400093E RID: 2366
		public GameObject chosenEffect;

		// Token: 0x0400093F RID: 2367
		public float loopTimeLimit = 2f;
	}
}
