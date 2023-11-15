using System;

namespace TUNING
{
	// Token: 0x02000DA3 RID: 3491
	public class METEORS
	{
		// Token: 0x02001CC4 RID: 7364
		public class DIFFICULTY
		{
			// Token: 0x0200225F RID: 8799
			public class PEROID_MULTIPLIER
			{
				// Token: 0x040099A4 RID: 39332
				public const float INFREQUENT = 2f;

				// Token: 0x040099A5 RID: 39333
				public const float INTENSE = 1f;

				// Token: 0x040099A6 RID: 39334
				public const float DOOMED = 1f;
			}

			// Token: 0x02002260 RID: 8800
			public class SECONDS_PER_METEOR_MULTIPLIER
			{
				// Token: 0x040099A7 RID: 39335
				public const float INFREQUENT = 1.5f;

				// Token: 0x040099A8 RID: 39336
				public const float INTENSE = 0.8f;

				// Token: 0x040099A9 RID: 39337
				public const float DOOMED = 0.5f;
			}

			// Token: 0x02002261 RID: 8801
			public class BOMBARD_OFF_MULTIPLIER
			{
				// Token: 0x040099AA RID: 39338
				public const float INFREQUENT = 1f;

				// Token: 0x040099AB RID: 39339
				public const float INTENSE = 1f;

				// Token: 0x040099AC RID: 39340
				public const float DOOMED = 0.5f;
			}

			// Token: 0x02002262 RID: 8802
			public class BOMBARD_ON_MULTIPLIER
			{
				// Token: 0x040099AD RID: 39341
				public const float INFREQUENT = 1f;

				// Token: 0x040099AE RID: 39342
				public const float INTENSE = 1f;

				// Token: 0x040099AF RID: 39343
				public const float DOOMED = 1f;
			}

			// Token: 0x02002263 RID: 8803
			public class MASS_MULTIPLIER
			{
				// Token: 0x040099B0 RID: 39344
				public const float INFREQUENT = 1f;

				// Token: 0x040099B1 RID: 39345
				public const float INTENSE = 0.8f;

				// Token: 0x040099B2 RID: 39346
				public const float DOOMED = 0.5f;
			}
		}

		// Token: 0x02001CC5 RID: 7365
		public class IDENTIFY_DURATION
		{
			// Token: 0x0400832B RID: 33579
			public const float TIER1 = 20f;
		}

		// Token: 0x02001CC6 RID: 7366
		public class PEROID
		{
			// Token: 0x0400832C RID: 33580
			public const float TIER1 = 5f;

			// Token: 0x0400832D RID: 33581
			public const float TIER2 = 10f;

			// Token: 0x0400832E RID: 33582
			public const float TIER3 = 20f;

			// Token: 0x0400832F RID: 33583
			public const float TIER4 = 30f;
		}

		// Token: 0x02001CC7 RID: 7367
		public class DURATION
		{
			// Token: 0x04008330 RID: 33584
			public const float TIER0 = 1800f;

			// Token: 0x04008331 RID: 33585
			public const float TIER1 = 3000f;

			// Token: 0x04008332 RID: 33586
			public const float TIER2 = 4200f;

			// Token: 0x04008333 RID: 33587
			public const float TIER3 = 6000f;
		}

		// Token: 0x02001CC8 RID: 7368
		public class DURATION_CLUSTER
		{
			// Token: 0x04008334 RID: 33588
			public const float TIER0 = 75f;

			// Token: 0x04008335 RID: 33589
			public const float TIER1 = 150f;

			// Token: 0x04008336 RID: 33590
			public const float TIER2 = 300f;

			// Token: 0x04008337 RID: 33591
			public const float TIER3 = 600f;

			// Token: 0x04008338 RID: 33592
			public const float TIER4 = 1800f;

			// Token: 0x04008339 RID: 33593
			public const float TIER5 = 3000f;
		}

		// Token: 0x02001CC9 RID: 7369
		public class TRAVEL_DURATION
		{
			// Token: 0x0400833A RID: 33594
			public const float TIER0 = 600f;

			// Token: 0x0400833B RID: 33595
			public const float TIER1 = 3000f;

			// Token: 0x0400833C RID: 33596
			public const float TIER2 = 4500f;

			// Token: 0x0400833D RID: 33597
			public const float TIER3 = 6000f;

			// Token: 0x0400833E RID: 33598
			public const float TIER4 = 12000f;

			// Token: 0x0400833F RID: 33599
			public const float TIER5 = 30000f;
		}

		// Token: 0x02001CCA RID: 7370
		public class BOMBARDMENT_ON
		{
			// Token: 0x04008340 RID: 33600
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);

			// Token: 0x04008341 RID: 33601
			public static MathUtil.MinMax UNLIMITED = new MathUtil.MinMax(10000f, 10000f);

			// Token: 0x04008342 RID: 33602
			public static MathUtil.MinMax CYCLE = new MathUtil.MinMax(600f, 600f);
		}

		// Token: 0x02001CCB RID: 7371
		public class BOMBARDMENT_OFF
		{
			// Token: 0x04008343 RID: 33603
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);
		}

		// Token: 0x02001CCC RID: 7372
		public class TRAVELDURATION
		{
			// Token: 0x04008344 RID: 33604
			public static float TIER0 = 0f;

			// Token: 0x04008345 RID: 33605
			public static float TIER1 = 5f;

			// Token: 0x04008346 RID: 33606
			public static float TIER2 = 10f;

			// Token: 0x04008347 RID: 33607
			public static float TIER3 = 20f;

			// Token: 0x04008348 RID: 33608
			public static float TIER4 = 30f;
		}
	}
}
