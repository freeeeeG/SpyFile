using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D59 RID: 3417
	public class SentCraftIntoTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x06006B03 RID: 27395 RVA: 0x0029AD90 File Offset: 0x00298F90
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x06006B04 RID: 27396 RVA: 0x0029ADA6 File Offset: 0x00298FA6
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, UI.SPACEDESTINATIONS.WORMHOLE.NAME);
		}

		// Token: 0x06006B05 RID: 27397 RVA: 0x0029ADBC File Offset: 0x00298FBC
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x0029ADC8 File Offset: 0x00298FC8
		public override bool Success()
		{
			return ClusterManager.Instance.GetClusterPOIManager().HasTemporalTearConsumedCraft();
		}
	}
}
