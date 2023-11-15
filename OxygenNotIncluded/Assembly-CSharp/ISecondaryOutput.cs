using System;

// Token: 0x020005D2 RID: 1490
public interface ISecondaryOutput
{
	// Token: 0x06002500 RID: 9472
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06002501 RID: 9473
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
