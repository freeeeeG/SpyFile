using System;
using UnityEngine;

namespace flanne.PerkSystem
{
	// Token: 0x02000183 RID: 387
	public abstract class Action
	{
		// Token: 0x06000973 RID: 2419
		public abstract void Activate(GameObject target);

		// Token: 0x06000974 RID: 2420 RVA: 0x00002F51 File Offset: 0x00001151
		public virtual void Init()
		{
		}
	}
}
