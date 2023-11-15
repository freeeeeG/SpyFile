using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ACC RID: 2764
public class CodexLargeSpacer : CodexWidget<CodexLargeSpacer>
{
	// Token: 0x06005525 RID: 21797 RVA: 0x001EF41B File Offset: 0x001ED61B
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
