using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A8D RID: 2701
public class DetailsPanelDrawer
{
	// Token: 0x06005284 RID: 21124 RVA: 0x001D886E File Offset: 0x001D6A6E
	public DetailsPanelDrawer(GameObject label_prefab, GameObject parent)
	{
		this.parent = parent;
		this.labelPrefab = label_prefab;
		this.stringformatter = new UIStringFormatter();
		this.floatFormatter = new UIFloatFormatter();
	}

	// Token: 0x06005285 RID: 21125 RVA: 0x001D88A8 File Offset: 0x001D6AA8
	public DetailsPanelDrawer NewLabel(string text)
	{
		DetailsPanelDrawer.Label label = default(DetailsPanelDrawer.Label);
		if (this.activeLabelCount >= this.labels.Count)
		{
			label.text = Util.KInstantiate(this.labelPrefab, this.parent, null).GetComponent<LocText>();
			label.tooltip = label.text.GetComponent<ToolTip>();
			label.text.transform.localScale = new Vector3(1f, 1f, 1f);
			this.labels.Add(label);
		}
		else
		{
			label = this.labels[this.activeLabelCount];
		}
		this.activeLabelCount++;
		label.text.text = text;
		label.tooltip.toolTip = "";
		label.tooltip.OnToolTip = null;
		label.text.gameObject.SetActive(true);
		return this;
	}

	// Token: 0x06005286 RID: 21126 RVA: 0x001D898C File Offset: 0x001D6B8C
	public DetailsPanelDrawer Tooltip(string tooltip_text)
	{
		this.labels[this.activeLabelCount - 1].tooltip.toolTip = tooltip_text;
		return this;
	}

	// Token: 0x06005287 RID: 21127 RVA: 0x001D89AD File Offset: 0x001D6BAD
	public DetailsPanelDrawer Tooltip(Func<string> tooltip_cb)
	{
		this.labels[this.activeLabelCount - 1].tooltip.OnToolTip = tooltip_cb;
		return this;
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x001D89CE File Offset: 0x001D6BCE
	public string Format(string format, float value)
	{
		return this.floatFormatter.Format(format, value);
	}

	// Token: 0x06005289 RID: 21129 RVA: 0x001D89DD File Offset: 0x001D6BDD
	public string Format(string format, string s0)
	{
		return this.stringformatter.Format(format, s0);
	}

	// Token: 0x0600528A RID: 21130 RVA: 0x001D89EC File Offset: 0x001D6BEC
	public string Format(string format, string s0, string s1)
	{
		return this.stringformatter.Format(format, s0, s1);
	}

	// Token: 0x0600528B RID: 21131 RVA: 0x001D89FC File Offset: 0x001D6BFC
	public DetailsPanelDrawer BeginDrawing()
	{
		this.activeLabelCount = 0;
		this.stringformatter.BeginDrawing();
		this.floatFormatter.BeginDrawing();
		return this;
	}

	// Token: 0x0600528C RID: 21132 RVA: 0x001D8A1C File Offset: 0x001D6C1C
	public DetailsPanelDrawer EndDrawing()
	{
		this.floatFormatter.EndDrawing();
		this.stringformatter.EndDrawing();
		for (int i = this.activeLabelCount; i < this.labels.Count; i++)
		{
			if (this.labels[i].text.gameObject.activeSelf)
			{
				this.labels[i].text.gameObject.SetActive(false);
			}
		}
		return this;
	}

	// Token: 0x04003720 RID: 14112
	private List<DetailsPanelDrawer.Label> labels = new List<DetailsPanelDrawer.Label>();

	// Token: 0x04003721 RID: 14113
	private int activeLabelCount;

	// Token: 0x04003722 RID: 14114
	private UIStringFormatter stringformatter;

	// Token: 0x04003723 RID: 14115
	private UIFloatFormatter floatFormatter;

	// Token: 0x04003724 RID: 14116
	private GameObject parent;

	// Token: 0x04003725 RID: 14117
	private GameObject labelPrefab;

	// Token: 0x020019A6 RID: 6566
	private struct Label
	{
		// Token: 0x040076F2 RID: 30450
		public LocText text;

		// Token: 0x040076F3 RID: 30451
		public ToolTip tooltip;
	}
}
