using System;
using System.Collections.Generic;

// Token: 0x02000065 RID: 101
public class DiscoverRewardData
{
	// Token: 0x06000277 RID: 631 RVA: 0x0000A75A File Offset: 0x0000895A
	public DiscoverRewardData()
	{
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000A762 File Offset: 0x00008962
	public DiscoverRewardData(eDiscoverRewardType rewardType, List<eItemType> itemTypes, int value)
	{
		this.DiscoverRewardType = rewardType;
		this.List_RewardContentType = itemTypes;
		this.value = value;
	}

	// Token: 0x040001CC RID: 460
	public eDiscoverRewardType DiscoverRewardType;

	// Token: 0x040001CD RID: 461
	public List<eItemType> List_RewardContentType;

	// Token: 0x040001CE RID: 462
	public int value;
}
