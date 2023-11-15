using System;
using UnityEngine;

// Token: 0x02000B21 RID: 2849
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenLineItem")]
public class InfoScreenLineItem : KMonoBehaviour
{
	// Token: 0x060057B6 RID: 22454 RVA: 0x002013AD File Offset: 0x001FF5AD
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x060057B7 RID: 22455 RVA: 0x002013BB File Offset: 0x001FF5BB
	public void SetTooltip(string tooltip)
	{
		this.toolTip.toolTip = tooltip;
	}

	// Token: 0x04003B4A RID: 15178
	[SerializeField]
	private LocText locText;

	// Token: 0x04003B4B RID: 15179
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04003B4C RID: 15180
	private string text;

	// Token: 0x04003B4D RID: 15181
	private string tooltip;
}
