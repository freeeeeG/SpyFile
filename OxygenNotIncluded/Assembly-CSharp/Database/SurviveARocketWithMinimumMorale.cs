using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D63 RID: 3427
	public class SurviveARocketWithMinimumMorale : ColonyAchievementRequirement
	{
		// Token: 0x06006B23 RID: 27427 RVA: 0x0029B1AE File Offset: 0x002993AE
		public SurviveARocketWithMinimumMorale(float minimumMorale, int numberOfCycles)
		{
			this.minimumMorale = minimumMorale;
			this.numberOfCycles = numberOfCycles;
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x0029B1C4 File Offset: 0x002993C4
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE_COMPLETE, this.minimumMorale, this.numberOfCycles);
			}
			return base.GetProgress(complete);
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x0029B1F8 File Offset: 0x002993F8
		public override bool Success()
		{
			foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.GetComponent<ColonyAchievementTracker>().cyclesRocketDupeMoraleAboveRequirement)
			{
				if (keyValuePair.Value >= this.numberOfCycles)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004DC9 RID: 19913
		public float minimumMorale;

		// Token: 0x04004DCA RID: 19914
		public int numberOfCycles;
	}
}
