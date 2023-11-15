using System;
using Database;
using UnityEngine;

// Token: 0x02000B3F RID: 2879
public class KleiPermitDioramaVis_ArtablePainting : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060058E6 RID: 22758 RVA: 0x002094E2 File Offset: 0x002076E2
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060058E7 RID: 22759 RVA: 0x002094EA File Offset: 0x002076EA
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x060058E8 RID: 22760 RVA: 0x00209500 File Offset: 0x00207700
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		BuildingDef value = KleiPermitVisUtil.GetBuildingDef(permit).Value;
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f * (float)value.HeightInCells / 2f + 176f);
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.9f;
	}

	// Token: 0x04003C25 RID: 15397
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04003C26 RID: 15398
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
