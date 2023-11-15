using System;
using TUNING;
using UnityEngine;

// Token: 0x02000097 RID: 151
public static class MooTuning
{
	// Token: 0x040001BC RID: 444
	public static readonly float STANDARD_LIFESPAN = 75f;

	// Token: 0x040001BD RID: 445
	public static readonly float STANDARD_CALORIES_PER_CYCLE = 200000f;

	// Token: 0x040001BE RID: 446
	public static readonly float STANDARD_STARVE_CYCLES = 6f;

	// Token: 0x040001BF RID: 447
	public static readonly float STANDARD_STOMACH_SIZE = MooTuning.STANDARD_CALORIES_PER_CYCLE * MooTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001C0 RID: 448
	public static readonly int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x040001C1 RID: 449
	private static readonly float BECKONS_PER_LIFESPAN = 4f;

	// Token: 0x040001C2 RID: 450
	private static readonly float BECKON_FUDGE_CYCLES = 11f;

	// Token: 0x040001C3 RID: 451
	private static readonly float BECKON_CYCLES = Mathf.Floor((MooTuning.STANDARD_LIFESPAN - MooTuning.BECKON_FUDGE_CYCLES) / MooTuning.BECKONS_PER_LIFESPAN);

	// Token: 0x040001C4 RID: 452
	public static readonly float WELLFED_EFFECT = 100f / (600f * MooTuning.BECKON_CYCLES);

	// Token: 0x040001C5 RID: 453
	public static readonly float WELLFED_CALORIES_PER_CYCLE = MooTuning.STANDARD_CALORIES_PER_CYCLE * 0.9f;

	// Token: 0x040001C6 RID: 454
	public static readonly float ELIGIBLE_MILKING_PERCENTAGE = 1f;

	// Token: 0x040001C7 RID: 455
	public static readonly float MILK_PER_CYCLE = 50f;

	// Token: 0x040001C8 RID: 456
	private static readonly float CYCLES_UNTIL_MILKING = 4f;

	// Token: 0x040001C9 RID: 457
	public static readonly float MILK_CAPACITY = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x040001CA RID: 458
	public static readonly float MILK_AMOUNT_AT_MILKING = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x040001CB RID: 459
	public static readonly float MILK_PRODUCTION_PERCENTAGE_PER_SECOND = 100f / (600f * MooTuning.CYCLES_UNTIL_MILKING);
}
