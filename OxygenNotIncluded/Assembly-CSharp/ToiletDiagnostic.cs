using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000766 RID: 1894
public class ToiletDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003456 RID: 13398 RVA: 0x00118D1C File Offset: 0x00116F1C
	public ToiletDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_region_toilet";
		this.tracker = TrackerTool.Instance.GetWorldTracker<WorkingToiletTracker>(worldID);
		base.AddCriterion("CheckHasAnyToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKHASANYTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasAnyToilets)));
		base.AddCriterion("CheckEnoughToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKENOUGHTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughToilets)));
		base.AddCriterion("CheckBladders", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKBLADDERS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckBladders)));
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x00118DC8 File Offset: 0x00116FC8
	private ColonyDiagnostic.DiagnosticResult CheckHasAnyToilets()
	{
		List<IUsable> worldItems = Components.Toilets.GetWorldItems(base.worldID, false);
		List<MinionIdentity> worldItems2 = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems2.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		else if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_TOILETS;
		}
		return result;
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x00118E4C File Offset: 0x0011704C
	private ColonyDiagnostic.DiagnosticResult CheckEnoughToilets()
	{
		Components.Toilets.GetWorldItems(base.worldID, false);
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			if (this.tracker.GetDataTimeLength() > 10f && this.tracker.GetAverageValue(this.trackerSampleCountSeconds) <= 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_WORKING_TOILETS;
			}
		}
		return result;
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x00118F0C File Offset: 0x0011710C
	private ColonyDiagnostic.DiagnosticResult CheckBladders()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			foreach (MinionIdentity cmp in worldItems)
			{
				PeeChoreMonitor.Instance smi = cmp.GetSMI<PeeChoreMonitor.Instance>();
				if (smi != null && smi.IsCritical())
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
					result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.TOILET_URGENT;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x00118FE0 File Offset: 0x001171E0
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x00119014 File Offset: 0x00117214
	public override string GetAverageValueString()
	{
		List<IUsable> worldItems = Components.Toilets.GetWorldItems(base.worldID, false);
		int num = worldItems.Count;
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (!worldItems[i].IsUsable())
			{
				num--;
			}
		}
		return num.ToString() + ":" + Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count.ToString();
	}
}
