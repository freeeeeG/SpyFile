using System;

// Token: 0x0200052A RID: 1322
public abstract class WorldTracker : Tracker
{
	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06001F92 RID: 8082 RVA: 0x000A8545 File Offset: 0x000A6745
	// (set) Token: 0x06001F93 RID: 8083 RVA: 0x000A854D File Offset: 0x000A674D
	public int WorldID { get; private set; }

	// Token: 0x06001F94 RID: 8084 RVA: 0x000A8556 File Offset: 0x000A6756
	public WorldTracker(int worldID)
	{
		this.WorldID = worldID;
	}
}
