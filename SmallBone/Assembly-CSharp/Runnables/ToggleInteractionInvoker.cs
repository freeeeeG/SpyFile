using System;
using Characters;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002BB RID: 699
	public class ToggleInteractionInvoker : InteractiveObject
	{
		// Token: 0x06000E34 RID: 3636 RVA: 0x0002CD72 File Offset: 0x0002AF72
		public override void InteractWith(Character character)
		{
			this._on = !this._on;
			if (this._on)
			{
				this._onExecute.Run();
				return;
			}
			this._offExecute.Run();
		}

		// Token: 0x04000BD0 RID: 3024
		[SerializeField]
		private bool _on;

		// Token: 0x04000BD1 RID: 3025
		[SerializeField]
		private Runnable _onExecute;

		// Token: 0x04000BD2 RID: 3026
		[SerializeField]
		private Runnable _offExecute;
	}
}
