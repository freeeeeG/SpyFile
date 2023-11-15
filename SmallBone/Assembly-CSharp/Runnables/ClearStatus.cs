using System;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200030A RID: 778
	public sealed class ClearStatus : Runnable
	{
		// Token: 0x06000F37 RID: 3895 RVA: 0x0002E8A1 File Offset: 0x0002CAA1
		public override void Run()
		{
			this._target.character.status.RemoveAllStatus();
		}

		// Token: 0x04000C8B RID: 3211
		[SerializeField]
		private Target _target;
	}
}
