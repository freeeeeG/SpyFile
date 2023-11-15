using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000105 RID: 261
	public class SpawnOnDisableProjectile : Projectile
	{
		// Token: 0x06000774 RID: 1908 RVA: 0x0002066C File Offset: 0x0001E86C
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, 100, true);
			this.isInitialized = true;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000206A0 File Offset: 0x0001E8A0
		private void OnDisable()
		{
			if (!this.isSecondary && this.isInitialized)
			{
				GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
				pooledObject.transform.position = base.transform.position;
				pooledObject.SetActive(true);
			}
		}

		// Token: 0x0400054A RID: 1354
		[SerializeField]
		private GameObject prefab;

		// Token: 0x0400054B RID: 1355
		private bool isInitialized;

		// Token: 0x0400054C RID: 1356
		private ObjectPooler OP;
	}
}
