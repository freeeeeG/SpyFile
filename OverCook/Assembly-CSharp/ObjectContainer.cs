using System;
using UnityEngine;

// Token: 0x02000527 RID: 1319
public class ObjectContainer : MonoBehaviour, IParentable
{
	// Token: 0x060018B2 RID: 6322 RVA: 0x0007D7BE File Offset: 0x0007BBBE
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return base.transform;
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x0007D7C6 File Offset: 0x0007BBC6
	public bool HasClientSidePrediction()
	{
		return false;
	}
}
