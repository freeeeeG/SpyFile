﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x0200075E RID: 1886
public class MeteorDiagnostic : ColonyDiagnostic
{
	// Token: 0x0600343A RID: 13370 RVA: 0x00117E88 File Offset: 0x00116088
	public MeteorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "meteors";
		base.AddCriterion("BombardmentUnderway", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.CRITERIA.CHECKUNDERWAY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckMeteorBombardment)));
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x00117ED7 File Offset: 0x001160D7
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x00117EE0 File Offset: 0x001160E0
	public ColonyDiagnostic.DiagnosticResult CheckMeteorBombardment()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.NORMAL, null);
		List<GameplayEventInstance> list = new List<GameplayEventInstance>();
		GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(base.worldID, ref list);
		for (int i = 0; i < list.Count; i++)
		{
			MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
			if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
				result.Message = string.Format(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.SHOWER_UNDERWAY, GameUtil.GetFormattedTime(statesInstance.BombardTimeRemaining(), "F0"));
			}
		}
		return result;
	}
}
