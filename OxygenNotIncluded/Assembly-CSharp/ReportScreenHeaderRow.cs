using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BD1 RID: 3025
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeaderRow")]
public class ReportScreenHeaderRow : KMonoBehaviour
{
	// Token: 0x06005F14 RID: 24340 RVA: 0x0022E8C0 File Offset: 0x0022CAC0
	public void SetLine(ReportManager.ReportGroup reportGroup)
	{
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		component.minWidth = (component.preferredWidth = this.nameWidth);
		this.spacer.minWidth = this.groupSpacerWidth;
		this.name.text = reportGroup.stringKey;
	}

	// Token: 0x0400404A RID: 16458
	[SerializeField]
	public new LocText name;

	// Token: 0x0400404B RID: 16459
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x0400404C RID: 16460
	[SerializeField]
	private Image bgImage;

	// Token: 0x0400404D RID: 16461
	public float groupSpacerWidth;

	// Token: 0x0400404E RID: 16462
	private float nameWidth = 164f;

	// Token: 0x0400404F RID: 16463
	[SerializeField]
	private Color oddRowColor;
}
