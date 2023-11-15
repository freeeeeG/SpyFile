using System;

// Token: 0x020003F9 RID: 1017
public abstract class PathFinderAbilities
{
	// Token: 0x0600157B RID: 5499 RVA: 0x00071BBB File Offset: 0x0006FDBB
	public PathFinderAbilities(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x00071BCA File Offset: 0x0006FDCA
	public void Refresh()
	{
		this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
		this.Refresh(this.navigator);
	}

	// Token: 0x0600157D RID: 5501
	protected abstract void Refresh(Navigator navigator);

	// Token: 0x0600157E RID: 5502
	public abstract bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged);

	// Token: 0x0600157F RID: 5503 RVA: 0x00071BF3 File Offset: 0x0006FDF3
	public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		return 0;
	}

	// Token: 0x04000BD6 RID: 3030
	protected Navigator navigator;

	// Token: 0x04000BD7 RID: 3031
	protected int prefabInstanceID;
}
