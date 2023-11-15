using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200075D RID: 1885
public class IdleDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003438 RID: 13368 RVA: 0x00117D18 File Offset: 0x00115F18
	public IdleDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<IdleTracker>(worldID);
		this.icon = "icon_errand_operate";
		base.AddCriterion("CheckIdle", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.CRITERIA.CHECKIDLE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckIdle)));
	}

	// Token: 0x06003439 RID: 13369 RVA: 0x00117D78 File Offset: 0x00115F78
	private ColonyDiagnostic.DiagnosticResult CheckIdle()
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
			result.Message = UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.NORMAL;
			if (this.tracker.GetMinValue(30f) > 0f && this.tracker.GetCurrentValue() > 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.IDLE;
				MinionIdentity minionIdentity = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Find((MinionIdentity match) => match.HasTag(GameTags.Idle));
				if (minionIdentity != null)
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.transform.position, minionIdentity.gameObject);
				}
			}
		}
		return result;
	}
}
