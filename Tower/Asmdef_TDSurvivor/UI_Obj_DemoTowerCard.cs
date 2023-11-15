using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class UI_Obj_DemoTowerCard : MonoBehaviour
{
	// Token: 0x060009FC RID: 2556 RVA: 0x000257B4 File Offset: 0x000239B4
	public void SetCardContent(eItemType itemType)
	{
		eCardType cardType = itemType.ToCardType();
		AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(itemType);
		this.cardface.SetupContent(itemType, cardType, itemDataByType.GetCardIcon(), true);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x000257E8 File Offset: 0x000239E8
	public void SetShowCard(bool isShow)
	{
		this.cardface.gameObject.SetActive(isShow);
		this.node_Locked.gameObject.SetActive(!isShow);
	}

	// Token: 0x040007BE RID: 1982
	[SerializeField]
	private UI_CardFace cardface;

	// Token: 0x040007BF RID: 1983
	[SerializeField]
	private Transform node_Locked;
}
