using System;

// Token: 0x02000A43 RID: 2627
internal class SafeAreaX : SafeArea
{
	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060033F3 RID: 13299 RVA: 0x000F39B2 File Offset: 0x000F1DB2
	public override string Label
	{
		get
		{
			return "SafeAreaX";
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060033F4 RID: 13300 RVA: 0x000F39B9 File Offset: 0x000F1DB9
	// (set) Token: 0x060033F5 RID: 13301 RVA: 0x000F39C0 File Offset: 0x000F1DC0
	public override float SafeAreaAxis
	{
		get
		{
			return SafeAreaAdjuster.SafeAreaWidth;
		}
		set
		{
			SafeAreaAdjuster.SafeAreaWidth = value;
		}
	}
}
