using System;

namespace TUNING
{
	// Token: 0x02000D8F RID: 3471
	public class ROBOTS
	{
		// Token: 0x02001C87 RID: 7303
		public class SCOUTBOT
		{
			// Token: 0x04008182 RID: 33154
			public static readonly float DIGGING = 1f;

			// Token: 0x04008183 RID: 33155
			public static readonly float CONSTRUCTION = 1f;

			// Token: 0x04008184 RID: 33156
			public static readonly float ATHLETICS = 1f;

			// Token: 0x04008185 RID: 33157
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04008186 RID: 33158
			public static readonly float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04008187 RID: 33159
			public static readonly float BATTERY_CAPACITY = ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE * 10f * 600f;
		}
	}
}
