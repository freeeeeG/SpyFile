using System;

// Token: 0x020003DC RID: 988
public class Urge : Resource
{
	// Token: 0x060014CA RID: 5322 RVA: 0x0006E114 File Offset: 0x0006C314
	public Urge(string id) : base(id, null, null)
	{
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x0006E11F File Offset: 0x0006C31F
	public override string ToString()
	{
		return this.Id;
	}
}
