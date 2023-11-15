using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D62 RID: 3426
	public class HarvestAHiveWithoutBeingStung : ColonyAchievementRequirement
	{
		// Token: 0x06006B20 RID: 27424 RVA: 0x0029B189 File Offset: 0x00299389
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HARVEST_HIVE;
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x0029B195 File Offset: 0x00299395
		public override bool Success()
		{
			return SaveGame.Instance.GetComponent<ColonyAchievementTracker>().harvestAHiveWithoutGettingStung;
		}
	}
}
