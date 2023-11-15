using System;
using Runnables;
using Runnables.Triggers;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear
{
	// Token: 0x0200081E RID: 2078
	public sealed class OnDiscardInvoker : MonoBehaviour
	{
		// Token: 0x06002AD8 RID: 10968 RVA: 0x00083C73 File Offset: 0x00081E73
		private void Start()
		{
			this._gear.onDiscard += this.OnDiscard;
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.OnMapLoaded;
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x00083CA7 File Offset: 0x00081EA7
		private void OnMapLoaded()
		{
			if (this._gear == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x00083CC2 File Offset: 0x00081EC2
		private void OnDiscard(Gear gear)
		{
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x04002477 RID: 9335
		[SerializeField]
		private Gear _gear;

		// Token: 0x04002478 RID: 9336
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04002479 RID: 9337
		[Runnable.SubcomponentAttribute]
		[SerializeField]
		private Runnable _execute;
	}
}
