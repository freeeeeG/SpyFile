using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000752 RID: 1874
public class BedDiagnostic : ColonyDiagnostic
{
	// Token: 0x060033FD RID: 13309 RVA: 0x00116900 File Offset: 0x00114B00
	public BedDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_region_bedroom";
		base.AddCriterion("CheckEnoughBeds", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CRITERIA.CHECKENOUGHBEDS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughBeds)));
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x00116950 File Offset: 0x00114B50
	private ColonyDiagnostic.DiagnosticResult CheckEnoughBeds()
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
			result.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NORMAL;
			int num = 0;
			List<Sleepable> worldItems2 = Components.Sleepables.GetWorldItems(base.worldID, false);
			for (int i = 0; i < worldItems2.Count; i++)
			{
				if (worldItems2[i].GetComponent<Assignable>() != null && worldItems2[i].GetComponent<Clinic>() == null)
				{
					num++;
				}
			}
			if (num < worldItems.Count)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NOT_ENOUGH_BEDS;
			}
		}
		return result;
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x00116A38 File Offset: 0x00114C38
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x00116A6C File Offset: 0x00114C6C
	public override string GetAverageValueString()
	{
		return Components.Sleepables.GetWorldItems(base.worldID, false).FindAll((Sleepable match) => match.GetComponent<Assignable>() != null).Count.ToString() + "/" + Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count.ToString();
	}
}
