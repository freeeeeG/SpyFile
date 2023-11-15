using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AC4 RID: 2756
public abstract class CodexWidget<SubClass> : ICodexWidget
{
	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x060054DF RID: 21727 RVA: 0x001EEE8D File Offset: 0x001ED08D
	// (set) Token: 0x060054E0 RID: 21728 RVA: 0x001EEE95 File Offset: 0x001ED095
	public int preferredWidth { get; set; }

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x060054E1 RID: 21729 RVA: 0x001EEE9E File Offset: 0x001ED09E
	// (set) Token: 0x060054E2 RID: 21730 RVA: 0x001EEEA6 File Offset: 0x001ED0A6
	public int preferredHeight { get; set; }

	// Token: 0x060054E3 RID: 21731 RVA: 0x001EEEAF File Offset: 0x001ED0AF
	protected CodexWidget()
	{
		this.preferredWidth = -1;
		this.preferredHeight = -1;
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x001EEEC5 File Offset: 0x001ED0C5
	protected CodexWidget(int preferredWidth, int preferredHeight)
	{
		this.preferredWidth = preferredWidth;
		this.preferredHeight = preferredHeight;
	}

	// Token: 0x060054E5 RID: 21733
	public abstract void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

	// Token: 0x060054E6 RID: 21734 RVA: 0x001EEEDB File Offset: 0x001ED0DB
	protected void ConfigurePreferredLayout(GameObject contentGameObject)
	{
		LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
		componentInChildren.preferredHeight = (float)this.preferredHeight;
		componentInChildren.preferredWidth = (float)this.preferredWidth;
	}
}
