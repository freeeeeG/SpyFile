using System;
using Level.Traps;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200030F RID: 783
	public class ControlTrap : Runnable
	{
		// Token: 0x06000F41 RID: 3905 RVA: 0x0002EAA8 File Offset: 0x0002CCA8
		public override void Run()
		{
			switch (this._type)
			{
			case ControlTrap.Type.Activate:
				this._trap.Activate();
				return;
			case ControlTrap.Type.Deactivate:
				this._trap.Deactivate();
				return;
			case ControlTrap.Type.Toggle:
				if (this._trap.activated)
				{
					this._trap.Deactivate();
					return;
				}
				this._trap.Activate();
				return;
			default:
				return;
			}
		}

		// Token: 0x04000C98 RID: 3224
		[SerializeField]
		private ControlableTrap _trap;

		// Token: 0x04000C99 RID: 3225
		[SerializeField]
		private ControlTrap.Type _type;

		// Token: 0x02000310 RID: 784
		private enum Type
		{
			// Token: 0x04000C9B RID: 3227
			Activate,
			// Token: 0x04000C9C RID: 3228
			Deactivate,
			// Token: 0x04000C9D RID: 3229
			Toggle
		}
	}
}
