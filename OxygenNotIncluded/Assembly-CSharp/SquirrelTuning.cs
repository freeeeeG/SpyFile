using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A0 RID: 160
public static class SquirrelTuning
{
	// Token: 0x040001EC RID: 492
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001ED RID: 493
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HUG = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040001EE RID: 494
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040001EF RID: 495
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001F0 RID: 496
	public static float STANDARD_STOMACH_SIZE = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE * SquirrelTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001F1 RID: 497
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001F2 RID: 498
	public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x040001F3 RID: 499
	public static float EGG_MASS = 2f;
}
