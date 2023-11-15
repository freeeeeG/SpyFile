using System;

namespace TUNING
{
	// Token: 0x02000D9C RID: 3484
	public class EQUIPMENT
	{
		// Token: 0x02001CAD RID: 7341
		public class TOYS
		{
			// Token: 0x0400828E RID: 33422
			public static string SLOT = "Toy";

			// Token: 0x0400828F RID: 33423
			public static float BALLOON_MASS = 1f;
		}

		// Token: 0x02001CAE RID: 7342
		public class ATTRIBUTE_MOD_IDS
		{
			// Token: 0x04008290 RID: 33424
			public static string DECOR = "Decor";

			// Token: 0x04008291 RID: 33425
			public static string INSULATION = "Insulation";

			// Token: 0x04008292 RID: 33426
			public static string ATHLETICS = "Athletics";

			// Token: 0x04008293 RID: 33427
			public static string DIGGING = "Digging";

			// Token: 0x04008294 RID: 33428
			public static string MAX_UNDERWATER_TRAVELCOST = "MaxUnderwaterTravelCost";

			// Token: 0x04008295 RID: 33429
			public static string THERMAL_CONDUCTIVITY_BARRIER = "ThermalConductivityBarrier";
		}

		// Token: 0x02001CAF RID: 7343
		public class TOOLS
		{
			// Token: 0x04008296 RID: 33430
			public static string TOOLSLOT = "Multitool";

			// Token: 0x04008297 RID: 33431
			public static string TOOLFABRICATOR = "MultitoolWorkbench";

			// Token: 0x04008298 RID: 33432
			public static string TOOL_ANIM = "constructor_gun_kanim";
		}

		// Token: 0x02001CB0 RID: 7344
		public class CLOTHING
		{
			// Token: 0x04008299 RID: 33433
			public static string SLOT = "Outfit";
		}

		// Token: 0x02001CB1 RID: 7345
		public class SUITS
		{
			// Token: 0x0400829A RID: 33434
			public static string SLOT = "Suit";

			// Token: 0x0400829B RID: 33435
			public static string FABRICATOR = "SuitFabricator";

			// Token: 0x0400829C RID: 33436
			public static string ANIM = "clothing_kanim";

			// Token: 0x0400829D RID: 33437
			public static string SNAPON = "snapTo_neck";

			// Token: 0x0400829E RID: 33438
			public static float SUIT_DURABILITY_SKILL_BONUS = 0.25f;

			// Token: 0x0400829F RID: 33439
			public static int OXYMASK_FABTIME = 20;

			// Token: 0x040082A0 RID: 33440
			public static int ATMOSUIT_FABTIME = 40;

			// Token: 0x040082A1 RID: 33441
			public static int ATMOSUIT_INSULATION = 50;

			// Token: 0x040082A2 RID: 33442
			public static int ATMOSUIT_ATHLETICS = -6;

			// Token: 0x040082A3 RID: 33443
			public static float ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.2f;

			// Token: 0x040082A4 RID: 33444
			public static int ATMOSUIT_DIGGING = 10;

			// Token: 0x040082A5 RID: 33445
			public static int ATMOSUIT_CONSTRUCTION = 10;

			// Token: 0x040082A6 RID: 33446
			public static float ATMOSUIT_BLADDER = -0.18333334f;

			// Token: 0x040082A7 RID: 33447
			public static int ATMOSUIT_MASS = 200;

			// Token: 0x040082A8 RID: 33448
			public static int ATMOSUIT_SCALDING = 1000;

			// Token: 0x040082A9 RID: 33449
			public static float ATMOSUIT_DECAY = -0.1f;

			// Token: 0x040082AA RID: 33450
			public static float LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.3f;

			// Token: 0x040082AB RID: 33451
			public static int LEADSUIT_SCALDING = 1000;

			// Token: 0x040082AC RID: 33452
			public static int LEADSUIT_INSULATION = 50;

			// Token: 0x040082AD RID: 33453
			public static int LEADSUIT_STRENGTH = 10;

			// Token: 0x040082AE RID: 33454
			public static int LEADSUIT_ATHLETICS = -8;

			// Token: 0x040082AF RID: 33455
			public static float LEADSUIT_RADIATION_SHIELDING = 0.66f;

			// Token: 0x040082B0 RID: 33456
			public static int AQUASUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x040082B1 RID: 33457
			public static int AQUASUIT_INSULATION = 0;

			// Token: 0x040082B2 RID: 33458
			public static int AQUASUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x040082B3 RID: 33459
			public static int AQUASUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x040082B4 RID: 33460
			public static int AQUASUIT_UNDERWATER_TRAVELCOST = 6;

			// Token: 0x040082B5 RID: 33461
			public static int TEMPERATURESUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x040082B6 RID: 33462
			public static float TEMPERATURESUIT_INSULATION = 0.2f;

			// Token: 0x040082B7 RID: 33463
			public static int TEMPERATURESUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x040082B8 RID: 33464
			public static int TEMPERATURESUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x040082B9 RID: 33465
			public const int OXYGEN_MASK_MASS = 15;

			// Token: 0x040082BA RID: 33466
			public static int OXYGEN_MASK_ATHLETICS = -2;

			// Token: 0x040082BB RID: 33467
			public static float OXYGEN_MASK_DECAY = -0.2f;

			// Token: 0x040082BC RID: 33468
			public static float INDESTRUCTIBLE_DURABILITY_MOD = 0f;

			// Token: 0x040082BD RID: 33469
			public static float REINFORCED_DURABILITY_MOD = 0.5f;

			// Token: 0x040082BE RID: 33470
			public static float FLIMSY_DURABILITY_MOD = 1.5f;

			// Token: 0x040082BF RID: 33471
			public static float THREADBARE_DURABILITY_MOD = 2f;

			// Token: 0x040082C0 RID: 33472
			public static float MINIMUM_USABLE_SUIT_CHARGE = 0.95f;
		}

		// Token: 0x02001CB2 RID: 7346
		public class VESTS
		{
			// Token: 0x040082C1 RID: 33473
			public static string SLOT = "Suit";

			// Token: 0x040082C2 RID: 33474
			public static string FABRICATOR = "ClothingFabricator";

			// Token: 0x040082C3 RID: 33475
			public static string SNAPON0 = "snapTo_body";

			// Token: 0x040082C4 RID: 33476
			public static string SNAPON1 = "snapTo_arm";

			// Token: 0x040082C5 RID: 33477
			public static string WARM_VEST_ANIM0 = "body_shirt_hot01_kanim";

			// Token: 0x040082C6 RID: 33478
			public static string WARM_VEST_ANIM1 = "body_shirt_hot02_kanim";

			// Token: 0x040082C7 RID: 33479
			public static string WARM_VEST_ICON0 = "shirt_hot01_kanim";

			// Token: 0x040082C8 RID: 33480
			public static string WARM_VEST_ICON1 = "shirt_hot02_kanim";

			// Token: 0x040082C9 RID: 33481
			public static float WARM_VEST_FABTIME = 180f;

			// Token: 0x040082CA RID: 33482
			public static float WARM_VEST_INSULATION = 0.01f;

			// Token: 0x040082CB RID: 33483
			public static int WARM_VEST_MASS = 4;

			// Token: 0x040082CC RID: 33484
			public static string COOL_VEST_ANIM0 = "body_shirt_cold01_kanim";

			// Token: 0x040082CD RID: 33485
			public static string COOL_VEST_ANIM1 = "body_shirt_cold02_kanim";

			// Token: 0x040082CE RID: 33486
			public static string COOL_VEST_ICON0 = "shirt_cold01_kanim";

			// Token: 0x040082CF RID: 33487
			public static string COOL_VEST_ICON1 = "shirt_cold02_kanim";

			// Token: 0x040082D0 RID: 33488
			public static float COOL_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x040082D1 RID: 33489
			public static float COOL_VEST_INSULATION = 0.01f;

			// Token: 0x040082D2 RID: 33490
			public static int COOL_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x040082D3 RID: 33491
			public static float FUNKY_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x040082D4 RID: 33492
			public static float FUNKY_VEST_DECOR = 1f;

			// Token: 0x040082D5 RID: 33493
			public static int FUNKY_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x040082D6 RID: 33494
			public static float CUSTOM_CLOTHING_FABTIME = 180f;

			// Token: 0x040082D7 RID: 33495
			public static float CUSTOM_ATMOSUIT_FABTIME = 15f;

			// Token: 0x040082D8 RID: 33496
			public static int CUSTOM_CLOTHING_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS + 3;
		}
	}
}
