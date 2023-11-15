﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000765 RID: 1893
public class StressDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003454 RID: 13396 RVA: 0x00118BA8 File Offset: 0x00116DA8
	public StressDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.STRESSDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<StressTracker>(worldID);
		this.icon = "mod_stress";
		base.AddCriterion("CheckStressed", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.STRESSDIAGNOSTIC.CRITERIA.CHECKSTRESSED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckStressed)));
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x00118C08 File Offset: 0x00116E08
	private ColonyDiagnostic.DiagnosticResult CheckStressed()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			TrackerTool.Instance.IsRocketInterior(base.worldID) ? UI.COLONY_DIAGNOSTICS.ROCKET : UI.CLUSTERMAP.PLANETOID_KEYWORD;
			result.Message = base.NO_MINIONS;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.STRESSDIAGNOSTIC.NORMAL;
			if (this.tracker.GetAverageValue(this.trackerSampleCountSeconds) >= STRESS.ACTING_OUT_RESET)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.STRESSDIAGNOSTIC.HIGH_STRESS;
				MinionIdentity minionIdentity = worldItems.Find((MinionIdentity match) => match.gameObject.GetAmounts().GetValue(Db.Get().Amounts.Stress.Id) >= STRESS.ACTING_OUT_RESET);
				if (minionIdentity != null)
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.gameObject.transform.position, minionIdentity.gameObject);
				}
			}
		}
		return result;
	}
}
