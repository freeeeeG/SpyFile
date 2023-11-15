using System;

// Token: 0x02000A44 RID: 2628
internal class SafeAreaY : SafeArea
{
	// Token: 0x170003AA RID: 938
	// (get) Token: 0x060033F7 RID: 13303 RVA: 0x000F39D0 File Offset: 0x000F1DD0
	public override string Label
	{
		get
		{
			return "SafeAreaY";
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x060033F8 RID: 13304 RVA: 0x000F39D7 File Offset: 0x000F1DD7
	// (set) Token: 0x060033F9 RID: 13305 RVA: 0x000F39DE File Offset: 0x000F1DDE
	public override float SafeAreaAxis
	{
		get
		{
			return SafeAreaAdjuster.SafeAreaHeight;
		}
		set
		{
			SafeAreaAdjuster.SafeAreaHeight = value;
		}
	}
}
