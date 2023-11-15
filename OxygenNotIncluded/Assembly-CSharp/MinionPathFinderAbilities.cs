using System;
using System.Diagnostics;

// Token: 0x0200039F RID: 927
public class MinionPathFinderAbilities : PathFinderAbilities
{
	// Token: 0x06001361 RID: 4961 RVA: 0x00065A48 File Offset: 0x00063C48
	public MinionPathFinderAbilities(Navigator navigator) : base(navigator)
	{
		this.transitionVoidOffsets = new CellOffset[navigator.NavGrid.transitions.Length][];
		for (int i = 0; i < this.transitionVoidOffsets.Length; i++)
		{
			this.transitionVoidOffsets[i] = navigator.NavGrid.transitions[i].voidOffsets;
		}
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x00065AA5 File Offset: 0x00063CA5
	protected override void Refresh(Navigator navigator)
	{
		this.proxyID = navigator.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<KPrefabID>().InstanceID;
		this.out_of_fuel = navigator.HasTag(GameTags.JetSuitOutOfFuel);
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x00065AD8 File Offset: 0x00063CD8
	public void SetIdleNavMaskEnabled(bool enabled)
	{
		this.idleNavMaskEnabled = enabled;
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x00065AE1 File Offset: 0x00063CE1
	private static bool IsAccessPermitted(int proxyID, int cell, int from_cell, NavType from_nav_type)
	{
		return Grid.HasPermission(cell, proxyID, from_cell, from_nav_type);
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x00065AEC File Offset: 0x00063CEC
	public override int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		if (!path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasLeadSuit))
		{
			return (int)(link.cost * 2);
		}
		return 0;
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x00065B04 File Offset: 0x00063D04
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, path.cell, from_cell, from_nav_type))
		{
			return false;
		}
		foreach (CellOffset offset in this.transitionVoidOffsets[transition_id])
		{
			int cell = Grid.OffsetCell(from_cell, offset);
			if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, cell, from_cell, from_nav_type))
			{
				return false;
			}
		}
		if (path.navType == NavType.Tube && from_nav_type == NavType.Floor && !Grid.HasUsableTubeEntrance(from_cell, this.prefabInstanceID))
		{
			return false;
		}
		if (path.navType == NavType.Hover && (this.out_of_fuel || !path.HasFlag(PathFinder.PotentialPath.Flags.HasJetPack)))
		{
			return false;
		}
		Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
		PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
		bool flag = path.HasFlag(PathFinder.PotentialPath.Flags.PerformSuitChecks) && Grid.TryGetSuitMarkerFlags(from_cell, out flags, out flags2) && (flags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		bool flag2 = SuitMarker.DoesTraversalDirectionRequireSuit(from_cell, path.cell, flags);
		bool flag3 = path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
		if (flag)
		{
			bool flag4 = path.HasFlag(flags2);
			if (flag2)
			{
				if (!flag3 && !Grid.HasSuit(from_cell, this.prefabInstanceID))
				{
					return false;
				}
			}
			else if (flag3 && (flags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != (Grid.SuitMarker.Flags)0 && (!flag4 || !Grid.HasEmptyLocker(from_cell, this.prefabInstanceID)))
			{
				return false;
			}
		}
		if (this.idleNavMaskEnabled && (Grid.PreventIdleTraversal[path.cell] || Grid.PreventIdleTraversal[from_cell]))
		{
			return false;
		}
		if (flag)
		{
			if (flag2)
			{
				if (!flag3)
				{
					path.SetFlags(flags2);
				}
			}
			else
			{
				path.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
			}
		}
		return true;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x00065C66 File Offset: 0x00063E66
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void BeginSample(string region_name)
	{
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x00065C68 File Offset: 0x00063E68
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void EndSample(string region_name)
	{
	}

	// Token: 0x04000A63 RID: 2659
	private CellOffset[][] transitionVoidOffsets;

	// Token: 0x04000A64 RID: 2660
	private int proxyID;

	// Token: 0x04000A65 RID: 2661
	private bool out_of_fuel;

	// Token: 0x04000A66 RID: 2662
	private bool idleNavMaskEnabled;
}
