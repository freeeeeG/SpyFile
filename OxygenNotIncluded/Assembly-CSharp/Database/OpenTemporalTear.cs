using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D39 RID: 3385
	public class OpenTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x06006A7C RID: 27260 RVA: 0x002989F7 File Offset: 0x00296BF7
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.OPEN_TEMPORAL_TEAR;
		}

		// Token: 0x06006A7D RID: 27261 RVA: 0x00298A03 File Offset: 0x00296C03
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06006A7E RID: 27262 RVA: 0x00298A11 File Offset: 0x00296C11
		public override bool Success()
		{
			return ClusterManager.Instance.GetComponent<ClusterPOIManager>().IsTemporalTearOpen();
		}

		// Token: 0x06006A7F RID: 27263 RVA: 0x00298A22 File Offset: 0x00296C22
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.OPEN_TEMPORAL_TEAR;
		}
	}
}
