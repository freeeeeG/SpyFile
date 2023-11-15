using System;
using System.Collections;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000324 RID: 804
	public class InfinityLoop : Runnable
	{
		// Token: 0x06000F76 RID: 3958 RVA: 0x0002F0B4 File Offset: 0x0002D2B4
		public override void Run()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002F0C3 File Offset: 0x0002D2C3
		private IEnumerator CRun()
		{
			for (;;)
			{
				this._runnable.Run();
				yield return Chronometer.global.WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x04000CB9 RID: 3257
		[SerializeField]
		private float _interval;

		// Token: 0x04000CBA RID: 3258
		[SerializeField]
		[Runnable.SubcomponentAttribute]
		private Runnable _runnable;
	}
}
