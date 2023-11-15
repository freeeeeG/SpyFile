using System;

// Token: 0x02000047 RID: 71
[Serializable]
public struct StatChange
{
	// Token: 0x040001E8 RID: 488
	public StatType type;

	// Token: 0x040001E9 RID: 489
	public bool isFlatMod;

	// Token: 0x040001EA RID: 490
	public int flatValue;

	// Token: 0x040001EB RID: 491
	public float value;
}
