using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x02000069 RID: 105
	public class ETFXPitchRandomizer : MonoBehaviour
	{
		// Token: 0x0600017E RID: 382 RVA: 0x000070E8 File Offset: 0x000052E8
		private void Start()
		{
			base.transform.GetComponent<AudioSource>().pitch *= 1f + Random.Range(-this.randomPercent / 100f, this.randomPercent / 100f);
		}

		// Token: 0x04000177 RID: 375
		public float randomPercent = 10f;
	}
}
