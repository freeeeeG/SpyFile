using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class UI_Obj_ShopCard : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001E0AF File Offset: 0x0001C2AF
	public eItemType ItemType
	{
		get
		{
			return this.curItemType;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060007DB RID: 2011 RVA: 0x0001E0B7 File Offset: 0x0001C2B7
	public AItemSettingData Data
	{
		get
		{
			return this.itemData;
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0001E0BF File Offset: 0x0001C2BF
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
		Debug.Log("button.onClick.AddListener(OnClickButton);");
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0001E0E7 File Offset: 0x0001C2E7
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x0001E105 File Offset: 0x0001C305
	public void OnClickButton()
	{
		Debug.Log(string.Format("點擊卡片: {0}", this.curItemType), base.gameObject);
		Action<UI_Obj_ShopCard> onCardClicked = this.OnCardClicked;
		if (onCardClicked == null)
		{
			return;
		}
		onCardClicked(this);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x0001E138 File Offset: 0x0001C338
	public void ToggleClickable(bool isClickable)
	{
		this.button.enabled = isClickable;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x0001E146 File Offset: 0x0001C346
	public void SetupContent(DiscoverRewardData data, UI_Obj_ShopCard.eCardSelectType cardSelectType, int price)
	{
		this.SetupContent(this.GetItemTypeFromData(data), cardSelectType, price);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0001E158 File Offset: 0x0001C358
	public void SetupContent(eItemType itemType, UI_Obj_ShopCard.eCardSelectType cardSelectType, int price)
	{
		this.curItemType = itemType;
		this.isClicked = false;
		this.cardSelectType = cardSelectType;
		this.price = price;
		this.itemData = Singleton<ResourceManager>.Instance.GetItemDataByType(this.curItemType);
		this.cardType = this.itemData.GetItemType().ToCardType();
		this.UpdateUI(this.itemData, this.cardType);
		switch (cardSelectType)
		{
		case UI_Obj_ShopCard.eCardSelectType.NONE:
		case UI_Obj_ShopCard.eCardSelectType.SELECTABLE:
			this.node_Price.SetActive(false);
			break;
		case UI_Obj_ShopCard.eCardSelectType.BUYABLE:
			this.node_Price.SetActive(true);
			break;
		}
		if (this.curItemType.ToCardType() == eCardType.TOWER_CARD)
		{
			this.cardFace.SetTowerDetail(this.curItemType, true, true);
		}
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0001E210 File Offset: 0x0001C410
	public void UpdatePrize(int curCurrency)
	{
		if (this.price <= 0)
		{
			return;
		}
		this.text_Price.text = this.price.ToString();
		this.text_Price.color = ((curCurrency >= this.price) ? Color.white : Color.red);
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0001E260 File Offset: 0x0001C460
	private void UpdateUI(AItemSettingData itemData, eCardType cardType)
	{
		Sprite cardIcon = itemData.GetCardIcon();
		this.cardFace.SetupContent(itemData.GetItemType(), cardType, cardIcon, true);
		this.text_Price.text = itemData.GetStoreCost().ToString();
		switch (cardType)
		{
		case eCardType.NONE:
		case eCardType.PANEL_CARD:
		case eCardType.WALL_CARD:
		case eCardType.COIN_CARD:
		case eCardType.HP_CARD:
			break;
		case eCardType.BUFF_CARD:
		case eCardType.TOWER_CARD:
			this.cardFace.ToggleNameText(true);
			return;
		default:
			if (cardType != eCardType.UNDEFINED)
			{
				return;
			}
			break;
		}
		this.cardFace.ToggleNameText(false);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0001E2E5 File Offset: 0x0001C4E5
	private eItemType GetItemTypeFromData(DiscoverRewardData data)
	{
		if (data.List_RewardContentType != null && data.List_RewardContentType.Count != 0)
		{
			return data.List_RewardContentType[0];
		}
		return eItemType.NONE;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0001E30A File Offset: 0x0001C50A
	public void ToggleCard(bool isOn)
	{
		this.Toggle(isOn);
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0001E313 File Offset: 0x0001C513
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0001E328 File Offset: 0x0001C528
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.cardType == eCardType.TOWER_CARD || this.cardType == eCardType.BUFF_CARD)
		{
			AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(this.curItemType);
			string locNameString = itemDataByType.GetLocNameString(false);
			string arg = itemDataByType.GetLocStatsString() + "\n" + itemDataByType.GetLocFlavorTextString();
			EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
			EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, arg);
			EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._2D, base.transform, Vector3.up * 50f + Vector3.right * 50f);
		}
		base.transform.DOPunchRotation(Vector3.forward * 3f, 0.5f, 10, 1f);
		Action<UI_Obj_ShopCard> onCardMouseEnter = this.OnCardMouseEnter;
		if (onCardMouseEnter != null)
		{
			onCardMouseEnter(this);
		}
		SoundManager.PlaySound("UI", "MouseOverCard", -1f, -1f, -1f);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0001E42F File Offset: 0x0001C62F
	public string GetLocNameString()
	{
		return Singleton<ResourceManager>.Instance.GetItemDataByType(this.curItemType).GetLocNameString(false);
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0001E448 File Offset: 0x0001C648
	public string GetLocTooltipString()
	{
		AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(this.curItemType);
		return itemDataByType.GetLocStatsString() + "\n" + itemDataByType.GetLocFlavorTextString();
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0001E47C File Offset: 0x0001C67C
	public void OnPointerExit(PointerEventData eventData)
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
		Action<UI_Obj_ShopCard> onCardMouseExit = this.OnCardMouseExit;
		if (onCardMouseExit == null)
		{
			return;
		}
		onCardMouseExit(this);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
	public void PlayPurchaseAnimation()
	{
		this.node_Price.SetActive(false);
		this.button.interactable = false;
		this.animator.SetTrigger("purchased");
		this.Toggle(false);
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0001E4D1 File Offset: 0x0001C6D1
	public void PlayPurchaseFailAnimation()
	{
		this.animator.SetTrigger("purchase_fail");
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0001E4E3 File Offset: 0x0001C6E3
	public void OnPointerClick(PointerEventData eventData)
	{
		Action<UI_Obj_ShopCard> onCardClicked = this.OnCardClicked;
		if (onCardClicked == null)
		{
			return;
		}
		onCardClicked(this);
	}

	// Token: 0x04000663 RID: 1635
	[SerializeField]
	private Animator animator;

	// Token: 0x04000664 RID: 1636
	[SerializeField]
	private UI_CardFace cardFace;

	// Token: 0x04000665 RID: 1637
	[SerializeField]
	private Button button;

	// Token: 0x04000666 RID: 1638
	[SerializeField]
	private GameObject node_Price;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	private TMP_Text text_Price;

	// Token: 0x04000668 RID: 1640
	[SerializeField]
	private UI_Obj_ShopCard.eCardSelectType cardSelectType;

	// Token: 0x04000669 RID: 1641
	private eItemType curItemType;

	// Token: 0x0400066A RID: 1642
	private AItemSettingData itemData;

	// Token: 0x0400066B RID: 1643
	private bool isClicked;

	// Token: 0x0400066C RID: 1644
	private eCardType cardType;

	// Token: 0x0400066D RID: 1645
	private int price;

	// Token: 0x0400066E RID: 1646
	public Action<UI_Obj_ShopCard> OnCardClicked;

	// Token: 0x0400066F RID: 1647
	public Action<UI_Obj_ShopCard> OnCardMouseEnter;

	// Token: 0x04000670 RID: 1648
	public Action<UI_Obj_ShopCard> OnCardMouseExit;

	// Token: 0x0200027B RID: 635
	public enum eCardSelectType
	{
		// Token: 0x04000BD3 RID: 3027
		NONE,
		// Token: 0x04000BD4 RID: 3028
		BUYABLE,
		// Token: 0x04000BD5 RID: 3029
		SELECTABLE
	}
}
