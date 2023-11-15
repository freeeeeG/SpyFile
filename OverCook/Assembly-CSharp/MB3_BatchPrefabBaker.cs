using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class MB3_BatchPrefabBaker : MonoBehaviour
{
	// Token: 0x0400017C RID: 380
	public MB3_BatchPrefabBaker.MB3_PrefabBakerRow[] prefabRows;

	// Token: 0x0400017D RID: 381
	public string outputPrefabFolder;

	// Token: 0x02000054 RID: 84
	[Serializable]
	public class MB3_PrefabBakerRow
	{
		// Token: 0x0400017E RID: 382
		public GameObject sourcePrefab;

		// Token: 0x0400017F RID: 383
		public GameObject resultPrefab;
	}
}
