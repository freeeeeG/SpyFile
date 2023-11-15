using System;

// Token: 0x020005D6 RID: 1494
[Serializable]
public class ConduitPortInfo
{
	// Token: 0x06002524 RID: 9508 RVA: 0x000CAC21 File Offset: 0x000C8E21
	public ConduitPortInfo(ConduitType type, CellOffset offset)
	{
		this.conduitType = type;
		this.offset = offset;
	}

	// Token: 0x04001548 RID: 5448
	public ConduitType conduitType;

	// Token: 0x04001549 RID: 5449
	public CellOffset offset;
}
