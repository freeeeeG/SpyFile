using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
[Serializable]
public class SpawnSession
{
	// Token: 0x040001DD RID: 477
	public GameObject monsterPrefab;

	// Token: 0x040001DE RID: 478
	public int HP = 1;

	// Token: 0x040001DF RID: 479
	public int maximum;

	// Token: 0x040001E0 RID: 480
	public int numPerSpawn;

	// Token: 0x040001E1 RID: 481
	public float spawnCooldown;

	// Token: 0x040001E2 RID: 482
	public float startTime;

	// Token: 0x040001E3 RID: 483
	public float duration;

	// Token: 0x040001E4 RID: 484
	public bool isElite;

	// Token: 0x040001E5 RID: 485
	[NonSerialized]
	public float timer;
}
