using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x02000092 RID: 146
public static class LightBugTuning
{
	// Token: 0x040001A4 RID: 420
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001A5 RID: 421
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ORANGE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001A6 RID: 422
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PURPLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001A7 RID: 423
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PINK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001A8 RID: 424
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLUE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001A9 RID: 425
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLACK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001AA RID: 426
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_CRYSTAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001AB RID: 427
	public static float STANDARD_CALORIES_PER_CYCLE = 40000f;

	// Token: 0x040001AC RID: 428
	public static float STANDARD_STARVE_CYCLES = 8f;

	// Token: 0x040001AD RID: 429
	public static float STANDARD_STOMACH_SIZE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE * LightBugTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001AE RID: 430
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001AF RID: 431
	public static float EGG_MASS = 0.2f;
}
