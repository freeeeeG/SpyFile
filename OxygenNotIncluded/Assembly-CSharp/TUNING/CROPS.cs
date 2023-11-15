﻿using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02000D95 RID: 3477
	public class CROPS
	{
		// Token: 0x04004FF6 RID: 20470
		public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;

		// Token: 0x04004FF7 RID: 20471
		public const float GROWTH_RATE = 0.0016666667f;

		// Token: 0x04004FF8 RID: 20472
		public const float WILD_GROWTH_RATE = 0.00041666668f;

		// Token: 0x04004FF9 RID: 20473
		public const float PLANTERPLOT_GROWTH_PENTALY = -0.5f;

		// Token: 0x04004FFA RID: 20474
		public const float BASE_BONUS_SEED_PROBABILITY = 0.1f;

		// Token: 0x04004FFB RID: 20475
		public const float SELF_HARVEST_TIME = 2400f;

		// Token: 0x04004FFC RID: 20476
		public const float SELF_PLANT_TIME = 2400f;

		// Token: 0x04004FFD RID: 20477
		public const float TREE_BRANCH_SELF_HARVEST_TIME = 12000f;

		// Token: 0x04004FFE RID: 20478
		public const float FERTILIZATION_GAIN_RATE = 1.6666666f;

		// Token: 0x04004FFF RID: 20479
		public const float FERTILIZATION_LOSS_RATE = -0.16666667f;

		// Token: 0x04005000 RID: 20480
		public static List<Crop.CropVal> CROP_TYPES = new List<Crop.CropVal>
		{
			new Crop.CropVal("BasicPlantFood", 1800f, 1, true),
			new Crop.CropVal(PrickleFruitConfig.ID, 3600f, 1, true),
			new Crop.CropVal(SwampFruitConfig.ID, 3960f, 1, true),
			new Crop.CropVal(MushroomConfig.ID, 4500f, 1, true),
			new Crop.CropVal("ColdWheatSeed", 10800f, 18, true),
			new Crop.CropVal(SpiceNutConfig.ID, 4800f, 4, true),
			new Crop.CropVal(BasicFabricConfig.ID, 1200f, 1, true),
			new Crop.CropVal(SwampLilyFlowerConfig.ID, 7200f, 2, true),
			new Crop.CropVal("GasGrassHarvested", 2400f, 1, true),
			new Crop.CropVal("WoodLog", 2700f, 300, true),
			new Crop.CropVal("Lettuce", 7200f, 12, true),
			new Crop.CropVal("BeanPlantSeed", 12600f, 12, true),
			new Crop.CropVal("OxyfernSeed", 7200f, 1, true),
			new Crop.CropVal("PlantMeat", 18000f, 10, true),
			new Crop.CropVal("WormBasicFruit", 2400f, 1, true),
			new Crop.CropVal("WormSuperFruit", 4800f, 8, true),
			new Crop.CropVal(SimHashes.Salt.ToString(), 3600f, 65, true),
			new Crop.CropVal(SimHashes.Water.ToString(), 6000f, 350, true)
		};
	}
}
