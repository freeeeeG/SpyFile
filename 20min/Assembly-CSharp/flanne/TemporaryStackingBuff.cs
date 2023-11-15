using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B3 RID: 179
	public class TemporaryStackingBuff : BuffPlayerStats
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x0001B2CA File Offset: 0x000194CA
		public void Activate()
		{
			base.StartCoroutine(this.BuffCR());
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001B2D9 File Offset: 0x000194D9
		private IEnumerator BuffCR()
		{
			base.ApplyBuff();
			yield return new WaitForSeconds((float)this.duration);
			base.RemoveBuff();
			yield break;
		}

		// Token: 0x040003A7 RID: 935
		[SerializeField]
		private int duration;
	}
}
