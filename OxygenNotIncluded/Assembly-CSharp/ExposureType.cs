using System;
using System.Collections.Generic;

// Token: 0x0200087D RID: 2173
public class ExposureType
{
	// Token: 0x04002919 RID: 10521
	public string germ_id;

	// Token: 0x0400291A RID: 10522
	public string sickness_id;

	// Token: 0x0400291B RID: 10523
	public string infection_effect;

	// Token: 0x0400291C RID: 10524
	public int exposure_threshold;

	// Token: 0x0400291D RID: 10525
	public bool infect_immediately;

	// Token: 0x0400291E RID: 10526
	public List<string> required_traits;

	// Token: 0x0400291F RID: 10527
	public List<string> excluded_traits;

	// Token: 0x04002920 RID: 10528
	public List<string> excluded_effects;

	// Token: 0x04002921 RID: 10529
	public int base_resistance;
}
