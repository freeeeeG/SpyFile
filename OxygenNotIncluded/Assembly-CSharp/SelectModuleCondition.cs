using System;
using UnityEngine;

// Token: 0x0200095F RID: 2399
public abstract class SelectModuleCondition
{
	// Token: 0x06004686 RID: 18054
	public abstract bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext);

	// Token: 0x06004687 RID: 18055
	public abstract string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart);

	// Token: 0x06004688 RID: 18056 RVA: 0x0018EAC5 File Offset: 0x0018CCC5
	public virtual bool IgnoreInSanboxMode()
	{
		return false;
	}

	// Token: 0x020017C8 RID: 6088
	public enum SelectionContext
	{
		// Token: 0x04006FE5 RID: 28645
		AddModuleAbove,
		// Token: 0x04006FE6 RID: 28646
		AddModuleBelow,
		// Token: 0x04006FE7 RID: 28647
		ReplaceModule
	}
}
