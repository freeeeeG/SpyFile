using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
[Serializable]
public class DiscoverRewardEntry
{
	// Token: 0x0400007E RID: 126
	[Header("$GetQuantityInfo")]
	public eDiscoverRewardType rewardType;

	// Token: 0x0400007F RID: 127
	[Header("權重")]
	public int weight = 1;

	// Token: 0x04000080 RID: 128
	[Header("每份有幾個")]
	public int quantityPerServe = 1;

	// Token: 0x04000081 RID: 129
	[Header("最小數量")]
	public int minQuantityMultiplier = 1;

	// Token: 0x04000082 RID: 130
	[Header("最大數量")]
	public int maxQuantityMultiplier = 1;

	// Token: 0x04000083 RID: 131
	[HideInInspector]
	public int quantity;
}
