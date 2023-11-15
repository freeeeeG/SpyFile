using System;
using UnityEngine;

// Token: 0x02000C43 RID: 3139
public class RoleStationSideScreen : SideScreenContent
{
	// Token: 0x06006373 RID: 25459 RVA: 0x0024C065 File Offset: 0x0024A265
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006374 RID: 25460 RVA: 0x0024C06D File Offset: 0x0024A26D
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x040043DE RID: 17374
	public GameObject content;

	// Token: 0x040043DF RID: 17375
	private GameObject target;

	// Token: 0x040043E0 RID: 17376
	public LocText DescriptionText;
}
