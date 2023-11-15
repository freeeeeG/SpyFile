using System;
using System.Collections;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002BC RID: 700
	public class ToggleInvoker : MonoBehaviour
	{
		// Token: 0x06000E36 RID: 3638 RVA: 0x0002CDA2 File Offset: 0x0002AFA2
		private void Start()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0002CDB1 File Offset: 0x0002AFB1
		private IEnumerator CRun()
		{
			for (;;)
			{
				if (this._trigger.IsSatisfied())
				{
					this._execute.Run();
					while (this._trigger.IsSatisfied())
					{
						yield return null;
					}
				}
				else
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x04000BD3 RID: 3027
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BD4 RID: 3028
		[SerializeField]
		private Runnable _execute;
	}
}
