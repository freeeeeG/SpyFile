using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D4A RID: 3402
	public class ExploreOilFieldSubZone : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AC8 RID: 27336 RVA: 0x00299DD4 File Offset: 0x00297FD4
		public override bool Success()
		{
			return Game.Instance.savedInfo.discoveredOilField;
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x00299DE5 File Offset: 0x00297FE5
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x00299DE7 File Offset: 0x00297FE7
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ENTER_OIL_BIOME;
		}
	}
}
