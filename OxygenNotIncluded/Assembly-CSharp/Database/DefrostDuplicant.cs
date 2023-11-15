using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D5C RID: 3420
	public class DefrostDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x06006B0E RID: 27406 RVA: 0x0029AECC File Offset: 0x002990CC
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.DEFROST_DUPLICANT;
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x0029AED8 File Offset: 0x002990D8
		public override bool Success()
		{
			return SaveGame.Instance.GetComponent<ColonyAchievementTracker>().defrostedDuplicant;
		}
	}
}
