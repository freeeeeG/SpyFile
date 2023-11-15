using System;

// Token: 0x02000802 RID: 2050
public interface ISurfacePlacementNotified
{
	// Token: 0x0600273D RID: 10045
	void OnSurfacePlacement(ServerAttachStation _station);

	// Token: 0x0600273E RID: 10046
	void OnSurfaceDeplacement(ServerAttachStation _station);
}
