using System;
using Characters;
using Runnables.Triggers;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002BE RID: 702
	public class InteractionInvoker : InteractiveObject
	{
		// Token: 0x06000E3F RID: 3647 RVA: 0x0002CE68 File Offset: 0x0002B068
		public override void InteractWith(Character character)
		{
			if (this._once && this._used)
			{
				return;
			}
			if (this._trigger.IsSatisfied())
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
				this._used = true;
				this._execute.Run();
				if (this._once)
				{
					base.Deactivate();
				}
			}
		}

		// Token: 0x04000BD8 RID: 3032
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04000BD9 RID: 3033
		[SerializeField]
		private Runnable _execute;

		// Token: 0x04000BDA RID: 3034
		[SerializeField]
		private bool _once = true;

		// Token: 0x04000BDB RID: 3035
		private bool _used;
	}
}
