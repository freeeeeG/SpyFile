using System;

// Token: 0x0200052B RID: 1323
public abstract class MinionTracker : Tracker
{
	// Token: 0x06001F95 RID: 8085 RVA: 0x000A8565 File Offset: 0x000A6765
	public MinionTracker(MinionIdentity identity)
	{
		this.identity = identity;
	}

	// Token: 0x040011A4 RID: 4516
	public MinionIdentity identity;
}
