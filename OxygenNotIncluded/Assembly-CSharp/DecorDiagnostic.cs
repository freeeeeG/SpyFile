using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000756 RID: 1878
public class DecorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003418 RID: 13336 RVA: 0x001170C8 File Offset: 0x001152C8
	public DecorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_category_decor";
		base.AddCriterion("CheckDecor", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.CRITERIA.CHECKDECOR, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckDecor)));
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x00117118 File Offset: 0x00115318
	private ColonyDiagnostic.DiagnosticResult CheckDecor()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		return result;
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x00117168 File Offset: 0x00115368
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}
}
