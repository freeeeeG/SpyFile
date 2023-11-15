using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000113 RID: 275
	public class SpawnFromObjectPool : MonoBehaviour
	{
		// Token: 0x060007AF RID: 1967 RVA: 0x000211A8 File Offset: 0x0001F3A8
		public void Spawn(string objectPoolTag)
		{
			GameObject pooledObject = ObjectPooler.SharedInstance.GetPooledObject(objectPoolTag);
			pooledObject.transform.position = base.transform.position;
			pooledObject.SetActive(true);
		}
	}
}
