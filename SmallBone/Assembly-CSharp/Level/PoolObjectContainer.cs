using System;
using UnityEngine;

namespace Level
{
	// Token: 0x0200047E RID: 1150
	public sealed class PoolObjectContainer : MonoBehaviour
	{
		// Token: 0x060015ED RID: 5613 RVA: 0x00044C82 File Offset: 0x00042E82
		public void Push(PoolObject poolObject)
		{
			poolObject.transform.SetParent(base.transform);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00044C98 File Offset: 0x00042E98
		public void DespawnAll()
		{
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				PoolObject component = base.transform.GetChild(i).GetComponent<PoolObject>();
				if (component != null)
				{
					component.Despawn();
				}
			}
		}
	}
}
