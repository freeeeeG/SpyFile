using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x020002C0 RID: 704
	public class ETFXPitchRandomizer : MonoBehaviour
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x000311A0 File Offset: 0x0002F3A0
		private void Start()
		{
			base.transform.GetComponent<AudioSource>().pitch *= 1f + Random.Range(-this.randomPercent / 100f, this.randomPercent / 100f);
		}

		// Token: 0x04000985 RID: 2437
		public float randomPercent = 10f;
	}
}
