using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000968 RID: 2408
public class NoFreeRocketInterior : SelectModuleCondition
{
	// Token: 0x060046A4 RID: 18084 RVA: 0x0018F224 File Offset: 0x0018D424
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		int num = 0;
		using (IEnumerator<WorldContainer> enumerator = ClusterManager.Instance.WorldContainers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsModuleInterior)
				{
					num++;
				}
			}
		}
		return num < ClusterManager.MAX_ROCKET_INTERIOR_COUNT;
	}

	// Token: 0x060046A5 RID: 18085 RVA: 0x0018F284 File Offset: 0x0018D484
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.FAILED;
	}
}
