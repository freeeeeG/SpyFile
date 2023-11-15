using System;
using UnityEngine;

// Token: 0x020008CD RID: 2253
public struct OreSizeVisualizerData
{
	// Token: 0x06004139 RID: 16697 RVA: 0x0016D580 File Offset: 0x0016B780
	public OreSizeVisualizerData(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.onMassChangedCB = null;
	}

	// Token: 0x04002A77 RID: 10871
	public PrimaryElement primaryElement;

	// Token: 0x04002A78 RID: 10872
	public Action<object> onMassChangedCB;
}
