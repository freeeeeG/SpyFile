using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public enum eDiscoverRewardType
{
	// Token: 0x040001DA RID: 474
	[InspectorName("!!未設定!!")]
	NONE,
	// Token: 0x040001DB RID: 475
	[InspectorName("ＨＰ")]
	HP,
	// Token: 0x040001DC RID: 476
	[InspectorName("金幣")]
	COIN,
	// Token: 0x040001DD RID: 477
	[InspectorName("砲台卡片")]
	TOWER_CARD,
	// Token: 0x040001DE RID: 478
	[InspectorName("底座卡片")]
	PANEL_CARD,
	// Token: 0x040001DF RID: 479
	[InspectorName("隨機 底座卡片")]
	RANDOM_PANEL_CARD,
	// Token: 0x040001E0 RID: 480
	[InspectorName("Buff卡片")]
	BUFF_CARD,
	// Token: 0x040001E1 RID: 481
	[InspectorName("隨機 Buff卡片")]
	RANDOM_BUFF_CARD
}
