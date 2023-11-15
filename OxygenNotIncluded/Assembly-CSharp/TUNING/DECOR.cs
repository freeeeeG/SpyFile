using System;
using STRINGS;

namespace TUNING
{
	// Token: 0x02000D8D RID: 3469
	public class DECOR
	{
		// Token: 0x04004F70 RID: 20336
		public static int LIT_BONUS = 15;

		// Token: 0x04004F71 RID: 20337
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x02001C80 RID: 7296
		public class BONUS
		{
			// Token: 0x04008152 RID: 33106
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 10,
				radius = 1
			};

			// Token: 0x04008153 RID: 33107
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 15,
				radius = 2
			};

			// Token: 0x04008154 RID: 33108
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 20,
				radius = 3
			};

			// Token: 0x04008155 RID: 33109
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 25,
				radius = 4
			};

			// Token: 0x04008156 RID: 33110
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04008157 RID: 33111
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 35,
				radius = 6
			};

			// Token: 0x04008158 RID: 33112
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 50,
				radius = 7
			};

			// Token: 0x04008159 RID: 33113
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 80,
				radius = 7
			};

			// Token: 0x0400815A RID: 33114
			public static readonly EffectorValues TIER8 = new EffectorValues
			{
				amount = 200,
				radius = 8
			};
		}

		// Token: 0x02001C81 RID: 7297
		public class PENALTY
		{
			// Token: 0x0400815B RID: 33115
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x0400815C RID: 33116
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x0400815D RID: 33117
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x0400815E RID: 33118
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x0400815F RID: 33119
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04008160 RID: 33120
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}

		// Token: 0x02001C82 RID: 7298
		public class SPACEARTIFACT
		{
			// Token: 0x04008161 RID: 33121
			public static readonly ArtifactTier TIER_NONE = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER_NONE.key, DECOR.NONE, 0f);

			// Token: 0x04008162 RID: 33122
			public static readonly ArtifactTier TIER0 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER0.key, DECOR.BONUS.TIER0, 0.25f);

			// Token: 0x04008163 RID: 33123
			public static readonly ArtifactTier TIER1 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER1.key, DECOR.BONUS.TIER2, 0.4f);

			// Token: 0x04008164 RID: 33124
			public static readonly ArtifactTier TIER2 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER2.key, DECOR.BONUS.TIER4, 0.55f);

			// Token: 0x04008165 RID: 33125
			public static readonly ArtifactTier TIER3 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER3.key, DECOR.BONUS.TIER5, 0.7f);

			// Token: 0x04008166 RID: 33126
			public static readonly ArtifactTier TIER4 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER4.key, DECOR.BONUS.TIER6, 0.85f);

			// Token: 0x04008167 RID: 33127
			public static readonly ArtifactTier TIER5 = new ArtifactTier(UI.SPACEARTIFACTS.ARTIFACTTIERS.TIER5.key, DECOR.BONUS.TIER7, 1f);
		}
	}
}
