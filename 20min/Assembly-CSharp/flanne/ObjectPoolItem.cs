using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000075 RID: 117
	[Serializable]
	public class ObjectPoolItem
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x0001883B File Offset: 0x00016A3B
		public ObjectPoolItem(string t, GameObject obj, int amt, bool exp = true)
		{
			this.tag = t;
			this.objectToPool = obj;
			this.amountToPool = Mathf.Max(amt, 2);
			this.shouldExpand = exp;
		}

		// Token: 0x040002D1 RID: 721
		public string tag;

		// Token: 0x040002D2 RID: 722
		public GameObject objectToPool;

		// Token: 0x040002D3 RID: 723
		public int amountToPool;

		// Token: 0x040002D4 RID: 724
		public bool shouldExpand = true;
	}
}
