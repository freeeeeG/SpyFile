using System;

// Token: 0x02000803 RID: 2051
public interface IClientSurfacePlacementNotified
{
	// Token: 0x0600273F RID: 10047
	void OnSurfacePlacement(ClientAttachStation _station);

	// Token: 0x06002740 RID: 10048
	void OnSurfaceDeplacement(ClientAttachStation _station);
}
