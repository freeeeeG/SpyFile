using System;
using System.Collections;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002F0 RID: 752
	public class WaitForTriggered : CRunnable
	{
		// Token: 0x06000ED2 RID: 3794 RVA: 0x0002DEC9 File Offset: 0x0002C0C9
		public override IEnumerator CRun()
		{
			while (!this._trigger.IsSatisfied())
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000C43 RID: 3139
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;
	}
}
