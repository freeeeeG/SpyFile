using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D3D RID: 3389
	public class CalorieSurplus : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A90 RID: 27280 RVA: 0x00298D38 File Offset: 0x00296F38
		public CalorieSurplus(float surplusAmount)
		{
			this.surplusAmount = (double)surplusAmount;
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x00298D48 File Offset: 0x00296F48
		public override bool Success()
		{
			return (double)(ClusterManager.Instance.CountAllRations() / 1000f) >= this.surplusAmount;
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x00298D66 File Offset: 0x00296F66
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x06006A93 RID: 27283 RVA: 0x00298D71 File Offset: 0x00296F71
		public void Deserialize(IReader reader)
		{
			this.surplusAmount = reader.ReadDouble();
		}

		// Token: 0x06006A94 RID: 27284 RVA: 0x00298D7F File Offset: 0x00296F7F
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, GameUtil.GetFormattedCalories(complete ? ((float)this.surplusAmount) : ClusterManager.Instance.CountAllRations(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.surplusAmount, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04004DA0 RID: 19872
		private double surplusAmount;
	}
}
