using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x0200009E RID: 158
public static class PuftTuning
{
	// Token: 0x040001E3 RID: 483
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftOxyliteEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftBleachstoneEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001E4 RID: 484
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ALPHA = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001E5 RID: 485
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_OXYLITE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.31f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftOxyliteEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040001E6 RID: 486
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLEACHSTONE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.31f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftBleachstoneEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040001E7 RID: 487
	public static float STANDARD_CALORIES_PER_CYCLE = 200000f;

	// Token: 0x040001E8 RID: 488
	public static float STANDARD_STARVE_CYCLES = 6f;

	// Token: 0x040001E9 RID: 489
	public static float STANDARD_STOMACH_SIZE = PuftTuning.STANDARD_CALORIES_PER_CYCLE * PuftTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001EA RID: 490
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x040001EB RID: 491
	public static float EGG_MASS = 0.5f;
}
