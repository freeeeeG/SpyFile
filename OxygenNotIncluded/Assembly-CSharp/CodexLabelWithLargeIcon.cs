using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ACF RID: 2767
public class CodexLabelWithLargeIcon : CodexLabelWithIcon
{
	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06005531 RID: 21809 RVA: 0x001EF546 File Offset: 0x001ED746
	// (set) Token: 0x06005532 RID: 21810 RVA: 0x001EF54E File Offset: 0x001ED74E
	public string linkID { get; set; }

	// Token: 0x06005533 RID: 21811 RVA: 0x001EF557 File Offset: 0x001ED757
	public CodexLabelWithLargeIcon()
	{
	}

	// Token: 0x06005534 RID: 21812 RVA: 0x001EF560 File Offset: 0x001ED760
	public CodexLabelWithLargeIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, string targetEntrylinkID) : base(text, style, coloredSprite, 128, 128)
	{
		base.icon = new CodexImage(128, 128, coloredSprite);
		base.label = new CodexText(text, style, null);
		this.linkID = targetEntrylinkID;
	}

	// Token: 0x06005535 RID: 21813 RVA: 0x001EF5AC File Offset: 0x001ED7AC
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.icon.ConfigureImage(contentGameObject.GetComponentsInChildren<Image>()[1]);
		if (base.icon.preferredWidth != -1 && base.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentsInChildren<Image>()[1].GetComponent<LayoutElement>();
			component.minWidth = (float)base.icon.preferredHeight;
			component.minHeight = (float)base.icon.preferredWidth;
			component.preferredHeight = (float)base.icon.preferredHeight;
			component.preferredWidth = (float)base.icon.preferredWidth;
		}
		base.label.text = UI.StripLinkFormatting(base.label.text);
		base.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		contentGameObject.GetComponent<KButton>().ClearOnClick();
		contentGameObject.GetComponent<KButton>().onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(this.linkID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
	}
}
