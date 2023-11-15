using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B4A RID: 2890
public class KleiPermitDioramaVis_PedestalAndItem : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06005919 RID: 22809 RVA: 0x00209CF6 File Offset: 0x00207EF6
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600591A RID: 22810 RVA: 0x00209CFE File Offset: 0x00207EFE
	public void ConfigureSetup()
	{
	}

	// Token: 0x0600591B RID: 22811 RVA: 0x00209D00 File Offset: 0x00207F00
	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		RectTransform rectTransform = this.pedestalKAnim.rectTransform();
		RectTransform rectTransform2 = this.itemSprite.rectTransform();
		rectTransform.pivot = new Vector2(0.5f, 0f);
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.pedestalKAnim, Assets.GetBuildingDef("ItemPedestal"));
		rectTransform2.pivot = new Vector2(0.5f, 0f);
		rectTransform2.anchoredPosition = rectTransform.anchoredPosition + Vector2.up * 0.79f * 176f;
		rectTransform2.sizeDelta = Vector2.one * 176f;
		this.itemSprite.sprite = permitPresentationInfo.sprite;
	}

	// Token: 0x04003C48 RID: 15432
	[SerializeField]
	private KBatchedAnimController pedestalKAnim;

	// Token: 0x04003C49 RID: 15433
	[SerializeField]
	private Image itemSprite;

	// Token: 0x04003C4A RID: 15434
	private const float TILE_COUNT_TO_PEDESTAL_SLOT = 0.79f;
}
