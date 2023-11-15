using System;

// Token: 0x020003F2 RID: 1010
public class NavMask
{
	// Token: 0x0600155C RID: 5468 RVA: 0x0007102E File Offset: 0x0006F22E
	public virtual bool IsTraversable(PathFinder.PotentialPath path, int from_cell, int cost, int transition_id, PathFinderAbilities abilities)
	{
		return true;
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x00071031 File Offset: 0x0006F231
	public virtual void ApplyTraversalToPath(ref PathFinder.PotentialPath path, int from_cell)
	{
	}
}
