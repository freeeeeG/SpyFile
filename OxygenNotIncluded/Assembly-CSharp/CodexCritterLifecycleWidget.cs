using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AD7 RID: 2775
public class CodexCritterLifecycleWidget : CodexWidget<CodexCritterLifecycleWidget>
{
	// Token: 0x06005561 RID: 21857 RVA: 0x001F1099 File Offset: 0x001EF299
	private CodexCritterLifecycleWidget()
	{
	}

	// Token: 0x06005562 RID: 21858 RVA: 0x001F10A4 File Offset: 0x001EF2A4
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("EggIcon").sprite = null;
		component.GetReference<Image>("EggIcon").color = Color.white;
		component.GetReference<LocText>("EggToBabyLabel").text = "";
		component.GetReference<Image>("BabyIcon").sprite = null;
		component.GetReference<Image>("BabyIcon").color = Color.white;
		component.GetReference<LocText>("BabyToAdultLabel").text = "";
		component.GetReference<Image>("AdultIcon").sprite = null;
		component.GetReference<Image>("AdultIcon").color = Color.white;
	}
}
