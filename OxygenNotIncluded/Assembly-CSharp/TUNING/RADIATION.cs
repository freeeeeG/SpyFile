using System;

namespace TUNING
{
	// Token: 0x02000D98 RID: 3480
	public class RADIATION
	{
		// Token: 0x0400500B RID: 20491
		public const float GERM_RAD_SCALE = 0.01f;

		// Token: 0x0400500C RID: 20492
		public const float STANDARD_DAILY_RECOVERY = 100f;

		// Token: 0x0400500D RID: 20493
		public const float EXTRA_VOMIT_RECOVERY = 20f;

		// Token: 0x0400500E RID: 20494
		public const float REACT_THRESHOLD = 133f;

		// Token: 0x02001CAA RID: 7338
		public class STANDARD_EMITTER
		{
			// Token: 0x0400827B RID: 33403
			public const float STEADY_PULSE_RATE = 0.2f;

			// Token: 0x0400827C RID: 33404
			public const float DOUBLE_SPEED_PULSE_RATE = 0.1f;

			// Token: 0x0400827D RID: 33405
			public const float RADIUS_SCALE = 1f;
		}

		// Token: 0x02001CAB RID: 7339
		public class RADIATION_PER_SECOND
		{
			// Token: 0x0400827E RID: 33406
			public const float TRIVIAL = 60f;

			// Token: 0x0400827F RID: 33407
			public const float VERY_LOW = 120f;

			// Token: 0x04008280 RID: 33408
			public const float LOW = 240f;

			// Token: 0x04008281 RID: 33409
			public const float MODERATE = 600f;

			// Token: 0x04008282 RID: 33410
			public const float HIGH = 1800f;

			// Token: 0x04008283 RID: 33411
			public const float VERY_HIGH = 4800f;

			// Token: 0x04008284 RID: 33412
			public const int EXTREME = 9600;
		}

		// Token: 0x02001CAC RID: 7340
		public class RADIATION_CONSTANT_RADS_PER_CYCLE
		{
			// Token: 0x04008285 RID: 33413
			public const float LESS_THAN_TRIVIAL = 60f;

			// Token: 0x04008286 RID: 33414
			public const float TRIVIAL = 120f;

			// Token: 0x04008287 RID: 33415
			public const float VERY_LOW = 240f;

			// Token: 0x04008288 RID: 33416
			public const float LOW = 480f;

			// Token: 0x04008289 RID: 33417
			public const float MODERATE = 1200f;

			// Token: 0x0400828A RID: 33418
			public const float MODERATE_PLUS = 2400f;

			// Token: 0x0400828B RID: 33419
			public const float HIGH = 3600f;

			// Token: 0x0400828C RID: 33420
			public const float VERY_HIGH = 8400f;

			// Token: 0x0400828D RID: 33421
			public const int EXTREME = 16800;
		}
	}
}
