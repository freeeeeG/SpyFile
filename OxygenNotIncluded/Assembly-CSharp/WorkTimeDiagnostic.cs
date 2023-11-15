using System;
using STRINGS;

// Token: 0x02000768 RID: 1896
public class WorkTimeDiagnostic : ColonyDiagnostic
{
	// Token: 0x0600345F RID: 13407 RVA: 0x001193D8 File Offset: 0x001175D8
	public WorkTimeDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.WORKTIMEDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetWorkTimeTracker(worldID, choreGroup);
		this.trackerSampleCountSeconds = 100f;
		this.name = choreGroup.Name;
		this.id = "WorkTimeDiagnostic_" + choreGroup.Id;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x00119450 File Offset: 0x00117650
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetAverageValue(this.trackerSampleCountSeconds) > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)))
		};
	}

	// Token: 0x04001FA6 RID: 8102
	public ChoreGroup choreGroup;
}
