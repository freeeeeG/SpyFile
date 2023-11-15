using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000961 RID: 2401
public class MaterialsAvailable : SelectModuleCondition
{
	// Token: 0x0600468E RID: 18062 RVA: 0x0018EB42 File Offset: 0x0018CD42
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x0018EB45 File Offset: 0x0018CD45
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		return existingModule == null || ProductInfoScreen.MaterialsMet(selectedPart.CraftRecipe);
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x0018EB60 File Offset: 0x0018CD60
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.COMPLETE;
		}
		string text = UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.FAILED;
		foreach (Recipe.Ingredient ingredient in selectedPart.CraftRecipe.Ingredients)
		{
			string str = "\n" + string.Format("{0}{1}: {2}", "    • ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			text += str;
		}
		return text;
	}
}
