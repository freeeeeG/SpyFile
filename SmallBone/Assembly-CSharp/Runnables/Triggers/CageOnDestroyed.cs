using System;
using Level;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200034C RID: 844
	public class CageOnDestroyed : Trigger
	{
		// Token: 0x06000FDE RID: 4062 RVA: 0x0002FA6E File Offset: 0x0002DC6E
		private void Start()
		{
			this._cage.onDestroyed += delegate()
			{
				this._destroyed = true;
			};
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0002FA87 File Offset: 0x0002DC87
		protected override bool Check()
		{
			return this._destroyed;
		}

		// Token: 0x04000D03 RID: 3331
		[SerializeField]
		private Cage _cage;

		// Token: 0x04000D04 RID: 3332
		private bool _destroyed;
	}
}
