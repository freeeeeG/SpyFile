using System;

// Token: 0x02000088 RID: 136
public static class BeeHiveTuning
{
	// Token: 0x04000170 RID: 368
	public static float ORE_DELIVERY_AMOUNT = 1f;

	// Token: 0x04000171 RID: 369
	public static float KG_ORE_EATEN_PER_CYCLE = BeeHiveTuning.ORE_DELIVERY_AMOUNT * 10f;

	// Token: 0x04000172 RID: 370
	public static float STANDARD_CALORIES_PER_CYCLE = 1500000f;

	// Token: 0x04000173 RID: 371
	public static float STANDARD_STARVE_CYCLES = 30f;

	// Token: 0x04000174 RID: 372
	public static float STANDARD_STOMACH_SIZE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE * BeeHiveTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000175 RID: 373
	public static float CALORIES_PER_KG_OF_ORE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE / BeeHiveTuning.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000176 RID: 374
	public static float POOP_CONVERSTION_RATE = 0.9f;

	// Token: 0x04000177 RID: 375
	public static Tag CONSUMED_ORE = SimHashes.UraniumOre.CreateTag();

	// Token: 0x04000178 RID: 376
	public static Tag PRODUCED_ORE = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x04000179 RID: 377
	public static float HIVE_GROWTH_TIME = 2f;

	// Token: 0x0400017A RID: 378
	public static float WASTE_DROPPED_ON_DEATH = 5f;

	// Token: 0x0400017B RID: 379
	public static int GERMS_DROPPED_ON_DEATH = 10000;
}
