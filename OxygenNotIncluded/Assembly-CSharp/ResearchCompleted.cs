using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000960 RID: 2400
public class ResearchCompleted : SelectModuleCondition
{
	// Token: 0x0600468A RID: 18058 RVA: 0x0018EAD0 File Offset: 0x0018CCD0
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x0018EAD4 File Offset: 0x0018CCD4
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		TechItem techItem = Db.Get().TechItems.TryGet(selectedPart.PrefabID);
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x0018EB20 File Offset: 0x0018CD20
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.FAILED;
	}
}
