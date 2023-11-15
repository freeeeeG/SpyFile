using System;

// Token: 0x02000062 RID: 98
[Serializable]
public class StageRewardData
{
	// Token: 0x06000267 RID: 615 RVA: 0x0000A40E File Offset: 0x0000860E
	public StageRewardData(int exp = 0, int gem = 0, eStageRewardType rewardType = eStageRewardType.NONE, eItemType itemType = eItemType.NONE)
	{
		this.Exp = exp;
		this.RewardType = rewardType;
		this.Gem = gem;
		this.ItemType = itemType;
	}

	// Token: 0x040001B9 RID: 441
	public eStageRewardType RewardType;

	// Token: 0x040001BA RID: 442
	public eItemType ItemType;

	// Token: 0x040001BB RID: 443
	public int Exp;

	// Token: 0x040001BC RID: 444
	public int Gem;

	// Token: 0x040001BD RID: 445
	public bool isDisplayAsUnknown;
}
