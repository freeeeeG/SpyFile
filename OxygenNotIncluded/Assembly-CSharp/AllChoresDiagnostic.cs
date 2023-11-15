using System;
using STRINGS;

// Token: 0x0200074F RID: 1871
public class AllChoresDiagnostic : ColonyDiagnostic
{
	// Token: 0x060033F5 RID: 13301 RVA: 0x0011637A File Offset: 0x0011457A
	public AllChoresDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<AllChoresCountTracker>(worldID);
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.icon = "icon_errand_operate";
	}

	// Token: 0x060033F6 RID: 13302 RVA: 0x001163BC File Offset: 0x001145BC
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}
}
