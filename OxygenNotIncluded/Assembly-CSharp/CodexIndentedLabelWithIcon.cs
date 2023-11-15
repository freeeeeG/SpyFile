using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ACB RID: 2763
public class CodexIndentedLabelWithIcon : CodexWidget<CodexIndentedLabelWithIcon>
{
	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x0600551D RID: 21789 RVA: 0x001EF30C File Offset: 0x001ED50C
	// (set) Token: 0x0600551E RID: 21790 RVA: 0x001EF314 File Offset: 0x001ED514
	public CodexImage icon { get; set; }

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x0600551F RID: 21791 RVA: 0x001EF31D File Offset: 0x001ED51D
	// (set) Token: 0x06005520 RID: 21792 RVA: 0x001EF325 File Offset: 0x001ED525
	public CodexText label { get; set; }

	// Token: 0x06005521 RID: 21793 RVA: 0x001EF32E File Offset: 0x001ED52E
	public CodexIndentedLabelWithIcon()
	{
	}

	// Token: 0x06005522 RID: 21794 RVA: 0x001EF336 File Offset: 0x001ED536
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06005523 RID: 21795 RVA: 0x001EF358 File Offset: 0x001ED558
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06005524 RID: 21796 RVA: 0x001EF380 File Offset: 0x001ED580
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		Image componentInChildren = contentGameObject.GetComponentInChildren<Image>();
		this.icon.ConfigureImage(componentInChildren);
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
	}
}
