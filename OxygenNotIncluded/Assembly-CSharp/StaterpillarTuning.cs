﻿using System;
using System.Collections.Generic;

// Token: 0x020000A2 RID: 162
public static class StaterpillarTuning
{
	// Token: 0x040001F4 RID: 500
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarGasEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarLiquidEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F5 RID: 501
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_GAS = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarGasEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarLiquidEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F6 RID: 502
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_LIQUID = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarGasEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarLiquidEgg".ToTag(),
			weight = 0.66f
		}
	};

	// Token: 0x040001F7 RID: 503
	public static float STANDARD_CALORIES_PER_CYCLE = 2000000f;

	// Token: 0x040001F8 RID: 504
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x040001F9 RID: 505
	public static float STANDARD_STOMACH_SIZE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE * StaterpillarTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001FA RID: 506
	public static float POOP_CONVERSTION_RATE = 0.05f;

	// Token: 0x040001FB RID: 507
	public static float EGG_MASS = 2f;
}
