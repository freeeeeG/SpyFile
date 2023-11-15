using System;

namespace TUNING
{
	// Token: 0x02000D91 RID: 3473
	public class DISEASE
	{
		// Token: 0x04004FBE RID: 20414
		public const int COUNT_SCALER = 1000;

		// Token: 0x04004FBF RID: 20415
		public const int GENERIC_EMIT_COUNT = 100000;

		// Token: 0x04004FC0 RID: 20416
		public const float GENERIC_EMIT_INTERVAL = 5f;

		// Token: 0x04004FC1 RID: 20417
		public const float GENERIC_INFECTION_RADIUS = 1.5f;

		// Token: 0x04004FC2 RID: 20418
		public const float GENERIC_INFECTION_INTERVAL = 5f;

		// Token: 0x04004FC3 RID: 20419
		public const float STINKY_EMIT_MASS = 0.0025000002f;

		// Token: 0x04004FC4 RID: 20420
		public const float STINKY_EMIT_INTERVAL = 2.5f;

		// Token: 0x04004FC5 RID: 20421
		public const float STORAGE_TRANSFER_RATE = 0.05f;

		// Token: 0x04004FC6 RID: 20422
		public const float WORKABLE_TRANSFER_RATE = 0.33f;

		// Token: 0x04004FC7 RID: 20423
		public const float LADDER_TRANSFER_RATE = 0.005f;

		// Token: 0x04004FC8 RID: 20424
		public const float INTERNAL_GERM_DEATH_MULTIPLIER = -0.00066666666f;

		// Token: 0x04004FC9 RID: 20425
		public const float INTERNAL_GERM_DEATH_ADDEND = -0.8333333f;

		// Token: 0x04004FCA RID: 20426
		public const float MINIMUM_IMMUNE_DAMAGE = 0.00016666666f;

		// Token: 0x02001C98 RID: 7320
		public class DURATION
		{
			// Token: 0x040081EA RID: 33258
			public const float LONG = 10800f;

			// Token: 0x040081EB RID: 33259
			public const float LONGISH = 4620f;

			// Token: 0x040081EC RID: 33260
			public const float NORMAL = 2220f;

			// Token: 0x040081ED RID: 33261
			public const float SHORT = 1020f;

			// Token: 0x040081EE RID: 33262
			public const float TEMPORARY = 180f;

			// Token: 0x040081EF RID: 33263
			public const float VERY_BRIEF = 60f;
		}

		// Token: 0x02001C99 RID: 7321
		public class IMMUNE_ATTACK_STRENGTH_PERCENT
		{
			// Token: 0x040081F0 RID: 33264
			public const float SLOW_3 = 0.00025f;

			// Token: 0x040081F1 RID: 33265
			public const float SLOW_2 = 0.0005f;

			// Token: 0x040081F2 RID: 33266
			public const float SLOW_1 = 0.00125f;

			// Token: 0x040081F3 RID: 33267
			public const float NORMAL = 0.005f;

			// Token: 0x040081F4 RID: 33268
			public const float FAST_1 = 0.0125f;

			// Token: 0x040081F5 RID: 33269
			public const float FAST_2 = 0.05f;

			// Token: 0x040081F6 RID: 33270
			public const float FAST_3 = 0.125f;
		}

		// Token: 0x02001C9A RID: 7322
		public class RADIATION_KILL_RATE
		{
			// Token: 0x040081F7 RID: 33271
			public const float NO_EFFECT = 0f;

			// Token: 0x040081F8 RID: 33272
			public const float SLOW = 1f;

			// Token: 0x040081F9 RID: 33273
			public const float NORMAL = 2.5f;

			// Token: 0x040081FA RID: 33274
			public const float FAST = 5f;
		}

		// Token: 0x02001C9B RID: 7323
		public static class GROWTH_FACTOR
		{
			// Token: 0x040081FB RID: 33275
			public const float NONE = float.PositiveInfinity;

			// Token: 0x040081FC RID: 33276
			public const float DEATH_1 = 12000f;

			// Token: 0x040081FD RID: 33277
			public const float DEATH_2 = 6000f;

			// Token: 0x040081FE RID: 33278
			public const float DEATH_3 = 3000f;

			// Token: 0x040081FF RID: 33279
			public const float DEATH_4 = 1200f;

			// Token: 0x04008200 RID: 33280
			public const float DEATH_5 = 300f;

			// Token: 0x04008201 RID: 33281
			public const float DEATH_MAX = 10f;

			// Token: 0x04008202 RID: 33282
			public const float DEATH_INSTANT = 0f;

			// Token: 0x04008203 RID: 33283
			public const float GROWTH_1 = -12000f;

			// Token: 0x04008204 RID: 33284
			public const float GROWTH_2 = -6000f;

			// Token: 0x04008205 RID: 33285
			public const float GROWTH_3 = -3000f;

			// Token: 0x04008206 RID: 33286
			public const float GROWTH_4 = -1200f;

			// Token: 0x04008207 RID: 33287
			public const float GROWTH_5 = -600f;

			// Token: 0x04008208 RID: 33288
			public const float GROWTH_6 = -300f;

			// Token: 0x04008209 RID: 33289
			public const float GROWTH_7 = -150f;
		}

		// Token: 0x02001C9C RID: 7324
		public static class UNDERPOPULATION_DEATH_RATE
		{
			// Token: 0x0400820A RID: 33290
			public const float NONE = 0f;

			// Token: 0x0400820B RID: 33291
			private const float BASE_NUM_TO_KILL = 400f;

			// Token: 0x0400820C RID: 33292
			public const float SLOW = 0.6666667f;

			// Token: 0x0400820D RID: 33293
			public const float FAST = 2.6666667f;
		}
	}
}
