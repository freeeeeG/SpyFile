using System;
using Database;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
public class KleiPermitDioramaVis_ArtableSticker : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060058EE RID: 22766 RVA: 0x002095F4 File Offset: 0x002077F4
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060058EF RID: 22767 RVA: 0x002095FC File Offset: 0x002077FC
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x060058F0 RID: 22768 RVA: 0x00209610 File Offset: 0x00207810
	public void ConfigureWith(PermitResource permit)
	{
		DbStickerBomb artablePermit = (DbStickerBomb)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
	}

	// Token: 0x04003C28 RID: 15400
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
