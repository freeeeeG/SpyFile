using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E0 RID: 224
	public class DropPrefabOverTime : MonoBehaviour
	{
		// Token: 0x060006B0 RID: 1712 RVA: 0x0001E0D8 File Offset: 0x0001C2D8
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, 5, true);
			base.InvokeRepeating("DropHP", 0f, this.timeToDrop);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001B336 File Offset: 0x00019536
		private void OnDestroy()
		{
			base.CancelInvoke();
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001E124 File Offset: 0x0001C324
		private void DropHP()
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(true);
		}

		// Token: 0x0400047D RID: 1149
		[SerializeField]
		private float timeToDrop;

		// Token: 0x0400047E RID: 1150
		[SerializeField]
		private GameObject prefab;

		// Token: 0x0400047F RID: 1151
		private ObjectPooler OP;
	}
}
