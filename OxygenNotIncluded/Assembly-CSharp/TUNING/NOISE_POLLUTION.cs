using System;

namespace TUNING
{
	// Token: 0x02000D8E RID: 3470
	public class NOISE_POLLUTION
	{
		// Token: 0x04004F72 RID: 20338
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x04004F73 RID: 20339
		public static readonly EffectorValues CONE_OF_SILENCE = new EffectorValues
		{
			amount = -120,
			radius = 5
		};

		// Token: 0x04004F74 RID: 20340
		public static float DUPLICANT_TIME_THRESHOLD = 3f;

		// Token: 0x02001C83 RID: 7299
		public class LENGTHS
		{
			// Token: 0x04008168 RID: 33128
			public static float VERYSHORT = 0.25f;

			// Token: 0x04008169 RID: 33129
			public static float SHORT = 0.5f;

			// Token: 0x0400816A RID: 33130
			public static float NORMAL = 1f;

			// Token: 0x0400816B RID: 33131
			public static float LONG = 1.5f;

			// Token: 0x0400816C RID: 33132
			public static float VERYLONG = 2f;
		}

		// Token: 0x02001C84 RID: 7300
		public class NOISY
		{
			// Token: 0x0400816D RID: 33133
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 45,
				radius = 10
			};

			// Token: 0x0400816E RID: 33134
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 55,
				radius = 10
			};

			// Token: 0x0400816F RID: 33135
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 65,
				radius = 10
			};

			// Token: 0x04008170 RID: 33136
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 75,
				radius = 15
			};

			// Token: 0x04008171 RID: 33137
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 90,
				radius = 15
			};

			// Token: 0x04008172 RID: 33138
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 105,
				radius = 20
			};

			// Token: 0x04008173 RID: 33139
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 125,
				radius = 20
			};
		}

		// Token: 0x02001C85 RID: 7301
		public class CREATURES
		{
			// Token: 0x04008174 RID: 33140
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x04008175 RID: 33141
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 35,
				radius = 5
			};

			// Token: 0x04008176 RID: 33142
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 45,
				radius = 5
			};

			// Token: 0x04008177 RID: 33143
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 55,
				radius = 5
			};

			// Token: 0x04008178 RID: 33144
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 65,
				radius = 5
			};

			// Token: 0x04008179 RID: 33145
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 75,
				radius = 5
			};

			// Token: 0x0400817A RID: 33146
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 90,
				radius = 10
			};

			// Token: 0x0400817B RID: 33147
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 105,
				radius = 10
			};
		}

		// Token: 0x02001C86 RID: 7302
		public class DAMPEN
		{
			// Token: 0x0400817C RID: 33148
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x0400817D RID: 33149
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x0400817E RID: 33150
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x0400817F RID: 33151
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04008180 RID: 33152
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04008181 RID: 33153
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}
	}
}
