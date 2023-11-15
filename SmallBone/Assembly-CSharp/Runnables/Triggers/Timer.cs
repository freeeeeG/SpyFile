using System;
using System.Collections;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000369 RID: 873
	public class Timer : Trigger
	{
		// Token: 0x0600101D RID: 4125 RVA: 0x00030087 File Offset: 0x0002E287
		private void Awake()
		{
			if (this._onPreTime)
			{
				this._running = true;
				base.StartCoroutine(this.CRun());
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x000300A5 File Offset: 0x0002E2A5
		protected override bool Check()
		{
			if (this._running)
			{
				return false;
			}
			base.StartCoroutine(this.CRun());
			return true;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000300BF File Offset: 0x0002E2BF
		private IEnumerator CRun()
		{
			this._running = true;
			float seconds = this._range ? UnityEngine.Random.Range(this._timeRange.x, this._timeRange.y) : this._time;
			yield return Chronometer.global.WaitForSeconds(seconds);
			this._running = false;
			yield break;
		}

		// Token: 0x04000D36 RID: 3382
		[SerializeField]
		private bool _onPreTime;

		// Token: 0x04000D37 RID: 3383
		[SerializeField]
		private float _time;

		// Token: 0x04000D38 RID: 3384
		[SerializeField]
		private bool _range;

		// Token: 0x04000D39 RID: 3385
		[SerializeField]
		[MinMaxSlider(0f, 100f)]
		private Vector2 _timeRange;

		// Token: 0x04000D3A RID: 3386
		private bool _running;
	}
}
