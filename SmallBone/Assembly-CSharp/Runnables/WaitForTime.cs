using System;
using System.Collections;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000300 RID: 768
	public class WaitForTime : CRunnable
	{
		// Token: 0x06000F12 RID: 3858 RVA: 0x0002E4C7 File Offset: 0x0002C6C7
		public override IEnumerator CRun()
		{
			yield return Chronometer.global.WaitForSeconds(this._time);
			yield break;
		}

		// Token: 0x04000C6C RID: 3180
		[SerializeField]
		private float _time;
	}
}
