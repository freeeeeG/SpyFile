using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000094 RID: 148
	[Serializable]
	public class BossSpawn
	{
		// Token: 0x04000345 RID: 837
		public int maxHP;

		// Token: 0x04000346 RID: 838
		public GameObject bossPrefab;

		// Token: 0x04000347 RID: 839
		public float timeToSpawn;

		// Token: 0x04000348 RID: 840
		public Vector3 spawnPosition;

		// Token: 0x04000349 RID: 841
		public bool dontSpawnArena;

		// Token: 0x0400034A RID: 842
		public bool killAllOnSpawn;
	}
}
