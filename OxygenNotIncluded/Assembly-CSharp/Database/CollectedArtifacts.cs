using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D37 RID: 3383
	public class CollectedArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x06006A70 RID: 27248 RVA: 0x002988B0 File Offset: 0x00296AB0
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x06006A71 RID: 27249 RVA: 0x002988F3 File Offset: 0x00296AF3
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06006A72 RID: 27250 RVA: 0x00298901 File Offset: 0x00296B01
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount >= 10;
		}

		// Token: 0x06006A73 RID: 27251 RVA: 0x00298914 File Offset: 0x00296B14
		private int GetStudiedArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount;
		}

		// Token: 0x06006A74 RID: 27252 RVA: 0x00298920 File Offset: 0x00296B20
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04004D9A RID: 19866
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
