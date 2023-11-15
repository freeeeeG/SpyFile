using System;
using Database;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
public class KleiPermitDioramaVis_BuildingHangingHook : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060058F2 RID: 22770 RVA: 0x00209638 File Offset: 0x00207838
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060058F3 RID: 22771 RVA: 0x00209640 File Offset: 0x00207840
	public void ConfigureSetup()
	{
	}

	// Token: 0x060058F4 RID: 22772 RVA: 0x00209644 File Offset: 0x00207844
	public void ConfigureWith(PermitResource permit)
	{
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (BuildingFacadeResource)permit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.buildingKAnimPosition, KleiPermitVisUtil.GetBuildingDef(permit).Unwrap(), Alignment.Top());
	}

	// Token: 0x04003C29 RID: 15401
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04003C2A RID: 15402
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
