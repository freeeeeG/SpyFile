using System;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000350 RID: 848
	public class GameObjectDestroied : Trigger
	{
		// Token: 0x06000FE8 RID: 4072 RVA: 0x0002FADF File Offset: 0x0002DCDF
		protected override bool Check()
		{
			return this._target == null;
		}

		// Token: 0x04000D08 RID: 3336
		[SerializeField]
		private GameObject _target;
	}
}
