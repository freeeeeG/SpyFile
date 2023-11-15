using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D4 RID: 212
public class UI_Panel_Rune_RuneStore : UI_Panel_Main_IconList
{
	// Token: 0x06000769 RID: 1897 RVA: 0x000298C9 File Offset: 0x00027AC9
	public override void InitIcons(Transform transformParent = null)
	{
		UI_ToolTip.inst.TryClose();
		base.InitIcons(null);
		this.UpdateLanguage();
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x000298E4 File Offset: 0x00027AE4
	private void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.RuneInfo runeInfo = inst.runeInfo;
		LanguageText.ShopMenu shopMenu = inst.shopMenu;
		this.textTitle.text = runeInfo.runeStore_PanelTitle;
		this.textRefresh.text = shopMenu.refresh.AppendKeycode("R");
		int refreshPrice = GameData.inst.runeStore.Get_RefreshPrice();
		if (refreshPrice == 0)
		{
			this.textPrice.text = shopMenu.refresh_FreeOnce;
		}
		else
		{
			this.textPrice.text = refreshPrice.ToString();
		}
		int num = Mathf.CeilToInt((float)GameData.inst.runeStore.freeRefreshTimeLeft);
		int num2 = num / 60;
		int num3 = num % 60;
		this.textTimeCount.text = runeInfo.runeStore_FreeRefreshTimeCountDown.Replace("sMin", num2.ToString()).Replace("sSec", num3.ToString());
		RectTransform[] array = this.rects;
		for (int i = 0; i < array.Length; i++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(array[i]);
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x000299DE File Offset: 0x00027BDE
	private void Update()
	{
		this.panelUpdateTimeLeft -= Time.unscaledDeltaTime;
		if (this.panelUpdateTimeLeft <= 0f)
		{
			this.UpdateLanguage();
			this.panelUpdateTimeLeft = 1f;
		}
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00029A10 File Offset: 0x00027C10
	protected override int IconNum()
	{
		return 6;
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00029A14 File Offset: 0x00027C14
	protected override bool IfAvailable(int ID)
	{
		if (ID == 0)
		{
			return true;
		}
		RuneGood runeGood = GameData.inst.runeStore.runeGoods[ID - 1];
		return runeGood != null && runeGood.indexInGood != -1;
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x00029A4C File Offset: 0x00027C4C
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		UI_Icon_RuneStoreSingleGood component = obj.GetComponent<UI_Icon_RuneStoreSingleGood>();
		if (ID == 0)
		{
			component.InitIcon_AsGeometryCoin();
			return;
		}
		RuneGood runeGood = GameData.inst.runeStore.runeGoods[ID - 1];
		component.InitIcon_AsRuneGood(runeGood);
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x00029A85 File Offset: 0x00027C85
	public void Icon_TryRefresh()
	{
		GameData.inst.runeStore.TryPayAndRefresh();
	}

	// Token: 0x04000632 RID: 1586
	[SerializeField]
	private Text textTitle;

	// Token: 0x04000633 RID: 1587
	[SerializeField]
	private Text textRefresh;

	// Token: 0x04000634 RID: 1588
	[SerializeField]
	private Text textPrice;

	// Token: 0x04000635 RID: 1589
	[SerializeField]
	private Text textTimeCount;

	// Token: 0x04000636 RID: 1590
	[SerializeField]
	private RectTransform[] rects;

	// Token: 0x04000637 RID: 1591
	[SerializeField]
	private float panelUpdateTimeLeft = 1f;
}
