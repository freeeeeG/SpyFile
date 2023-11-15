using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000D9F RID: 3487
	public class ROCKETRY
	{
		// Token: 0x06006C2D RID: 27693 RVA: 0x002AC1EC File Offset: 0x002AA3EC
		public static float MassFromPenaltyPercentage(float penaltyPercentage = 0.5f)
		{
			return -(1f / Mathf.Pow(penaltyPercentage - 1f, 5f));
		}

		// Token: 0x06006C2E RID: 27694 RVA: 0x002AC208 File Offset: 0x002AA408
		public static float CalculateMassWithPenalty(float realMass)
		{
			float b = Mathf.Pow(realMass / ROCKETRY.MASS_PENALTY_DIVISOR, ROCKETRY.MASS_PENALTY_EXPONENT);
			return Mathf.Max(realMass, b);
		}

		// Token: 0x04005071 RID: 20593
		public static float MISSION_DURATION_SCALE = 1800f;

		// Token: 0x04005072 RID: 20594
		public static float MASS_PENALTY_EXPONENT = 3.2f;

		// Token: 0x04005073 RID: 20595
		public static float MASS_PENALTY_DIVISOR = 300f;

		// Token: 0x04005074 RID: 20596
		public const float SELF_DESTRUCT_REFUND_FACTOR = 0.5f;

		// Token: 0x04005075 RID: 20597
		public static float CARGO_CAPACITY_SCALE = 10f;

		// Token: 0x04005076 RID: 20598
		public static float LIQUID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x04005077 RID: 20599
		public static float SOLID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x04005078 RID: 20600
		public static float GAS_CARGO_BAY_CLUSTER_CAPACITY = 1100f;

		// Token: 0x04005079 RID: 20601
		public const float ENTITIES_CARGO_BAY_CLUSTER_CAPACITY = 100f;

		// Token: 0x0400507A RID: 20602
		public static Vector2I ROCKET_INTERIOR_SIZE = new Vector2I(32, 32);

		// Token: 0x02001CB5 RID: 7349
		public class DESTINATION_RESEARCH
		{
			// Token: 0x040082E2 RID: 33506
			public static int EVERGREEN = 10;

			// Token: 0x040082E3 RID: 33507
			public static int BASIC = 50;

			// Token: 0x040082E4 RID: 33508
			public static int HIGH = 150;
		}

		// Token: 0x02001CB6 RID: 7350
		public class DESTINATION_ANALYSIS
		{
			// Token: 0x040082E5 RID: 33509
			public static int DISCOVERED = 50;

			// Token: 0x040082E6 RID: 33510
			public static int COMPLETE = 100;

			// Token: 0x040082E7 RID: 33511
			public static float DEFAULT_CYCLES_PER_DISCOVERY = 0.5f;
		}

		// Token: 0x02001CB7 RID: 7351
		public class DESTINATION_THRUST_COSTS
		{
			// Token: 0x040082E8 RID: 33512
			public static int LOW = 3;

			// Token: 0x040082E9 RID: 33513
			public static int MID = 5;

			// Token: 0x040082EA RID: 33514
			public static int HIGH = 7;

			// Token: 0x040082EB RID: 33515
			public static int VERY_HIGH = 9;
		}

		// Token: 0x02001CB8 RID: 7352
		public class CLUSTER_FOW
		{
			// Token: 0x040082EC RID: 33516
			public static float POINTS_TO_REVEAL = 100f;

			// Token: 0x040082ED RID: 33517
			public static float DEFAULT_CYCLES_PER_REVEAL = 0.5f;
		}

		// Token: 0x02001CB9 RID: 7353
		public class ENGINE_EFFICIENCY
		{
			// Token: 0x040082EE RID: 33518
			public static float WEAK = 20f;

			// Token: 0x040082EF RID: 33519
			public static float MEDIUM = 40f;

			// Token: 0x040082F0 RID: 33520
			public static float STRONG = 60f;

			// Token: 0x040082F1 RID: 33521
			public static float BOOSTER = 30f;
		}

		// Token: 0x02001CBA RID: 7354
		public class ROCKET_HEIGHT
		{
			// Token: 0x040082F2 RID: 33522
			public static int VERY_SHORT = 10;

			// Token: 0x040082F3 RID: 33523
			public static int SHORT = 16;

			// Token: 0x040082F4 RID: 33524
			public static int MEDIUM = 20;

			// Token: 0x040082F5 RID: 33525
			public static int TALL = 25;

			// Token: 0x040082F6 RID: 33526
			public static int VERY_TALL = 35;

			// Token: 0x040082F7 RID: 33527
			public static int MAX_MODULE_STACK_HEIGHT = ROCKETRY.ROCKET_HEIGHT.VERY_TALL - 5;
		}

		// Token: 0x02001CBB RID: 7355
		public class OXIDIZER_EFFICIENCY
		{
			// Token: 0x040082F8 RID: 33528
			public static float VERY_LOW = 0.334f;

			// Token: 0x040082F9 RID: 33529
			public static float LOW = 1f;

			// Token: 0x040082FA RID: 33530
			public static float HIGH = 1.33f;
		}

		// Token: 0x02001CBC RID: 7356
		public class DLC1_OXIDIZER_EFFICIENCY
		{
			// Token: 0x040082FB RID: 33531
			public static float VERY_LOW = 1f;

			// Token: 0x040082FC RID: 33532
			public static float LOW = 2f;

			// Token: 0x040082FD RID: 33533
			public static float HIGH = 4f;
		}

		// Token: 0x02001CBD RID: 7357
		public class CARGO_CONTAINER_MASS
		{
			// Token: 0x040082FE RID: 33534
			public static float STATIC_MASS = 1000f;

			// Token: 0x040082FF RID: 33535
			public static float PAYLOAD_MASS = 1000f;
		}

		// Token: 0x02001CBE RID: 7358
		public class BURDEN
		{
			// Token: 0x04008300 RID: 33536
			public static int INSIGNIFICANT = 1;

			// Token: 0x04008301 RID: 33537
			public static int MINOR = 2;

			// Token: 0x04008302 RID: 33538
			public static int MINOR_PLUS = 3;

			// Token: 0x04008303 RID: 33539
			public static int MODERATE = 4;

			// Token: 0x04008304 RID: 33540
			public static int MODERATE_PLUS = 5;

			// Token: 0x04008305 RID: 33541
			public static int MAJOR = 6;

			// Token: 0x04008306 RID: 33542
			public static int MAJOR_PLUS = 7;

			// Token: 0x04008307 RID: 33543
			public static int MEGA = 9;

			// Token: 0x04008308 RID: 33544
			public static int MONUMENTAL = 15;
		}

		// Token: 0x02001CBF RID: 7359
		public class ENGINE_POWER
		{
			// Token: 0x04008309 RID: 33545
			public static int EARLY_WEAK = 16;

			// Token: 0x0400830A RID: 33546
			public static int EARLY_STRONG = 23;

			// Token: 0x0400830B RID: 33547
			public static int MID_VERY_STRONG = 48;

			// Token: 0x0400830C RID: 33548
			public static int MID_STRONG = 31;

			// Token: 0x0400830D RID: 33549
			public static int MID_WEAK = 27;

			// Token: 0x0400830E RID: 33550
			public static int LATE_STRONG = 34;

			// Token: 0x0400830F RID: 33551
			public static int LATE_VERY_STRONG = 55;
		}

		// Token: 0x02001CC0 RID: 7360
		public class FUEL_COST_PER_DISTANCE
		{
			// Token: 0x04008310 RID: 33552
			public static float VERY_LOW = 0.033333335f;

			// Token: 0x04008311 RID: 33553
			public static float LOW = 0.0375f;

			// Token: 0x04008312 RID: 33554
			public static float MEDIUM = 0.075f;

			// Token: 0x04008313 RID: 33555
			public static float HIGH = 0.09375f;

			// Token: 0x04008314 RID: 33556
			public static float VERY_HIGH = 0.15f;

			// Token: 0x04008315 RID: 33557
			public static float GAS_VERY_LOW = 0.025f;

			// Token: 0x04008316 RID: 33558
			public static float GAS_LOW = 0.027777778f;

			// Token: 0x04008317 RID: 33559
			public static float GAS_HIGH = 0.041666668f;

			// Token: 0x04008318 RID: 33560
			public static float PARTICLES = 0.33333334f;
		}
	}
}
