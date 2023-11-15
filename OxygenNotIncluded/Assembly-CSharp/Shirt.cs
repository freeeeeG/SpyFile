using System;

// Token: 0x0200096C RID: 2412
public class Shirt : Resource
{
	// Token: 0x060046C2 RID: 18114 RVA: 0x0018F776 File Offset: 0x0018D976
	public Shirt(string id) : base(id, null, null)
	{
		this.hash = new HashedString(id);
	}

	// Token: 0x04002EEA RID: 12010
	public HashedString hash;
}
