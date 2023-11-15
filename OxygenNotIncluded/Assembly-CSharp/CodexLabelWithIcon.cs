using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ACE RID: 2766
public class CodexLabelWithIcon : CodexWidget<CodexLabelWithIcon>
{
	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06005529 RID: 21801 RVA: 0x001EF436 File Offset: 0x001ED636
	// (set) Token: 0x0600552A RID: 21802 RVA: 0x001EF43E File Offset: 0x001ED63E
	public CodexImage icon { get; set; }

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x0600552B RID: 21803 RVA: 0x001EF447 File Offset: 0x001ED647
	// (set) Token: 0x0600552C RID: 21804 RVA: 0x001EF44F File Offset: 0x001ED64F
	public CodexText label { get; set; }

	// Token: 0x0600552D RID: 21805 RVA: 0x001EF458 File Offset: 0x001ED658
	public CodexLabelWithIcon()
	{
	}

	// Token: 0x0600552E RID: 21806 RVA: 0x001EF460 File Offset: 0x001ED660
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x0600552F RID: 21807 RVA: 0x001EF482 File Offset: 0x001ED682
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06005530 RID: 21808 RVA: 0x001EF4A8 File Offset: 0x001ED6A8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.icon.ConfigureImage(contentGameObject.GetComponentInChildren<Image>());
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentInChildren<Image>().GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
	}
}
