using System;

// Token: 0x02000031 RID: 49
public abstract class Modifier
{
	// Token: 0x060003BE RID: 958 RVA: 0x00014A02 File Offset: 0x00012C02
	public Modifier(int sortOrder)
	{
		this.sortOrder = sortOrder;
	}

	// Token: 0x040001CF RID: 463
	public readonly int sortOrder;
}
