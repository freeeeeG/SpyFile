using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D46 RID: 3398
	public class EatXCalories : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AB7 RID: 27319 RVA: 0x002998AD File Offset: 0x00297AAD
		public EatXCalories(int numCalories)
		{
			this.numCalories = numCalories;
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x002998BC File Offset: 0x00297ABC
		public override bool Success()
		{
			return RationTracker.Get().GetCaloriesConsumed() / 1000f > (float)this.numCalories;
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x002998D7 File Offset: 0x00297AD7
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x06006ABA RID: 27322 RVA: 0x002998E8 File Offset: 0x00297AE8
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : RationTracker.Get().GetCaloriesConsumed(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04004DAF RID: 19887
		private int numCalories;
	}
}
