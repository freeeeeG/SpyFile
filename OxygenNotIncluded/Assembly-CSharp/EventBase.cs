using System;

// Token: 0x020007B1 RID: 1969
public class EventBase : Resource
{
	// Token: 0x06003698 RID: 13976 RVA: 0x00126EBB File Offset: 0x001250BB
	public EventBase(string id) : base(id, id)
	{
		this.hash = Hash.SDBMLower(id);
	}

	// Token: 0x06003699 RID: 13977 RVA: 0x00126ED1 File Offset: 0x001250D1
	public virtual string GetDescription(EventInstanceBase ev)
	{
		return "";
	}

	// Token: 0x04002186 RID: 8582
	public int hash;
}
