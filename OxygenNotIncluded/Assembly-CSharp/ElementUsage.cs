using System;

// Token: 0x02000AD3 RID: 2771
public class ElementUsage
{
	// Token: 0x0600554B RID: 21835 RVA: 0x001F0814 File Offset: 0x001EEA14
	public ElementUsage(Tag tag, float amount, bool continuous)
	{
		this.tag = tag;
		this.amount = amount;
		this.continuous = continuous;
	}

	// Token: 0x040038E2 RID: 14562
	public Tag tag;

	// Token: 0x040038E3 RID: 14563
	public float amount;

	// Token: 0x040038E4 RID: 14564
	public bool continuous;
}
