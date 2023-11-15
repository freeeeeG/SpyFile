using System;

// Token: 0x020008DA RID: 2266
public struct PlantElementAbsorber
{
	// Token: 0x06004188 RID: 16776 RVA: 0x0016ECFD File Offset: 0x0016CEFD
	public void Clear()
	{
		this.storage = null;
		this.consumedElements = null;
	}

	// Token: 0x04002AA8 RID: 10920
	public Storage storage;

	// Token: 0x04002AA9 RID: 10921
	public PlantElementAbsorber.LocalInfo localInfo;

	// Token: 0x04002AAA RID: 10922
	public HandleVector<int>.Handle[] accumulators;

	// Token: 0x04002AAB RID: 10923
	public PlantElementAbsorber.ConsumeInfo[] consumedElements;

	// Token: 0x02001715 RID: 5909
	public struct ConsumeInfo
	{
		// Token: 0x06008D69 RID: 36201 RVA: 0x0031B903 File Offset: 0x00319B03
		public ConsumeInfo(Tag tag, float mass_consumption_rate)
		{
			this.tag = tag;
			this.massConsumptionRate = mass_consumption_rate;
		}

		// Token: 0x04006DA9 RID: 28073
		public Tag tag;

		// Token: 0x04006DAA RID: 28074
		public float massConsumptionRate;
	}

	// Token: 0x02001716 RID: 5910
	public struct LocalInfo
	{
		// Token: 0x04006DAB RID: 28075
		public Tag tag;

		// Token: 0x04006DAC RID: 28076
		public float massConsumptionRate;
	}
}
