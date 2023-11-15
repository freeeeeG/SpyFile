using System;
using UnityEngine;

// Token: 0x02000C60 RID: 3168
public class SimpleInfoPanel
{
	// Token: 0x060064C9 RID: 25801 RVA: 0x00255869 File Offset: 0x00253A69
	public SimpleInfoPanel(SimpleInfoScreen simpleInfoRoot)
	{
		this.simpleInfoRoot = simpleInfoRoot;
	}

	// Token: 0x060064CA RID: 25802 RVA: 0x00255878 File Offset: 0x00253A78
	public virtual void Refresh(CollapsibleDetailContentPanel panel, GameObject selectedTarget)
	{
	}

	// Token: 0x040044EC RID: 17644
	protected SimpleInfoScreen simpleInfoRoot;
}
