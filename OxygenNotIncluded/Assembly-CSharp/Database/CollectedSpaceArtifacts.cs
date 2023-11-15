using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D38 RID: 3384
	public class CollectedSpaceArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x06006A76 RID: 27254 RVA: 0x00298954 File Offset: 0x00296B54
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_SPACE_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedSpaceArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x06006A77 RID: 27255 RVA: 0x00298997 File Offset: 0x00296B97
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06006A78 RID: 27256 RVA: 0x002989A5 File Offset: 0x00296BA5
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount >= 10;
		}

		// Token: 0x06006A79 RID: 27257 RVA: 0x002989B8 File Offset: 0x00296BB8
		private int GetStudiedSpaceArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount;
		}

		// Token: 0x06006A7A RID: 27258 RVA: 0x002989C4 File Offset: 0x00296BC4
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_SPACE_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04004D9B RID: 19867
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
