using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public class EntombedDiagnostic : ColonyDiagnostic
{
	// Token: 0x06003422 RID: 13346 RVA: 0x001171EC File Offset: 0x001153EC
	public EntombedDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_dig";
		base.AddCriterion("CheckEntombed", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.CRITERIA.CHECKENTOMBED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEntombed)));
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x0011723C File Offset: 0x0011543C
	private ColonyDiagnostic.DiagnosticResult CheckEntombed()
	{
		List<BuildingComplete> worldItems = Components.EntombedBuildings.GetWorldItems(base.worldID, false);
		this.m_entombedCount = 0;
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.NORMAL;
		foreach (BuildingComplete buildingComplete in worldItems)
		{
			if (!buildingComplete.IsNullOrDestroyed() && buildingComplete.HasTag(GameTags.Entombed))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
				result.Message = UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.BUILDING_ENTOMBED;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(buildingComplete.gameObject.transform.position, buildingComplete.gameObject);
				this.m_entombedCount++;
			}
		}
		return result;
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x00117328 File Offset: 0x00115528
	public override string GetAverageValueString()
	{
		return this.m_entombedCount.ToString();
	}

	// Token: 0x04001FA3 RID: 8099
	private int m_entombedCount;
}
