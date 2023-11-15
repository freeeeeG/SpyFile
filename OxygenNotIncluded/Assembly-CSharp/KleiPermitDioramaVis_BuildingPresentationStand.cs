using System;
using Database;
using UnityEngine;

// Token: 0x02000B45 RID: 2885
public class KleiPermitDioramaVis_BuildingPresentationStand : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060058FE RID: 22782 RVA: 0x00209850 File Offset: 0x00207A50
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060058FF RID: 22783 RVA: 0x00209858 File Offset: 0x00207A58
	public void ConfigureSetup()
	{
	}

	// Token: 0x06005900 RID: 22784 RVA: 0x0020985C File Offset: 0x00207A5C
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.anchorPos, KleiPermitVisUtil.GetBuildingDef(permit).Unwrap(), this.lastAlignment);
	}

	// Token: 0x06005901 RID: 22785 RVA: 0x002098A8 File Offset: 0x00207AA8
	public KleiPermitDioramaVis_BuildingPresentationStand WithAlignment(Alignment alignment)
	{
		this.lastAlignment = alignment;
		this.anchorPos = new Vector2(alignment.x.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-160f, 160f)), alignment.y.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-156f, 156f)));
		return this;
	}

	// Token: 0x04003C2E RID: 15406
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04003C2F RID: 15407
	private Alignment lastAlignment;

	// Token: 0x04003C30 RID: 15408
	private Vector2 anchorPos;

	// Token: 0x04003C31 RID: 15409
	public const float LEFT = -160f;

	// Token: 0x04003C32 RID: 15410
	public const float TOP = 156f;
}
