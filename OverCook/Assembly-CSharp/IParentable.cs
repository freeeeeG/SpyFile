using System;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public interface IParentable
{
	// Token: 0x0600170A RID: 5898
	Transform GetAttachPoint(GameObject gameObject);

	// Token: 0x0600170B RID: 5899
	bool HasClientSidePrediction();
}
