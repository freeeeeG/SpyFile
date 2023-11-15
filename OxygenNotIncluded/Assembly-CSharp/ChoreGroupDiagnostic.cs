using System;
using STRINGS;

// Token: 0x02000754 RID: 1876
public class ChoreGroupDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003405 RID: 13317 RVA: 0x00116CD4 File Offset: 0x00114ED4
	public ChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.CHOREGROUPDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetChoreGroupTracker(worldID, choreGroup);
		this.name = choreGroup.Name;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.id = "ChoreGroupDiagnostic_" + choreGroup.Id;
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x00116D40 File Offset: 0x00114F40
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetCurrentValue() > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}

	// Token: 0x04001F94 RID: 8084
	public ChoreGroup choreGroup;
}
