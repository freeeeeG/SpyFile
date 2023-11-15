using System;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C7 RID: 711
	public sealed class OnEnableInvoker : MonoBehaviour
	{
		// Token: 0x06000E63 RID: 3683 RVA: 0x0002D405 File Offset: 0x0002B605
		private void OnEnable()
		{
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x04000BF4 RID: 3060
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BF5 RID: 3061
		[SerializeField]
		private Runnable _execute;
	}
}
