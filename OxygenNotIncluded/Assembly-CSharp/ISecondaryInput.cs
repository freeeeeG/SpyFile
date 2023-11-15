using System;

// Token: 0x020005D3 RID: 1491
public interface ISecondaryInput
{
	// Token: 0x06002502 RID: 9474
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06002503 RID: 9475
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
