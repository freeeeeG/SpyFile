using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E4B RID: 3659
	public class TriggerActionStart : Operation
	{
		// Token: 0x060048C1 RID: 18625 RVA: 0x000D4373 File Offset: 0x000D2573
		public override void Run()
		{
			this._action.TriggerStartManually();
		}

		// Token: 0x040037CE RID: 14286
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
