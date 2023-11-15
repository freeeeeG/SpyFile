using System;

// Token: 0x020004DD RID: 1245
public static class NavigationTactics
{
	// Token: 0x04000FD1 RID: 4049
	public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, 1, 4);

	// Token: 0x04000FD2 RID: 4050
	public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12, 1);

	// Token: 0x04000FD3 RID: 4051
	public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999, 1);
}
