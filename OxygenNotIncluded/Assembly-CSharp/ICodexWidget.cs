using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
public interface ICodexWidget
{
	// Token: 0x060054DE RID: 21726
	void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);
}
