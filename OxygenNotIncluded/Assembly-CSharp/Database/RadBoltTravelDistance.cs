using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D61 RID: 3425
	public class RadBoltTravelDistance : ColonyAchievementRequirement
	{
		// Token: 0x06006B1D RID: 27421 RVA: 0x0029B130 File Offset: 0x00299330
		public RadBoltTravelDistance(int travelDistance)
		{
			this.travelDistance = travelDistance;
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x0029B13F File Offset: 0x0029933F
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RADBOLT_TRAVEL, SaveGame.Instance.GetComponent<ColonyAchievementTracker>().radBoltTravelDistance, this.travelDistance);
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x0029B16F File Offset: 0x0029936F
		public override bool Success()
		{
			return SaveGame.Instance.GetComponent<ColonyAchievementTracker>().radBoltTravelDistance > (float)this.travelDistance;
		}

		// Token: 0x04004DC8 RID: 19912
		private int travelDistance;
	}
}
