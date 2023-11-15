using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200005F RID: 95
	public class Spawner : MonoBehaviour
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x000169DC File Offset: 0x00014BDC
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.spawnedObject.name, this.spawnedObject, 100, true);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00016A08 File Offset: 0x00014C08
		public void Spawn()
		{
			foreach (Transform transform in this.spawnPoints)
			{
				GameObject pooledObject = this.OP.GetPooledObject(this.spawnedObject.name);
				pooledObject.transform.position = transform.position;
				pooledObject.SetActive(true);
				pooledObject.GetComponent<MoveComponent2D>().vector = (pooledObject.transform.position - base.transform.position).normalized * this.initialSpeed;
			}
		}

		// Token: 0x04000251 RID: 593
		[SerializeField]
		private float initialSpeed;

		// Token: 0x04000252 RID: 594
		[SerializeField]
		private GameObject spawnedObject;

		// Token: 0x04000253 RID: 595
		[SerializeField]
		private Transform[] spawnPoints;

		// Token: 0x04000254 RID: 596
		private ObjectPooler OP;
	}
}
