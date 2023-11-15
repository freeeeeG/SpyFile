using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AD5 RID: 2773
public class CodexCollapsibleHeader : CodexWidget<CodexCollapsibleHeader>
{
	// Token: 0x06005553 RID: 21843 RVA: 0x001F0F43 File Offset: 0x001EF143
	public CodexCollapsibleHeader(string label, ContentContainer contents)
	{
		this.label = label;
		this.contents = contents;
	}

	// Token: 0x06005554 RID: 21844 RVA: 0x001F0F5C File Offset: 0x001EF15C
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("Label");
		reference.text = this.label;
		reference.textStyleSetting = textStyles[CodexTextStyle.Subtitle];
		reference.ApplySettings();
		MultiToggle reference2 = component.GetReference<MultiToggle>("ExpandToggle");
		reference2.ChangeState(1);
		reference2.onClick = delegate()
		{
			this.ToggleCategoryOpen(contentGameObject, !this.contents.go.activeSelf);
		};
	}

	// Token: 0x06005555 RID: 21845 RVA: 0x001F0FD3 File Offset: 0x001EF1D3
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		this.contents.go.SetActive(open);
	}

	// Token: 0x040038F1 RID: 14577
	private ContentContainer contents;

	// Token: 0x040038F2 RID: 14578
	private string label;
}
