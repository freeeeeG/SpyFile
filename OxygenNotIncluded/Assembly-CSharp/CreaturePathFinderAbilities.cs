using System;
using Klei.AI;

// Token: 0x0200039E RID: 926
public class CreaturePathFinderAbilities : PathFinderAbilities
{
	// Token: 0x0600135E RID: 4958 RVA: 0x000659EB File Offset: 0x00063BEB
	public CreaturePathFinderAbilities(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x000659F4 File Offset: 0x00063BF4
	protected override void Refresh(Navigator navigator)
	{
		if (PathFinder.IsSubmerged(Grid.PosToCell(navigator)))
		{
			this.canTraverseSubmered = true;
			return;
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.MaxUnderwaterTravelCost.Lookup(navigator);
		this.canTraverseSubmered = (attributeInstance == null);
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x00065A36 File Offset: 0x00063C36
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		return !submerged || this.canTraverseSubmered;
	}

	// Token: 0x04000A62 RID: 2658
	public bool canTraverseSubmered;
}
