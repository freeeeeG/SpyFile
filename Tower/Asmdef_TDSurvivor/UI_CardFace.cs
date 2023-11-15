using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012D RID: 301
public class UI_CardFace : MonoBehaviour
{
	// Token: 0x060007BD RID: 1981 RVA: 0x0001D588 File Offset: 0x0001B788
	private void OnEnable()
	{
		EventMgr.Register(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0001D5A1 File Offset: 0x0001B7A1
	private void OnDisable()
	{
		EventMgr.Remove(eGameEvents.OnLanguageChanged, new Action(this.OnLanguageChanged));
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0001D5BA File Offset: 0x0001B7BA
	private void OnLanguageChanged()
	{
		this.UpdateCardName();
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0001D5C2 File Offset: 0x0001B7C2
	private void UpdateCardName()
	{
		if (this.doShowName)
		{
			this.text_ItemName.text = Singleton<ResourceManager>.Instance.GetItemDataByType(this.itemType).GetLocNameString(true);
			return;
		}
		this.text_ItemName.text = "";
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0001D600 File Offset: 0x0001B800
	public void SetupContent(eItemType itemType, eCardType cardType, Sprite iconSprite, bool showItemName = false)
	{
		if (iconSprite == null)
		{
			Debug.LogError("IconSprite是空的!!!");
		}
		this.itemType = itemType;
		this.doShowName = showItemName;
		this.image_Icon.enabled = true;
		this.image_Icon.sprite = iconSprite;
		this.image_Icon.AdjustSizeToSprite();
		this.node_TowerCard.gameObject.SetActive(cardType == eCardType.TOWER_CARD);
		this.node_PanelCard.gameObject.SetActive(cardType == eCardType.PANEL_CARD);
		this.node_BuffCard.gameObject.SetActive(cardType == eCardType.BUFF_CARD);
		this.node_HPCard.gameObject.SetActive(cardType == eCardType.HP_CARD);
		this.node_CoinCard.gameObject.SetActive(cardType == eCardType.COIN_CARD);
		this.node_TowerSize.gameObject.SetActive(false);
		this.node_ElementType.gameObject.SetActive(false);
		this.UpdateCardName();
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
	public void SetTowerDetail(eItemType itemType, bool showTowerSize, bool showTowerCost)
	{
		TowerSettingData towerSettingData = Singleton<ResourceManager>.Instance.GetItemDataByType(itemType) as TowerSettingData;
		this.node_TowerSize.gameObject.SetActive(showTowerSize && towerSettingData.TowerSizeType != eTowerSizeType._1x1);
		if (showTowerSize && towerSettingData.TowerSizeType != eTowerSizeType._1x1)
		{
			this.text_TowerSize.text = towerSettingData.TowerSizeType.GetString();
		}
		this.node_TowerCost.gameObject.SetActive(showTowerCost);
		if (showTowerCost)
		{
			this.text_TowerCost.text = towerSettingData.GetBuildCost(1f).ToString();
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0001D774 File Offset: 0x0001B974
	public void SetIconColor(Color color)
	{
		color = (color + Color.white) / 2f;
		this.image_Icon.color = color;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0001D79C File Offset: 0x0001B99C
	public void ToggleNameText(bool isOn)
	{
		if (isOn)
		{
			this.image_Icon.transform.localPosition = new Vector3(0f, 15f, 0f);
		}
		else
		{
			this.image_Icon.transform.localPosition = Vector3.zero;
		}
		this.text_ItemName.enabled = isOn;
	}

	// Token: 0x04000643 RID: 1603
	[SerializeField]
	private Image image_Icon;

	// Token: 0x04000644 RID: 1604
	[SerializeField]
	private TMP_Text text_ItemName;

	// Token: 0x04000645 RID: 1605
	[SerializeField]
	private Transform node_TowerCard;

	// Token: 0x04000646 RID: 1606
	[SerializeField]
	private Transform node_PanelCard;

	// Token: 0x04000647 RID: 1607
	[SerializeField]
	private Transform node_BuffCard;

	// Token: 0x04000648 RID: 1608
	[SerializeField]
	private Transform node_HPCard;

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	private Transform node_CoinCard;

	// Token: 0x0400064A RID: 1610
	[SerializeField]
	private Transform node_TowerSize;

	// Token: 0x0400064B RID: 1611
	[SerializeField]
	private Transform node_ElementType;

	// Token: 0x0400064C RID: 1612
	[SerializeField]
	private Transform node_TowerCost;

	// Token: 0x0400064D RID: 1613
	[SerializeField]
	private TMP_Text text_TowerSize;

	// Token: 0x0400064E RID: 1614
	[SerializeField]
	private TMP_Text text_ElementType;

	// Token: 0x0400064F RID: 1615
	[SerializeField]
	private TMP_Text text_TowerCost;

	// Token: 0x04000650 RID: 1616
	private eItemType itemType;

	// Token: 0x04000651 RID: 1617
	private bool doShowName;
}
