using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02000D94 RID: 3476
	public class FOOD
	{
		// Token: 0x04004FDC RID: 20444
		public const float EATING_SECONDS_PER_CALORIE = 2E-05f;

		// Token: 0x04004FDD RID: 20445
		public const float FOOD_CALORIES_PER_CYCLE = 1000000f;

		// Token: 0x04004FDE RID: 20446
		public const int FOOD_AMOUNT_INGREDIENT_ONLY = 0;

		// Token: 0x04004FDF RID: 20447
		public const float KCAL_SMALL_PORTION = 600000f;

		// Token: 0x04004FE0 RID: 20448
		public const float KCAL_BONUS_COOKING_LOW = 250000f;

		// Token: 0x04004FE1 RID: 20449
		public const float KCAL_BASIC_PORTION = 800000f;

		// Token: 0x04004FE2 RID: 20450
		public const float KCAL_PREPARED_FOOD = 4000000f;

		// Token: 0x04004FE3 RID: 20451
		public const float KCAL_BONUS_COOKING_BASIC = 400000f;

		// Token: 0x04004FE4 RID: 20452
		public const float DEFAULT_PRESERVE_TEMPERATURE = 255.15f;

		// Token: 0x04004FE5 RID: 20453
		public const float DEFAULT_ROT_TEMPERATURE = 277.15f;

		// Token: 0x04004FE6 RID: 20454
		public const float HIGH_PRESERVE_TEMPERATURE = 283.15f;

		// Token: 0x04004FE7 RID: 20455
		public const float HIGH_ROT_TEMPERATURE = 308.15f;

		// Token: 0x04004FE8 RID: 20456
		public const float EGG_COOK_TEMPERATURE = 344.15f;

		// Token: 0x04004FE9 RID: 20457
		public const float DEFAULT_MASS = 1f;

		// Token: 0x04004FEA RID: 20458
		public const float DEFAULT_SPICE_MASS = 1f;

		// Token: 0x04004FEB RID: 20459
		public const float ROT_TO_ELEMENT_TIME = 600f;

		// Token: 0x04004FEC RID: 20460
		public const int MUSH_BAR_SPAWN_GERMS = 1000;

		// Token: 0x04004FED RID: 20461
		public const float IDEAL_TEMPERATURE_TOLERANCE = 10f;

		// Token: 0x04004FEE RID: 20462
		public const int FOOD_QUALITY_AWFUL = -1;

		// Token: 0x04004FEF RID: 20463
		public const int FOOD_QUALITY_TERRIBLE = 0;

		// Token: 0x04004FF0 RID: 20464
		public const int FOOD_QUALITY_MEDIOCRE = 1;

		// Token: 0x04004FF1 RID: 20465
		public const int FOOD_QUALITY_GOOD = 2;

		// Token: 0x04004FF2 RID: 20466
		public const int FOOD_QUALITY_GREAT = 3;

		// Token: 0x04004FF3 RID: 20467
		public const int FOOD_QUALITY_AMAZING = 4;

		// Token: 0x04004FF4 RID: 20468
		public const int FOOD_QUALITY_WONDERFUL = 5;

		// Token: 0x04004FF5 RID: 20469
		public const int FOOD_QUALITY_MORE_WONDERFUL = 6;

		// Token: 0x02001C9D RID: 7325
		public class SPOIL_TIME
		{
			// Token: 0x0400820E RID: 33294
			public const float DEFAULT = 4800f;

			// Token: 0x0400820F RID: 33295
			public const float QUICK = 2400f;

			// Token: 0x04008210 RID: 33296
			public const float SLOW = 9600f;

			// Token: 0x04008211 RID: 33297
			public const float VERYSLOW = 19200f;
		}

		// Token: 0x02001C9E RID: 7326
		public class FOOD_TYPES
		{
			// Token: 0x04008212 RID: 33298
			public static readonly EdiblesManager.FoodInfo FIELDRATION = new EdiblesManager.FoodInfo("FieldRation", "", 800000f, -1, 255.15f, 277.15f, 19200f, false);

			// Token: 0x04008213 RID: 33299
			public static readonly EdiblesManager.FoodInfo MUSHBAR = new EdiblesManager.FoodInfo("MushBar", "", 800000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008214 RID: 33300
			public static readonly EdiblesManager.FoodInfo BASICPLANTFOOD = new EdiblesManager.FoodInfo("BasicPlantFood", "", 600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008215 RID: 33301
			public static readonly EdiblesManager.FoodInfo BASICFORAGEPLANT = new EdiblesManager.FoodInfo("BasicForagePlant", "", 800000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x04008216 RID: 33302
			public static readonly EdiblesManager.FoodInfo FORESTFORAGEPLANT = new EdiblesManager.FoodInfo("ForestForagePlant", "", 6400000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x04008217 RID: 33303
			public static readonly EdiblesManager.FoodInfo SWAMPFORAGEPLANT = new EdiblesManager.FoodInfo("SwampForagePlant", "EXPANSION1_ID", 2400000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x04008218 RID: 33304
			public static readonly EdiblesManager.FoodInfo MUSHROOM = new EdiblesManager.FoodInfo(MushroomConfig.ID, "", 2400000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008219 RID: 33305
			public static readonly EdiblesManager.FoodInfo LETTUCE = new EdiblesManager.FoodInfo("Lettuce", "", 400000f, 0, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x0400821A RID: 33306
			public static readonly EdiblesManager.FoodInfo RAWEGG = new EdiblesManager.FoodInfo("RawEgg", "", 1600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x0400821B RID: 33307
			public static readonly EdiblesManager.FoodInfo MEAT = new EdiblesManager.FoodInfo("Meat", "", 1600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x0400821C RID: 33308
			public static readonly EdiblesManager.FoodInfo PLANTMEAT = new EdiblesManager.FoodInfo("PlantMeat", "EXPANSION1_ID", 1200000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x0400821D RID: 33309
			public static readonly EdiblesManager.FoodInfo PRICKLEFRUIT = new EdiblesManager.FoodInfo(PrickleFruitConfig.ID, "", 1600000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x0400821E RID: 33310
			public static readonly EdiblesManager.FoodInfo SWAMPFRUIT = new EdiblesManager.FoodInfo(SwampFruitConfig.ID, "EXPANSION1_ID", 1840000f, 0, 255.15f, 277.15f, 2400f, true);

			// Token: 0x0400821F RID: 33311
			public static readonly EdiblesManager.FoodInfo FISH_MEAT = new EdiblesManager.FoodInfo("FishMeat", "", 1000000f, 2, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x04008220 RID: 33312
			public static readonly EdiblesManager.FoodInfo SHELLFISH_MEAT = new EdiblesManager.FoodInfo("ShellfishMeat", "", 1000000f, 2, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x04008221 RID: 33313
			public static readonly EdiblesManager.FoodInfo WORMBASICFRUIT = new EdiblesManager.FoodInfo("WormBasicFruit", "EXPANSION1_ID", 800000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008222 RID: 33314
			public static readonly EdiblesManager.FoodInfo WORMSUPERFRUIT = new EdiblesManager.FoodInfo("WormSuperFruit", "EXPANSION1_ID", 250000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x04008223 RID: 33315
			public static readonly EdiblesManager.FoodInfo PICKLEDMEAL = new EdiblesManager.FoodInfo("PickledMeal", "", 1800000f, -1, 255.15f, 277.15f, 19200f, true);

			// Token: 0x04008224 RID: 33316
			public static readonly EdiblesManager.FoodInfo BASICPLANTBAR = new EdiblesManager.FoodInfo("BasicPlantBar", "", 1700000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008225 RID: 33317
			public static readonly EdiblesManager.FoodInfo FRIEDMUSHBAR = new EdiblesManager.FoodInfo("FriedMushBar", "", 1050000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008226 RID: 33318
			public static readonly EdiblesManager.FoodInfo GAMMAMUSH = new EdiblesManager.FoodInfo("GammaMush", "", 1050000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x04008227 RID: 33319
			public static readonly EdiblesManager.FoodInfo GRILLED_PRICKLEFRUIT = new EdiblesManager.FoodInfo("GrilledPrickleFruit", "", 2000000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008228 RID: 33320
			public static readonly EdiblesManager.FoodInfo SWAMP_DELIGHTS = new EdiblesManager.FoodInfo("SwampDelights", "EXPANSION1_ID", 2240000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008229 RID: 33321
			public static readonly EdiblesManager.FoodInfo FRIED_MUSHROOM = new EdiblesManager.FoodInfo("FriedMushroom", "", 2800000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x0400822A RID: 33322
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_BREAD = new EdiblesManager.FoodInfo("ColdWheatBread", "", 1200000f, 2, 255.15f, 277.15f, 4800f, true);

			// Token: 0x0400822B RID: 33323
			public static readonly EdiblesManager.FoodInfo COOKED_EGG = new EdiblesManager.FoodInfo("CookedEgg", "", 2800000f, 2, 255.15f, 277.15f, 2400f, true);

			// Token: 0x0400822C RID: 33324
			public static readonly EdiblesManager.FoodInfo COOKED_FISH = new EdiblesManager.FoodInfo("CookedFish", "", 1600000f, 3, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x0400822D RID: 33325
			public static readonly EdiblesManager.FoodInfo COOKED_MEAT = new EdiblesManager.FoodInfo("CookedMeat", "", 4000000f, 3, 255.15f, 277.15f, 2400f, true);

			// Token: 0x0400822E RID: 33326
			public static readonly EdiblesManager.FoodInfo PANCAKES = new EdiblesManager.FoodInfo("Pancakes", "", 3600000f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x0400822F RID: 33327
			public static readonly EdiblesManager.FoodInfo WORMBASICFOOD = new EdiblesManager.FoodInfo("WormBasicFood", "EXPANSION1_ID", 1200000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008230 RID: 33328
			public static readonly EdiblesManager.FoodInfo WORMSUPERFOOD = new EdiblesManager.FoodInfo("WormSuperFood", "EXPANSION1_ID", 2400000f, 3, 255.15f, 277.15f, 19200f, true);

			// Token: 0x04008231 RID: 33329
			public static readonly EdiblesManager.FoodInfo FRUITCAKE = new EdiblesManager.FoodInfo("FruitCake", "", 4000000f, 3, 255.15f, 277.15f, 19200f, false);

			// Token: 0x04008232 RID: 33330
			public static readonly EdiblesManager.FoodInfo SALSA = new EdiblesManager.FoodInfo("Salsa", "", 4400000f, 4, 255.15f, 277.15f, 2400f, true);

			// Token: 0x04008233 RID: 33331
			public static readonly EdiblesManager.FoodInfo SURF_AND_TURF = new EdiblesManager.FoodInfo("SurfAndTurf", "", 6000000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x04008234 RID: 33332
			public static readonly EdiblesManager.FoodInfo MUSHROOM_WRAP = new EdiblesManager.FoodInfo("MushroomWrap", "", 4800000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x04008235 RID: 33333
			public static readonly EdiblesManager.FoodInfo TOFU = new EdiblesManager.FoodInfo("Tofu", "", 3600000f, 2, 255.15f, 277.15f, 2400f, true);

			// Token: 0x04008236 RID: 33334
			public static readonly EdiblesManager.FoodInfo CURRY = new EdiblesManager.FoodInfo("Curry", "", 5000000f, 4, 255.15f, 277.15f, 9600f, true).AddEffects(new List<string>
			{
				"HotStuff"
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

			// Token: 0x04008237 RID: 33335
			public static readonly EdiblesManager.FoodInfo SPICEBREAD = new EdiblesManager.FoodInfo("SpiceBread", "", 4000000f, 5, 255.15f, 277.15f, 4800f, true);

			// Token: 0x04008238 RID: 33336
			public static readonly EdiblesManager.FoodInfo SPICY_TOFU = new EdiblesManager.FoodInfo("SpicyTofu", "", 4000000f, 5, 255.15f, 277.15f, 2400f, true);

			// Token: 0x04008239 RID: 33337
			public static readonly EdiblesManager.FoodInfo QUICHE = new EdiblesManager.FoodInfo("Quiche", "", 6400000f, 5, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x0400823A RID: 33338
			public static readonly EdiblesManager.FoodInfo BERRY_PIE = new EdiblesManager.FoodInfo("BerryPie", "EXPANSION1_ID", 4200000f, 5, 255.15f, 277.15f, 2400f, true);

			// Token: 0x0400823B RID: 33339
			public static readonly EdiblesManager.FoodInfo BURGER = new EdiblesManager.FoodInfo("Burger", "", 6000000f, 6, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"GoodEats"
			}, DlcManager.AVAILABLE_ALL_VERSIONS).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x0400823C RID: 33340
			public static readonly EdiblesManager.FoodInfo BEAN = new EdiblesManager.FoodInfo("BeanPlantSeed", "", 0f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x0400823D RID: 33341
			public static readonly EdiblesManager.FoodInfo SPICENUT = new EdiblesManager.FoodInfo(SpiceNutConfig.ID, "", 0f, 0, 255.15f, 277.15f, 2400f, true);

			// Token: 0x0400823E RID: 33342
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_SEED = new EdiblesManager.FoodInfo("ColdWheatSeed", "", 0f, 0, 283.15f, 308.15f, 9600f, true);
		}

		// Token: 0x02001C9F RID: 7327
		public class RECIPES
		{
			// Token: 0x0400823F RID: 33343
			public static float SMALL_COOK_TIME = 30f;

			// Token: 0x04008240 RID: 33344
			public static float STANDARD_COOK_TIME = 50f;
		}
	}
}
