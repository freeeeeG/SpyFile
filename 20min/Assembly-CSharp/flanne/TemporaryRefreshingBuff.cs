using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B2 RID: 178
	public class TemporaryRefreshingBuff : BuffPlayerStats
	{
		// Token: 0x060005C0 RID: 1472 RVA: 0x0001B28C File Offset: 0x0001948C
		public void Activate()
		{
			if (this.buffCoroutine == null)
			{
				this.buffCoroutine = this.BuffCR();
				base.StartCoroutine(this.buffCoroutine);
				return;
			}
			this._timer = 0f;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001B2BB File Offset: 0x000194BB
		private IEnumerator BuffCR()
		{
			base.ApplyBuff();
			while (this._timer < (float)this.duration)
			{
				this._timer += Time.deltaTime;
				yield return null;
			}
			base.RemoveBuff();
			this._timer = 0f;
			this.buffCoroutine = null;
			yield break;
		}

		// Token: 0x040003A4 RID: 932
		[SerializeField]
		private int duration;

		// Token: 0x040003A5 RID: 933
		private IEnumerator buffCoroutine;

		// Token: 0x040003A6 RID: 934
		private float _timer;
	}
}
