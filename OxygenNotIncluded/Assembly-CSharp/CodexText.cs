using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC6 RID: 2758
public class CodexText : CodexWidget<CodexText>
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x060054F1 RID: 21745 RVA: 0x001EEF69 File Offset: 0x001ED169
	// (set) Token: 0x060054F2 RID: 21746 RVA: 0x001EEF71 File Offset: 0x001ED171
	public string text { get; set; }

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x060054F3 RID: 21747 RVA: 0x001EEF7A File Offset: 0x001ED17A
	// (set) Token: 0x060054F4 RID: 21748 RVA: 0x001EEF82 File Offset: 0x001ED182
	public string messageID { get; set; }

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x060054F5 RID: 21749 RVA: 0x001EEF8B File Offset: 0x001ED18B
	// (set) Token: 0x060054F6 RID: 21750 RVA: 0x001EEF93 File Offset: 0x001ED193
	public CodexTextStyle style { get; set; }

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x060054F8 RID: 21752 RVA: 0x001EEFAF File Offset: 0x001ED1AF
	// (set) Token: 0x060054F7 RID: 21751 RVA: 0x001EEF9C File Offset: 0x001ED19C
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

	// Token: 0x060054F9 RID: 21753 RVA: 0x001EEFCA File Offset: 0x001ED1CA
	public CodexText()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x060054FA RID: 21754 RVA: 0x001EEFD9 File Offset: 0x001ED1D9
	public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body, string id = null)
	{
		this.text = text;
		this.style = style;
		if (id != null)
		{
			this.messageID = id;
		}
	}

	// Token: 0x060054FB RID: 21755 RVA: 0x001EEFFC File Offset: 0x001ED1FC
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x060054FC RID: 21756 RVA: 0x001EF048 File Offset: 0x001ED248
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
