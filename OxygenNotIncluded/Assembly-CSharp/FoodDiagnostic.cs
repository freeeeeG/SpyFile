﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200075B RID: 1883
public class FoodDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003430 RID: 13360 RVA: 0x00117914 File Offset: 0x00115B14
	public FoodDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<KCalTracker>(worldID);
		this.icon = "icon_category_food";
		this.trackerSampleCountSeconds = 150f;
		this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
		base.AddCriterion("CheckEnoughFood", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKENOUGHFOOD, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughFood)));
		base.AddCriterion("CheckStarvation", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKSTARVATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckStarvation)));
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x001179AC File Offset: 0x00115BAC
	private ColonyDiagnostic.DiagnosticResult CheckAnyFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.PASS, null);
		if (Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count != 0)
		{
			if (this.tracker.GetDataTimeLength() < 10f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
				result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
			}
			else if (this.tracker.GetAverageValue(this.trackerSampleCountSeconds) == 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
				result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.FAIL;
			}
		}
		return result;
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x00117A44 File Offset: 0x00115C44
	private ColonyDiagnostic.DiagnosticResult CheckEnoughFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		if (this.tracker.GetDataTimeLength() < 10f)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
		}
		else
		{
			int num = 3000;
			if ((float)worldItems.Count * (1000f * (float)num) > this.tracker.GetAverageValue(this.trackerSampleCountSeconds))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				float currentValue = this.tracker.GetCurrentValue();
				float f = (float)Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count * -1000000f;
				string formattedCalories = GameUtil.GetFormattedCalories(currentValue, GameUtil.TimeSlice.None, true);
				string formattedCalories2 = GameUtil.GetFormattedCalories(Mathf.Abs(f), GameUtil.TimeSlice.None, true);
				string text = MISC.NOTIFICATIONS.FOODLOW.TOOLTIP;
				text = text.Replace("{0}", formattedCalories);
				text = text.Replace("{1}", formattedCalories2);
				result.Message = text;
			}
		}
		return result;
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x00117B50 File Offset: 0x00115D50
	private ColonyDiagnostic.DiagnosticResult CheckStarvation()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
		{
			if (!minionIdentity.IsNull())
			{
				CalorieMonitor.Instance smi = minionIdentity.GetSMI<CalorieMonitor.Instance>();
				if (!smi.IsNullOrStopped() && smi.IsInsideState(smi.sm.hungry.starving))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
					result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.HUNGRY;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
				}
			}
		}
		return result;
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x00117C28 File Offset: 0x00115E28
	public override string GetCurrentValueString()
	{
		return GameUtil.GetFormattedCalories(this.tracker.GetCurrentValue(), GameUtil.TimeSlice.None, true);
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x00117C3C File Offset: 0x00115E3C
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.NORMAL;
		}
		return diagnosticResult;
	}
}
