using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000965 RID: 2405
public class TopOnly : SelectModuleCondition
{
	// Token: 0x0600469B RID: 18075 RVA: 0x0018EE44 File Offset: 0x0018D044
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		global::Debug.Assert(existingModule != null, "Existing module is null in top only condition");
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			global::Debug.Assert(existingModule.GetComponent<LaunchPad>() == null, "Trying to replace launch pad with rocket module");
			return existingModule.GetComponent<BuildingAttachPoint>() == null || existingModule.GetComponent<BuildingAttachPoint>().points[0].attachedBuilding == null;
		}
		return existingModule.GetComponent<LaunchPad>() != null || (existingModule.GetComponent<BuildingAttachPoint>() != null && existingModule.GetComponent<BuildingAttachPoint>().points[0].attachedBuilding == null);
	}

	// Token: 0x0600469C RID: 18076 RVA: 0x0018EEE5 File Offset: 0x0018D0E5
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.TOP_ONLY.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.TOP_ONLY.FAILED;
	}
}
