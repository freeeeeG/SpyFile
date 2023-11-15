using System;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C9 RID: 713
	public class StartInvoker : MonoBehaviour
	{
		// Token: 0x06000E68 RID: 3688 RVA: 0x0002D456 File Offset: 0x0002B656
		private void Start()
		{
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x04000BF8 RID: 3064
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BF9 RID: 3065
		[SerializeField]
		private Runnable _execute;
	}
}
