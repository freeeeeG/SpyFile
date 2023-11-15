using System;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public struct ElementSplitter
{
	// Token: 0x06001B38 RID: 6968 RVA: 0x00092264 File Offset: 0x00090464
	public ElementSplitter(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.kPrefabID = go.GetComponent<KPrefabID>();
		this.onTakeCB = null;
		this.canAbsorbCB = null;
	}

	// Token: 0x04000F24 RID: 3876
	public PrimaryElement primaryElement;

	// Token: 0x04000F25 RID: 3877
	public Func<float, Pickupable> onTakeCB;

	// Token: 0x04000F26 RID: 3878
	public Func<Pickupable, bool> canAbsorbCB;

	// Token: 0x04000F27 RID: 3879
	public KPrefabID kPrefabID;
}
