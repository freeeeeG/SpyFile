using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x0200008E RID: 142
public static class DreckoTuning
{
	// Token: 0x04000194 RID: 404
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000195 RID: 405
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PLASTIC = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000196 RID: 406
	public static float STANDARD_CALORIES_PER_CYCLE = 2000000f;

	// Token: 0x04000197 RID: 407
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000198 RID: 408
	public static float STANDARD_STOMACH_SIZE = DreckoTuning.STANDARD_CALORIES_PER_CYCLE * DreckoTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000199 RID: 409
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400019A RID: 410
	public static float EGG_MASS = 2f;
}
