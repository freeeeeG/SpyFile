using System;
using System.Collections.Generic;

// Token: 0x020001CC RID: 460
public struct ModeData
{
	// Token: 0x040005DD RID: 1501
	public int ModeID;

	// Token: 0x040005DE RID: 1502
	public int Wave;

	// Token: 0x040005DF RID: 1503
	public StrategyBase HighestTurretStrategy;

	// Token: 0x040005E0 RID: 1504
	public string HighestTurretName;

	// Token: 0x040005E1 RID: 1505
	public int HighestTurretDamage;

	// Token: 0x040005E2 RID: 1506
	public List<string> HighestTurretSkills;

	// Token: 0x040005E3 RID: 1507
	public int BeforeLost;
}
