using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x02000090 RID: 144
public static class HatchTuning
{
	// Token: 0x0400019B RID: 411
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400019C RID: 412
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HARD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400019D RID: 413
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_VEGGIE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x0400019E RID: 414
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_METAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.11f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.22f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x0400019F RID: 415
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x040001A0 RID: 416
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001A1 RID: 417
	public static float STANDARD_STOMACH_SIZE = HatchTuning.STANDARD_CALORIES_PER_CYCLE * HatchTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001A2 RID: 418
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001A3 RID: 419
	public static float EGG_MASS = 2f;
}
