using System;

namespace TUNING
{
	// Token: 0x02000D8B RID: 3467
	public class SKILLS
	{
		// Token: 0x04004F60 RID: 20320
		public static int TARGET_SKILLS_EARNED = 15;

		// Token: 0x04004F61 RID: 20321
		public static int TARGET_SKILLS_CYCLE = 250;

		// Token: 0x04004F62 RID: 20322
		public static float EXPERIENCE_LEVEL_POWER = 1.44f;

		// Token: 0x04004F63 RID: 20323
		public static float PASSIVE_EXPERIENCE_PORTION = 0.5f;

		// Token: 0x04004F64 RID: 20324
		public static float ACTIVE_EXPERIENCE_PORTION = 0.6f;

		// Token: 0x04004F65 RID: 20325
		public static float FULL_EXPERIENCE = 1f;

		// Token: 0x04004F66 RID: 20326
		public static float ALL_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.9f;

		// Token: 0x04004F67 RID: 20327
		public static float MOST_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.75f;

		// Token: 0x04004F68 RID: 20328
		public static float PART_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.5f;

		// Token: 0x04004F69 RID: 20329
		public static float BARELY_EVER_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.25f;

		// Token: 0x04004F6A RID: 20330
		public static float APTITUDE_EXPERIENCE_MULTIPLIER = 0.5f;

		// Token: 0x04004F6B RID: 20331
		public static int[] SKILL_TIER_MORALE_COST = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
	}
}
