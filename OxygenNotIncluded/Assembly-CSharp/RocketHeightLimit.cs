using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000967 RID: 2407
public class RocketHeightLimit : SelectModuleCondition
{
	// Token: 0x060046A1 RID: 18081 RVA: 0x0018F0F0 File Offset: 0x0018D2F0
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		int num = selectedPart.HeightInCells;
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			num -= existingModule.GetComponent<Building>().Def.HeightInCells;
		}
		if (existingModule == null)
		{
			return true;
		}
		RocketModuleCluster component = existingModule.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return true;
		}
		int num2 = component.CraftInterface.MaxHeight;
		if (num2 <= 0)
		{
			num2 = ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT;
		}
		RocketEngineCluster component2 = existingModule.GetComponent<RocketEngineCluster>();
		RocketEngineCluster component3 = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule && component2 != null)
		{
			if (component3 != null)
			{
				num2 = component3.maxHeight;
			}
			else
			{
				num2 = ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT;
			}
		}
		if (component3 != null && selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow)
		{
			num2 = component3.maxHeight;
		}
		return num2 == -1 || component.CraftInterface.RocketHeight + num <= num2;
	}

	// Token: 0x060046A2 RID: 18082 RVA: 0x0018F1B8 File Offset: 0x0018D3B8
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		UnityEngine.Object engine = moduleBase.GetComponent<RocketModuleCluster>().CraftInterface.GetEngine();
		RocketEngineCluster component = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
		bool flag = engine != null || component != null;
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.COMPLETE;
		}
		if (flag)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED_NO_ENGINE;
	}
}
