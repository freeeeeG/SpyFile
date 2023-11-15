using System;
using Level;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C8 RID: 712
	public class OnLootMapRewardInvoker : MonoBehaviour
	{
		// Token: 0x06000E65 RID: 3685 RVA: 0x0002D41F File Offset: 0x0002B61F
		private void Start()
		{
			Map.Instance.mapReward.onLoot += this.Run;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0002D43C File Offset: 0x0002B63C
		private void Run()
		{
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x04000BF6 RID: 3062
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BF7 RID: 3063
		[SerializeField]
		private Runnable _execute;
	}
}
