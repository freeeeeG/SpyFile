using System;
using System.Collections;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002CA RID: 714
	public class UpdateInvoker : MonoBehaviour
	{
		// Token: 0x06000E6A RID: 3690 RVA: 0x0002D470 File Offset: 0x0002B670
		private void Start()
		{
			if (this._once)
			{
				base.StartCoroutine(this.CRun());
				return;
			}
			base.StartCoroutine(this.CUpdate());
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0002D495 File Offset: 0x0002B695
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				yield return null;
				if (this._trigger.IsSatisfied())
				{
					this._execute.Run();
				}
			}
			yield break;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0002D4A4 File Offset: 0x0002B6A4
		private IEnumerator CRun()
		{
			while (!this._trigger.IsSatisfied())
			{
				yield return null;
			}
			this._execute.Run();
			yield break;
		}

		// Token: 0x04000BFA RID: 3066
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BFB RID: 3067
		[SerializeField]
		private Runnable _execute;

		// Token: 0x04000BFC RID: 3068
		[SerializeField]
		private bool _once;
	}
}
