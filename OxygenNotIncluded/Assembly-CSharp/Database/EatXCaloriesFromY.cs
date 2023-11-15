using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D45 RID: 3397
	public class EatXCaloriesFromY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AB3 RID: 27315 RVA: 0x002997C5 File Offset: 0x002979C5
		public EatXCaloriesFromY(int numCalories, List<string> fromFoodType)
		{
			this.numCalories = numCalories;
			this.fromFoodType = fromFoodType;
		}

		// Token: 0x06006AB4 RID: 27316 RVA: 0x002997E6 File Offset: 0x002979E6
		public override bool Success()
		{
			return RationTracker.Get().GetCaloiresConsumedByFood(this.fromFoodType) / 1000f > (float)this.numCalories;
		}

		// Token: 0x06006AB5 RID: 27317 RVA: 0x00299808 File Offset: 0x00297A08
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
			int num = reader.ReadInt32();
			this.fromFoodType = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadKleiString();
				this.fromFoodType.Add(item);
			}
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x00299854 File Offset: 0x00297A54
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIES_FROM_MEAT, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : RationTracker.Get().GetCaloiresConsumedByFood(this.fromFoodType), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x04004DAD RID: 19885
		private int numCalories;

		// Token: 0x04004DAE RID: 19886
		private List<string> fromFoodType = new List<string>();
	}
}
