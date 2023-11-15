using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000116 RID: 278
	public class SpawnPrefabOnStart : MonoBehaviour
	{
		// Token: 0x060007B9 RID: 1977 RVA: 0x00021348 File Offset: 0x0001F548
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, this.amountToInitInObjPool, true);
			for (int i = 0; i < this.amountToSpawn; i++)
			{
				this.Spawn();
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0002139A File Offset: 0x0001F59A
		public void Spawn()
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(true);
		}

		// Token: 0x0400058E RID: 1422
		[SerializeField]
		private GameObject prefab;

		// Token: 0x0400058F RID: 1423
		[SerializeField]
		private int amountToInitInObjPool;

		// Token: 0x04000590 RID: 1424
		[SerializeField]
		private int amountToSpawn;

		// Token: 0x04000591 RID: 1425
		private ObjectPooler OP;
	}
}
