using System;
using STRINGS;

// Token: 0x0200075C RID: 1884
public class HeatDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003436 RID: 13366 RVA: 0x00117C7C File Offset: 0x00115E7C
	public HeatDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BatteryTracker>(worldID);
		this.trackerSampleCountSeconds = 4f;
		base.AddCriterion("CheckHeat", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.CRITERIA.CHECKHEAT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHeat)));
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x00117CDC File Offset: 0x00115EDC
	private ColonyDiagnostic.DiagnosticResult CheckHeat()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NORMAL
		};
	}
}
