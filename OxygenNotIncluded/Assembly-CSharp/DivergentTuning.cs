using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x0200008C RID: 140
public static class DivergentTuning
{
	// Token: 0x04000188 RID: 392
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BEETLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000189 RID: 393
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WORM = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x0400018A RID: 394
	public static int TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION = 2;

	// Token: 0x0400018B RID: 395
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x0400018C RID: 396
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x0400018D RID: 397
	public static float STANDARD_STOMACH_SIZE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE * DivergentTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400018E RID: 398
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400018F RID: 399
	public static int PEN_SIZE_PER_CREATURE_WORM = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x04000190 RID: 400
	public static float EGG_MASS = 2f;
}
