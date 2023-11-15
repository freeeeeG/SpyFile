using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AC9 RID: 2761
public class CodexDividerLine : CodexWidget<CodexDividerLine>
{
	// Token: 0x06005519 RID: 21785 RVA: 0x001EF2E8 File Offset: 0x001ED4E8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		contentGameObject.GetComponent<LayoutElement>().minWidth = 530f;
	}
}
