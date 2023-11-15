using System;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200035C RID: 860
	public class Inverter : Trigger
	{
		// Token: 0x0600100A RID: 4106 RVA: 0x0002FEBF File Offset: 0x0002E0BF
		protected override bool Check()
		{
			return !this._trigger.IsSatisfied();
		}

		// Token: 0x04000D1F RID: 3359
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;
	}
}
