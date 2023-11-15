using System;
using System.Collections;
using UnityEngine;

namespace Runnables.Customs
{
	// Token: 0x0200037D RID: 893
	public class RepeatInvoker : MonoBehaviour
	{
		// Token: 0x06001058 RID: 4184 RVA: 0x0003066B File Offset: 0x0002E86B
		private void OnEnable()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0003067A File Offset: 0x0002E87A
		private IEnumerator CRun()
		{
			for (;;)
			{
				yield return Chronometer.global.WaitForSeconds(UnityEngine.Random.Range(this._interval.x, this._interval.y));
				this._execute.Run();
			}
			yield break;
		}

		// Token: 0x04000D5F RID: 3423
		[MinMaxSlider(0f, 60f)]
		[SerializeField]
		private Vector2 _interval;

		// Token: 0x04000D60 RID: 3424
		[SerializeField]
		private Runnable _execute;
	}
}
