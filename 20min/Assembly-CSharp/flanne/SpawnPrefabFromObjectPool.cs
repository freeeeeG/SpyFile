using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000115 RID: 277
	public class SpawnPrefabFromObjectPool : MonoBehaviour
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x000212D3 File Offset: 0x0001F4D3
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, this.amountToInitInObjPool, true);
			this.isInitialized = true;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002130A File Offset: 0x0001F50A
		public void Spawn()
		{
			if (!this.isInitialized)
			{
				return;
			}
			GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(true);
		}

		// Token: 0x0400058A RID: 1418
		[SerializeField]
		private GameObject prefab;

		// Token: 0x0400058B RID: 1419
		[SerializeField]
		private int amountToInitInObjPool;

		// Token: 0x0400058C RID: 1420
		private bool isInitialized;

		// Token: 0x0400058D RID: 1421
		private ObjectPooler OP;
	}
}
