using System;
using UnityEngine;

// Token: 0x02000C48 RID: 3144
public class SideScreen : KScreen
{
	// Token: 0x060063B4 RID: 25524 RVA: 0x0024E46A File Offset: 0x0024C66A
	public void SetContent(SideScreenContent sideScreenContent, GameObject target)
	{
		if (sideScreenContent.transform.parent != this.contentBody.transform)
		{
			sideScreenContent.transform.SetParent(this.contentBody.transform);
		}
		sideScreenContent.SetTarget(target);
	}

	// Token: 0x04004415 RID: 17429
	[SerializeField]
	private GameObject contentBody;
}
