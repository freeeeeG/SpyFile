using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x0200008A RID: 138
public static class CrabTuning
{
	// Token: 0x04000180 RID: 384
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.97f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.01f
		}
	};

	// Token: 0x04000181 RID: 385
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WOOD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000182 RID: 386
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_FRESH = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000183 RID: 387
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000184 RID: 388
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000185 RID: 389
	public static float STANDARD_STOMACH_SIZE = CrabTuning.STANDARD_CALORIES_PER_CYCLE * CrabTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000186 RID: 390
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000187 RID: 391
	public static float EGG_MASS = 2f;
}
