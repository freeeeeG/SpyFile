using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class CardManager : Singleton<CardManager>
{
	// Token: 0x060001E8 RID: 488 RVA: 0x0000898A File Offset: 0x00006B8A
	private void OnEnable()
	{
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000898C File Offset: 0x00006B8C
	private void OnDisable()
	{
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000898E File Offset: 0x00006B8E
	private void OnRequestRedrawCards()
	{
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00008990 File Offset: 0x00006B90
	public CardData CreateCardData(eItemType cardType, bool isFromPlayerStorage, eItemType cardType_Combine = eItemType.NONE)
	{
		CardData cardData;
		if (cardType_Combine == eItemType.NONE)
		{
			AItemSettingData aitemSettingData = Singleton<ResourceManager>.Instance.GetItemDataByType(cardType);
			aitemSettingData = Object.Instantiate<AItemSettingData>(aitemSettingData);
			cardData = this.CreateCardData(aitemSettingData, isFromPlayerStorage);
		}
		else
		{
			CannonSettingData cannonSettingData = Singleton<ResourceManager>.Instance.GetItemDataByType(cardType) as CannonSettingData;
			PanelSettingData panelSettingData = Singleton<ResourceManager>.Instance.GetItemDataByType(cardType_Combine) as PanelSettingData;
			cannonSettingData = Object.Instantiate<CannonSettingData>(cannonSettingData);
			panelSettingData = Object.Instantiate<PanelSettingData>(panelSettingData);
			cannonSettingData.CombineMultiplier(panelSettingData);
			cardData = this.CreateCardData(cannonSettingData, isFromPlayerStorage);
			cardData.CardType = eCardType.TOWER_CARD;
		}
		return cardData;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00008A08 File Offset: 0x00006C08
	public CardData CreateCardData(AItemSettingData scriptableObjData, bool isFromPlayerStorage)
	{
		return new CardData(scriptableObjData, isFromPlayerStorage);
	}
}
