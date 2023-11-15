using System;

namespace TUNING
{
	// Token: 0x02000D88 RID: 3464
	public class FIXEDTRAITS
	{
		// Token: 0x02001C77 RID: 7287
		public class SUNLIGHT
		{
			// Token: 0x04008120 RID: 33056
			public static int DEFAULT_SPACED_OUT_SUNLIGHT = 40000;

			// Token: 0x04008121 RID: 33057
			public static int NONE = 0;

			// Token: 0x04008122 RID: 33058
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.25f);

			// Token: 0x04008123 RID: 33059
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.5f);

			// Token: 0x04008124 RID: 33060
			public static int LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.75f);

			// Token: 0x04008125 RID: 33061
			public static int MED_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.875f);

			// Token: 0x04008126 RID: 33062
			public static int MED = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT;

			// Token: 0x04008127 RID: 33063
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.25f);

			// Token: 0x04008128 RID: 33064
			public static int HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.5f);

			// Token: 0x04008129 RID: 33065
			public static int VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2;

			// Token: 0x0400812A RID: 33066
			public static int VERY_VERY_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2.5f);

			// Token: 0x0400812B RID: 33067
			public static int VERY_VERY_VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 3;

			// Token: 0x0400812C RID: 33068
			public static int DEFAULT_VALUE = FIXEDTRAITS.SUNLIGHT.VERY_HIGH;

			// Token: 0x02002246 RID: 8774
			public class NAME
			{
				// Token: 0x0400992B RID: 39211
				public static string NONE = "sunlightNone";

				// Token: 0x0400992C RID: 39212
				public static string VERY_VERY_LOW = "sunlightVeryVeryLow";

				// Token: 0x0400992D RID: 39213
				public static string VERY_LOW = "sunlightVeryLow";

				// Token: 0x0400992E RID: 39214
				public static string LOW = "sunlightLow";

				// Token: 0x0400992F RID: 39215
				public static string MED_LOW = "sunlightMedLow";

				// Token: 0x04009930 RID: 39216
				public static string MED = "sunlightMed";

				// Token: 0x04009931 RID: 39217
				public static string MED_HIGH = "sunlightMedHigh";

				// Token: 0x04009932 RID: 39218
				public static string HIGH = "sunlightHigh";

				// Token: 0x04009933 RID: 39219
				public static string VERY_HIGH = "sunlightVeryHigh";

				// Token: 0x04009934 RID: 39220
				public static string VERY_VERY_HIGH = "sunlightVeryVeryHigh";

				// Token: 0x04009935 RID: 39221
				public static string VERY_VERY_VERY_HIGH = "sunlightVeryVeryVeryHigh";

				// Token: 0x04009936 RID: 39222
				public static string DEFAULT = FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH;
			}
		}

		// Token: 0x02001C78 RID: 7288
		public class COSMICRADIATION
		{
			// Token: 0x0400812D RID: 33069
			public static int BASELINE = 250;

			// Token: 0x0400812E RID: 33070
			public static int NONE = 0;

			// Token: 0x0400812F RID: 33071
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.25f);

			// Token: 0x04008130 RID: 33072
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.5f);

			// Token: 0x04008131 RID: 33073
			public static int LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.75f);

			// Token: 0x04008132 RID: 33074
			public static int MED_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.875f);

			// Token: 0x04008133 RID: 33075
			public static int MED = FIXEDTRAITS.COSMICRADIATION.BASELINE;

			// Token: 0x04008134 RID: 33076
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.25f);

			// Token: 0x04008135 RID: 33077
			public static int HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.5f);

			// Token: 0x04008136 RID: 33078
			public static int VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 2;

			// Token: 0x04008137 RID: 33079
			public static int VERY_VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 3;

			// Token: 0x04008138 RID: 33080
			public static int DEFAULT_VALUE = FIXEDTRAITS.COSMICRADIATION.MED;

			// Token: 0x04008139 RID: 33081
			public static float TELESCOPE_RADIATION_SHIELDING = 0.5f;

			// Token: 0x02002247 RID: 8775
			public class NAME
			{
				// Token: 0x04009937 RID: 39223
				public static string NONE = "cosmicRadiationNone";

				// Token: 0x04009938 RID: 39224
				public static string VERY_VERY_LOW = "cosmicRadiationVeryVeryLow";

				// Token: 0x04009939 RID: 39225
				public static string VERY_LOW = "cosmicRadiationVeryLow";

				// Token: 0x0400993A RID: 39226
				public static string LOW = "cosmicRadiationLow";

				// Token: 0x0400993B RID: 39227
				public static string MED_LOW = "cosmicRadiationMedLow";

				// Token: 0x0400993C RID: 39228
				public static string MED = "cosmicRadiationMed";

				// Token: 0x0400993D RID: 39229
				public static string MED_HIGH = "cosmicRadiationMedHigh";

				// Token: 0x0400993E RID: 39230
				public static string HIGH = "cosmicRadiationHigh";

				// Token: 0x0400993F RID: 39231
				public static string VERY_HIGH = "cosmicRadiationVeryHigh";

				// Token: 0x04009940 RID: 39232
				public static string VERY_VERY_HIGH = "cosmicRadiationVeryVeryHigh";

				// Token: 0x04009941 RID: 39233
				public static string DEFAULT = FIXEDTRAITS.COSMICRADIATION.NAME.MED;
			}
		}
	}
}
