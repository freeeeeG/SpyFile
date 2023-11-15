using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AF RID: 175
public class UI_Panel_Battle_UpgradeShop_MysteryBag : MonoBehaviour
{
	// Token: 0x06000610 RID: 1552 RVA: 0x00022A7C File Offset: 0x00020C7C
	public void Init()
	{
		this.slots[0].gameObject.SetActive(true);
		this.slots[1].gameObject.SetActive(true);
		this.ifActive = true;
		base.gameObject.SetActive(this.ifActive);
		this.slots[0].InitMysteryBagWithID(DataSelector.RandomUpgrade_WithRank(EnumRank.LEGENDARY).id);
		this.slots[1].InitMysteryBagWithID(DataSelector.RandomUpgrade_WithRank(EnumRank.LEGENDARY).id);
		LanguageText.ShopMenu shopMenu = LanguageText.Inst.shopMenu;
		this.textsGet[0].text = shopMenu.mysteryBag_Get.AppendKeycode("1");
		this.textsRefuse[0].text = shopMenu.mysteryBag_Refuse.AppendKeycode("2");
		this.textsGet[1].text = shopMenu.mysteryBag_Get.AppendKeycode("3");
		this.textsRefuse[1].text = shopMenu.mysteryBag_Refuse.AppendKeycode("4");
		this.textTitle.text = shopMenu.mysteryBag_Title;
		this.textTip.text = shopMenu.mysteryBag_Tip;
		RectTransform[] array = this.rects;
		for (int i = 0; i < array.Length; i++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(array[i]);
		}
		this.Record_UnlockUpgrade();
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00022BBC File Offset: 0x00020DBC
	public void Get(int order)
	{
		if (order < 0 || order > 1)
		{
			Debug.LogError("Error_MysteryBagOrderError");
			return;
		}
		Battle.inst.Upgrade_Gain(this.slots[order].upgradeID);
		this.slots[order].gameObject.SetActive(false);
		this.DetectClose();
		UI_Panel_Battle_UpgradeShop.inst.Init();
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00022C16 File Offset: 0x00020E16
	public void Refuse(int order)
	{
		if (order < 0 || order > 1)
		{
			Debug.LogError("Error_MysteryBagOrderError");
			return;
		}
		this.slots[order].gameObject.SetActive(false);
		this.DetectClose();
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00022C44 File Offset: 0x00020E44
	private void DetectClose()
	{
		if (this.slots[0].gameObject.activeSelf)
		{
			return;
		}
		if (this.slots[1].gameObject.activeSelf)
		{
			return;
		}
		this.ifActive = false;
		base.gameObject.SetActive(this.ifActive);
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00022C94 File Offset: 0x00020E94
	private void Record_UnlockUpgrade()
	{
		for (int i = 0; i < 2; i++)
		{
			GameData.inst.record.Upgrade_GainOnce(this.slots[i].upgradeID);
		}
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x0400050A RID: 1290
	[SerializeField]
	public UI_Panel_Battle_UpgradeShop_Single[] slots = new UI_Panel_Battle_UpgradeShop_Single[2];

	// Token: 0x0400050B RID: 1291
	public bool ifActive;

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	private Text textTitle;

	// Token: 0x0400050D RID: 1293
	[SerializeField]
	private Text textTip;

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	private Text[] textsGet = new Text[2];

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	private Text[] textsRefuse = new Text[2];

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	private RectTransform[] rects;
}
