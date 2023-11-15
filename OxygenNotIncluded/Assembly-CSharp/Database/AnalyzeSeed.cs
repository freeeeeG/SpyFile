using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D5E RID: 3422
	public class AnalyzeSeed : ColonyAchievementRequirement
	{
		// Token: 0x06006B14 RID: 27412 RVA: 0x0029AF84 File Offset: 0x00299184
		public AnalyzeSeed(string seedname)
		{
			this.seedName = seedname;
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x0029AF93 File Offset: 0x00299193
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ANALYZE_SEED, this.seedName.ProperName());
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x0029AFB4 File Offset: 0x002991B4
		public override bool Success()
		{
			return SaveGame.Instance.GetComponent<ColonyAchievementTracker>().analyzedSeeds.Contains(this.seedName);
		}

		// Token: 0x04004DC6 RID: 19910
		private string seedName;
	}
}
