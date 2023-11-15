using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D52 RID: 3410
	public class CureDisease : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AE8 RID: 27368 RVA: 0x0029A6D0 File Offset: 0x002988D0
		public override bool Success()
		{
			return Game.Instance.savedInfo.curedDisease;
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x0029A6E1 File Offset: 0x002988E1
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x0029A6E3 File Offset: 0x002988E3
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CURED_DISEASE;
		}
	}
}
