using System;

// Token: 0x0200052D RID: 1325
public struct DataPoint
{
	// Token: 0x06001FA3 RID: 8099 RVA: 0x000A8AC3 File Offset: 0x000A6CC3
	public DataPoint(float start, float end, float value)
	{
		this.periodStart = start;
		this.periodEnd = end;
		this.periodValue = value;
	}

	// Token: 0x040011A9 RID: 4521
	public float periodStart;

	// Token: 0x040011AA RID: 4522
	public float periodEnd;

	// Token: 0x040011AB RID: 4523
	public float periodValue;
}
