using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x02000095 RID: 149
public static class MoleTuning
{
	// Token: 0x040001B1 RID: 433
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001B2 RID: 434
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DELICACY = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040001B3 RID: 435
	public static float STANDARD_CALORIES_PER_CYCLE = 4800000f;

	// Token: 0x040001B4 RID: 436
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001B5 RID: 437
	public static float STANDARD_STOMACH_SIZE = MoleTuning.STANDARD_CALORIES_PER_CYCLE * MoleTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001B6 RID: 438
	public static float DELICACY_STOMACH_SIZE = MoleTuning.STANDARD_STOMACH_SIZE / 2f;

	// Token: 0x040001B7 RID: 439
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER2;

	// Token: 0x040001B8 RID: 440
	public static float EGG_MASS = 2f;

	// Token: 0x040001B9 RID: 441
	public static int DEPTH_TO_HIDE = 2;

	// Token: 0x040001BA RID: 442
	public static HashedString[] GINGER_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger",
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};
}
