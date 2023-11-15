using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D2 RID: 210
public class UI_Panel_Rune_RuneDetail : MonoBehaviour
{
	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x000291A0 File Offset: 0x000273A0
	public static UI_Panel_Rune_RuneDetail inst
	{
		get
		{
			return UI_Panel_Main_RunePanel.inst.panelRuneDetail;
		}
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x000291AC File Offset: 0x000273AC
	private void Update()
	{
		this.DetectHotKey();
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x000291B4 File Offset: 0x000273B4
	public void UpdatePreview()
	{
		if (this.runeStoreSingleGood != null)
		{
			this.OpenWithRune(this.runeStoreSingleGood.theGood.theRune);
			return;
		}
		if (this.ifRuneValid(this.tempPreviewRune))
		{
			this.OpenWithRune(this.tempPreviewRune);
			return;
		}
		this.tempPreviewRune = null;
		if (this.ifRuneValid(this.lockedPreviewRune))
		{
			this.OpenWithRune(this.lockedPreviewRune);
			return;
		}
		this.lockedPreviewRune = null;
		this.Close();
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00029230 File Offset: 0x00027430
	public void ClearAll()
	{
		this.runeStoreSingleGood = null;
		this.tempPreviewRune = null;
		this.lockedPreviewRune = null;
		this.Close();
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0002924D File Offset: 0x0002744D
	public void SetWithRuneGood(UI_Icon_RuneStoreSingleGood good)
	{
		this.runeStoreSingleGood = good;
		this.ifBuy = true;
		this.tempPreviewRune = null;
		this.lockedPreviewRune = null;
		this.UpdatePreview();
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00029271 File Offset: 0x00027471
	public void ClearRuneGood()
	{
		this.runeStoreSingleGood = null;
		this.ifBuy = false;
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00029281 File Offset: 0x00027481
	public void SetTempPreview(Rune rune)
	{
		this.ClearRuneGood();
		this.tempPreviewRune = rune;
		this.UpdatePreview();
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00029296 File Offset: 0x00027496
	public void SetLockedPreview(Rune rune)
	{
		this.ClearRuneGood();
		this.lockedPreviewRune = rune;
		this.UpdatePreview();
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x000292AB File Offset: 0x000274AB
	public bool ifRuneSelected(Rune rune)
	{
		return rune == this.lockedPreviewRune;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x000292B9 File Offset: 0x000274B9
	public bool ifRuneGoodPreview(Rune rune)
	{
		return this.runeStoreSingleGood != null && this.runeStoreSingleGood.theGood != null && rune == this.runeStoreSingleGood.theGood.theRune;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x000292EC File Offset: 0x000274EC
	public bool ifTempAbove(Rune rune)
	{
		return rune == this.tempPreviewRune;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x000292F7 File Offset: 0x000274F7
	private bool ifRuneValid(Rune rune)
	{
		return rune != null && GameData.inst.runes.Contains(rune);
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00029314 File Offset: 0x00027514
	public void OpenWithRune(Rune rune)
	{
		UI_Panel_Main_RunePanel.inst.panelSort.Close();
		base.gameObject.SetActive(true);
		this.runeOnPreview = rune;
		this.panelRuneDetail.InitWithRune(rune, this.singleLine_MaxWidth, UI_Icon_Rune.EnumIconRuneType.DETAIL, false, false);
		LanguageText.RuneInfo runeInfo = LanguageText.Inst.runeInfo;
		this.textTitle.text = runeInfo.title_RuneDetail;
		this.rectPanelTitle.sizeDelta = new Vector2(this.width, this.rectPanelTitle.sizeDelta.y);
		this.objButtonsParent.GetComponent<RectTransform>().localPosition = new Vector2(0f, this.posYorigin - this.posYdeltaPerRow * (float)this.panelRuneDetail.GetRowCount());
		this.rectPanelTotal.sizeDelta = new Vector2(this.width, this.heightDelta + this.posYdeltaPerRow * (float)this.panelRuneDetail.GetRowCount());
		UI_Panel_Main_RunePanel.EnumRunePanelMode panelMode = UI_Panel_Main_RunePanel.inst.panelMode;
		if (panelMode != UI_Panel_Main_RunePanel.EnumRunePanelMode.MANAGE)
		{
			if (panelMode != UI_Panel_Main_RunePanel.EnumRunePanelMode.STORE)
			{
				return;
			}
			this.objButtonSet_Manage.SetActive(false);
			this.objButtonSet_Store.SetActive(true);
			if (this.ifBuy)
			{
				this.storeMode_PriceText.text = runeInfo.mid_BuyPrice;
				this.storeMode_PriceNum.text = this.runeStoreSingleGood.theGood.thePrice.ToString();
				this.textButton_Buy.text = runeInfo.mid_ButtonBuy.AppendKeycode("B");
				this.textButton_Buy.gameObject.SetActive(true);
				this.textButton_Recycle.gameObject.SetActive(false);
			}
			else
			{
				this.storeMode_PriceText.text = runeInfo.mid_RecyclePrice;
				this.storeMode_PriceNum.text = rune.GetInt_RecyclePrice().ToString();
				this.textButton_Recycle.text = runeInfo.mid_ButtonRecycle.AppendKeycode("X");
				this.textButton_Buy.gameObject.SetActive(false);
				this.textButton_Recycle.gameObject.SetActive(true);
			}
			RectTransform[] array = this.rectsToReforce;
			for (int i = 0; i < array.Length; i++)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(array[i]);
			}
			return;
		}
		else
		{
			this.objButtonSet_Manage.SetActive(true);
			this.objButtonSet_Store.SetActive(false);
			this.textButton_Equip.text = runeInfo.mid_ButtonEquip.AppendKeycode("1");
			this.textButton_Fuse.text = runeInfo.mid_ButtonFuse.AppendKeycode("2");
			if (rune.ifFavorite)
			{
				this.textButton_Mark.text = runeInfo.mid_ButtonMark_Cancle.AppendKeycode("W");
				return;
			}
			this.textButton_Mark.text = runeInfo.mid_ButtonMark.AppendKeycode("W");
			return;
		}
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x000295BC File Offset: 0x000277BC
	private void DetectHotKey()
	{
		UI_Panel_Main_RunePanel.EnumRunePanelMode panelMode = UI_Panel_Main_RunePanel.inst.panelMode;
		if (panelMode != UI_Panel_Main_RunePanel.EnumRunePanelMode.MANAGE)
		{
			if (panelMode != UI_Panel_Main_RunePanel.EnumRunePanelMode.STORE)
			{
				return;
			}
			if (this.ifBuy)
			{
				if (MyInput.GetKeyDownWithSound(KeyCode.B))
				{
					this.Button_Buy();
					return;
				}
			}
			else if (MyInput.GetKeyDownWithSound(KeyCode.X))
			{
				this.Button_Recycle();
			}
		}
		else
		{
			if (MyInput.GetKeyDownWithSound(KeyCode.Alpha1) || MyInput.GetKeyDownWithSound(KeyCode.Keypad1))
			{
				this.Button_GoEquip();
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.Alpha2) || MyInput.GetKeyDownWithSound(KeyCode.Keypad2))
			{
				this.Button_GoFuse();
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.W))
			{
				this.Button_Mark();
				return;
			}
		}
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0002964C File Offset: 0x0002784C
	public void Button_GoEquip()
	{
		int index = GameData.inst.runes.IndexOf(this.runeOnPreview);
		UI_Panel_Main_RunePanel.inst.RuneEquip_Equip(index);
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0002967C File Offset: 0x0002787C
	public void Button_GoFuse()
	{
		int index = GameData.inst.runes.IndexOf(this.runeOnPreview);
		UI_Panel_Main_RunePanel.inst.RuneSyn_Add(index);
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x000296AA File Offset: 0x000278AA
	public void Button_Mark()
	{
		this.runeOnPreview.ifFavorite = !this.runeOnPreview.ifFavorite;
		UI_Panel_Main_RunePanel.inst.Open();
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x000296D0 File Offset: 0x000278D0
	public void Button_Recycle()
	{
		if (this.runeOnPreview.ifFavorite)
		{
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.runeInfo.floatTip_CantRecycleFavorite);
			return;
		}
		if (Rune.RemoveRune(this.runeOnPreview))
		{
			int int_RecyclePrice = this.runeOnPreview.GetInt_RecyclePrice();
			GameData.inst.GeometryCoin_Get((long)int_RecyclePrice);
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.rune_RecycleRune.Replace("sPrice", int_RecyclePrice.ToString()).Replace("sRune", this.runeOnPreview.Get_Lang_RuneName()));
		}
		else
		{
			Debug.LogError("Error_符文回收异常");
		}
		UI_Panel_Main_RunePanel.inst.Open();
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0002977E File Offset: 0x0002797E
	public void Button_Buy()
	{
		this.runeStoreSingleGood.theGood.TryBuyThisRuneGood();
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000613 RID: 1555
	private Rune runeOnPreview;

	// Token: 0x04000614 RID: 1556
	[SerializeField]
	private Text textTitle;

	// Token: 0x04000615 RID: 1557
	[SerializeField]
	private UI_Panel_Main_RuneSynResult_Child panelRuneDetail;

	// Token: 0x04000616 RID: 1558
	[Header("布局参数")]
	[SerializeField]
	private RectTransform rectPanelTotal;

	// Token: 0x04000617 RID: 1559
	[SerializeField]
	private RectTransform rectPanelTitle;

	// Token: 0x04000618 RID: 1560
	[SerializeField]
	private float width = 558f;

	// Token: 0x04000619 RID: 1561
	[SerializeField]
	private float singleLine_MaxWidth = 450f;

	// Token: 0x0400061A RID: 1562
	[SerializeField]
	private float posYorigin = -500f;

	// Token: 0x0400061B RID: 1563
	[SerializeField]
	private float posYdeltaPerRow = 50f;

	// Token: 0x0400061C RID: 1564
	[SerializeField]
	private float heightDelta = 550f;

	// Token: 0x0400061D RID: 1565
	[Header("Previews")]
	private Rune lockedPreviewRune;

	// Token: 0x0400061E RID: 1566
	private Rune tempPreviewRune;

	// Token: 0x0400061F RID: 1567
	[Header("Buttons")]
	[SerializeField]
	private GameObject objButtonsParent;

	// Token: 0x04000620 RID: 1568
	[SerializeField]
	private GameObject objButtonSet_Manage;

	// Token: 0x04000621 RID: 1569
	[SerializeField]
	private GameObject objButtonSet_Store;

	// Token: 0x04000622 RID: 1570
	[SerializeField]
	private Text textButton_Equip;

	// Token: 0x04000623 RID: 1571
	[SerializeField]
	private Text textButton_Fuse;

	// Token: 0x04000624 RID: 1572
	[SerializeField]
	private Text textButton_Mark;

	// Token: 0x04000625 RID: 1573
	[SerializeField]
	private Text textButton_Recycle;

	// Token: 0x04000626 RID: 1574
	[SerializeField]
	private Text textButton_Buy;

	// Token: 0x04000627 RID: 1575
	[SerializeField]
	private Text storeMode_PriceText;

	// Token: 0x04000628 RID: 1576
	[SerializeField]
	private Text storeMode_PriceNum;

	// Token: 0x04000629 RID: 1577
	[SerializeField]
	private bool ifBuy;

	// Token: 0x0400062A RID: 1578
	[SerializeField]
	private RectTransform[] rectsToReforce;

	// Token: 0x0400062B RID: 1579
	[SerializeField]
	private UI_Icon_RuneStoreSingleGood runeStoreSingleGood;
}
