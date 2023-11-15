using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D5F RID: 3423
	public class HarvestAmountFromSpacePOI : ColonyAchievementRequirement
	{
		// Token: 0x06006B17 RID: 27415 RVA: 0x0029AFD5 File Offset: 0x002991D5
		public HarvestAmountFromSpacePOI(float amountToHarvest)
		{
			this.amountToHarvest = amountToHarvest;
		}

		// Token: 0x06006B18 RID: 27416 RVA: 0x0029AFE4 File Offset: 0x002991E4
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MINE_SPACE_POI, SaveGame.Instance.GetComponent<ColonyAchievementTracker>().totalMaterialsHarvestFromPOI, this.amountToHarvest);
		}

		// Token: 0x06006B19 RID: 27417 RVA: 0x0029B014 File Offset: 0x00299214
		public override bool Success()
		{
			return SaveGame.Instance.GetComponent<ColonyAchievementTracker>().totalMaterialsHarvestFromPOI > this.amountToHarvest;
		}

		// Token: 0x04004DC7 RID: 19911
		private float amountToHarvest;
	}
}
