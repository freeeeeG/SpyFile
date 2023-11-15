using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC7 RID: 2759
public class CodexTextWithTooltip : CodexWidget<CodexTextWithTooltip>
{
	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x060054FD RID: 21757 RVA: 0x001EF05E File Offset: 0x001ED25E
	// (set) Token: 0x060054FE RID: 21758 RVA: 0x001EF066 File Offset: 0x001ED266
	public string text { get; set; }

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x060054FF RID: 21759 RVA: 0x001EF06F File Offset: 0x001ED26F
	// (set) Token: 0x06005500 RID: 21760 RVA: 0x001EF077 File Offset: 0x001ED277
	public string tooltip { get; set; }

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06005501 RID: 21761 RVA: 0x001EF080 File Offset: 0x001ED280
	// (set) Token: 0x06005502 RID: 21762 RVA: 0x001EF088 File Offset: 0x001ED288
	public CodexTextStyle style { get; set; }

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06005504 RID: 21764 RVA: 0x001EF0A4 File Offset: 0x001ED2A4
	// (set) Token: 0x06005503 RID: 21763 RVA: 0x001EF091 File Offset: 0x001ED291
	public string stringKey
	{
		get
		{
			return "--> " + (this.text ?? "NULL");
		}
		set
		{
			this.text = Strings.Get(value);
		}
	}

	// Token: 0x06005505 RID: 21765 RVA: 0x001EF0BF File Offset: 0x001ED2BF
	public CodexTextWithTooltip()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x06005506 RID: 21766 RVA: 0x001EF0CE File Offset: 0x001ED2CE
	public CodexTextWithTooltip(string text, string tooltip, CodexTextStyle style = CodexTextStyle.Body)
	{
		this.text = text;
		this.style = style;
		this.tooltip = tooltip;
	}

	// Token: 0x06005507 RID: 21767 RVA: 0x001EF0EC File Offset: 0x001ED2EC
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x06005508 RID: 21768 RVA: 0x001EF138 File Offset: 0x001ED338
	public void ConfigureTooltip(ToolTip tooltip)
	{
		tooltip.SetSimpleTooltip(this.tooltip);
	}

	// Token: 0x06005509 RID: 21769 RVA: 0x001EF146 File Offset: 0x001ED346
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		this.ConfigureTooltip(contentGameObject.GetComponent<ToolTip>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
