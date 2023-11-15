using System;
using Database;
using UnityEngine;

// Token: 0x02000B40 RID: 2880
public class KleiPermitDioramaVis_ArtableSculpture : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060058EA RID: 22762 RVA: 0x002095B0 File Offset: 0x002077B0
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060058EB RID: 22763 RVA: 0x002095B8 File Offset: 0x002077B8
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x060058EC RID: 22764 RVA: 0x002095CC File Offset: 0x002077CC
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
	}

	// Token: 0x04003C27 RID: 15399
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
