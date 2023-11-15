using System;
using Database;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
public class KleiPermitDioramaVis_BuildingPresentationStandHanging : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06005903 RID: 22787 RVA: 0x00209922 File Offset: 0x00207B22
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06005904 RID: 22788 RVA: 0x0020992A File Offset: 0x00207B2A
	public void ConfigureSetup()
	{
	}

	// Token: 0x06005905 RID: 22789 RVA: 0x0020992C File Offset: 0x00207B2C
	public void ConfigureWith(PermitResource permit)
	{
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (BuildingFacadeResource)permit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.buildingKAnimPosition, KleiPermitVisUtil.GetBuildingDef(permit).Unwrap(), Alignment.Top());
	}

	// Token: 0x04003C33 RID: 15411
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04003C34 RID: 15412
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
