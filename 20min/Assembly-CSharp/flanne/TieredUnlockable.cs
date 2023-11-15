using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000135 RID: 309
	public abstract class TieredUnlockable : MonoBehaviour
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000839 RID: 2105
		// (set) Token: 0x0600083A RID: 2106
		public abstract int level { get; set; }
	}
}
