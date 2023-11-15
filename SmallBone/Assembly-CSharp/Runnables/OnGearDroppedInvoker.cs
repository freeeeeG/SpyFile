using System;
using Characters.Gear;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002B6 RID: 694
	public sealed class OnGearDroppedInvoker : MonoBehaviour
	{
		// Token: 0x06000E26 RID: 3622 RVA: 0x0002CC4F File Offset: 0x0002AE4F
		private void Awake()
		{
			this._gear.onDropped += this.OnGearDrop;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0002CC68 File Offset: 0x0002AE68
		private void OnGearDrop()
		{
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x04000BC6 RID: 3014
		[SerializeField]
		private Gear _gear;

		// Token: 0x04000BC7 RID: 3015
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04000BC8 RID: 3016
		[SerializeField]
		private Runnable _execute;
	}
}
