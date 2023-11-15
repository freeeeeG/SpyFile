using System;
using Database;
using UnityEngine;

// Token: 0x02000B44 RID: 2884
public class KleiPermitDioramaVis_BuildingOnFloor : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060058FA RID: 22778 RVA: 0x0020981B File Offset: 0x00207A1B
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x00209823 File Offset: 0x00207A23
	public void ConfigureSetup()
	{
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x00209828 File Offset: 0x00207A28
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
	}

	// Token: 0x04003C2D RID: 15405
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
