using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000094 RID: 148
public class UI_Icon_RuneStoreSingleGood : UI_Icon
{
	// Token: 0x0600052B RID: 1323 RVA: 0x0001E514 File Offset: 0x0001C714
	private void Update()
	{
		if (this.ifMouseAbove && (MyInput.GetKeyDownWithSound(KeyCode.S) || MyInput.GetKeyDownWithSound(Input.GetMouseButtonDown(2))))
		{
			UI_Panel_Main_RunePanel.inst.ViewDetail(this.theGood.theRune);
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0001E54C File Offset: 0x0001C74C
	public void InitIcon_AsGeometryCoin()
	{
		this.iconRune.gameObject.SetActive(false);
		this.objIconGeometryCoin.SetActive(true);
		this.objIconPriceStar.SetActive(true);
		this.objIconPriceGeometryCoin.SetActive(false);
		this.goodType = UI_Icon_RuneStoreSingleGood.EnumGoodType.GEOMETRYCOIN;
		this.textGoodName.text = LanguageText.Inst.tooltip_GeometryCoin.geometryCoin + " x10";
		this.textPrice.text = GameData.inst.runeStore.Get_Current10GeometryCoinPrice().ToString();
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0001E5DC File Offset: 0x0001C7DC
	public void InitIcon_AsRuneGood(RuneGood runeGood)
	{
		this.theGood = runeGood;
		this.iconRune.gameObject.SetActive(true);
		this.objIconGeometryCoin.SetActive(false);
		this.objIconPriceStar.SetActive(false);
		this.objIconPriceGeometryCoin.SetActive(true);
		this.goodType = UI_Icon_RuneStoreSingleGood.EnumGoodType.RUNEGOOD;
		this.textGoodName.text = DataBase.Inst.Data_RuneDatas[runeGood.theRune.typeID].Language_Name;
		this.textPrice.text = runeGood.thePrice.ToString();
		this.iconRune.UpdateIcon_WithRune(runeGood.theRune, UI_Icon_Rune.EnumIconRuneType.RUNESTORE);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0001E67C File Offset: 0x0001C87C
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId == -1)
		{
			switch (this.goodType)
			{
			case UI_Icon_RuneStoreSingleGood.EnumGoodType.UNINITED:
				Debug.LogError("Error_GoodType==UNINITED!!");
				return;
			case UI_Icon_RuneStoreSingleGood.EnumGoodType.GEOMETRYCOIN:
				if (MyInput.KeyCtrlHold())
				{
					UI_Panel_Main_RunePanel.inst.StartCoroutine(GameData.inst.runeStore.TryBuy10GeometryCoins_Repeat100Times());
					return;
				}
				if (MyInput.KeyShiftHold())
				{
					UI_Panel_Main_RunePanel.inst.StartCoroutine(GameData.inst.runeStore.TryBuy10GeometryCoins_Repeat10Times());
					return;
				}
				GameData.inst.runeStore.TryBuy10GeometryCoins();
				return;
			case UI_Icon_RuneStoreSingleGood.EnumGoodType.RUNEGOOD:
				this.theGood.TryBuyThisRuneGood();
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0001E71C File Offset: 0x0001C91C
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.ifMouseAbove = true;
		base.TextSetHighlight();
		switch (this.goodType)
		{
		case UI_Icon_RuneStoreSingleGood.EnumGoodType.UNINITED:
			Debug.LogError("Error_GoodType==UNINITED!!");
			return;
		case UI_Icon_RuneStoreSingleGood.EnumGoodType.GEOMETRYCOIN:
			UI_ToolTip.inst.ShowWithString(this.GetString_ToolTipInfo());
			return;
		case UI_Icon_RuneStoreSingleGood.EnumGoodType.RUNEGOOD:
			UI_Panel_Rune_RuneDetail.inst.SetWithRuneGood(this);
			this.iconRune.OutlineNew_Show_Selected();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001E782 File Offset: 0x0001C982
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.ifMouseAbove = false;
		base.TextSetNormal();
		UI_ToolTip.inst.Close();
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0001E79C File Offset: 0x0001C99C
	private string GetString_ToolTipInfo()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.RuneInfo runeInfo = inst.runeInfo;
		UI_Setting inst2 = UI_Setting.Inst;
		UI_Setting.CommonSets commonSets = inst2.commonSets;
		switch (this.goodType)
		{
		case UI_Icon_RuneStoreSingleGood.EnumGoodType.UNINITED:
			Debug.LogError("Error_GoodType==UNINITED!!");
			return "Error";
		case UI_Icon_RuneStoreSingleGood.EnumGoodType.GEOMETRYCOIN:
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(runeInfo.runeStore_BuyGeometryCoin_Title.TextSet(commonSets.blueSmallTile)).AppendLine();
			stringBuilder.Append(runeInfo.runeStore_BuyGeometryCoin_Info.Replace("sPrice", GameData.inst.runeStore.Get_Current10GeometryCoinPrice().ToString().Colored(inst2.color_Input)).Replace("sGC", 10.ToString().Colored(inst2.color_Input)));
			stringBuilder.AppendLine().AppendLine().Append(inst.main_Common.tip_ShiftBuy10times).AppendLine().Append(inst.main_Common.tip_CtrlBuy100times);
			return stringBuilder.ToString();
		}
		case UI_Icon_RuneStoreSingleGood.EnumGoodType.RUNEGOOD:
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(runeInfo.runeStore_BuyRuneGood_Title.TextSet(commonSets.blueSmallTile)).AppendLine();
			stringBuilder2.Append(runeInfo.runeStore_BuyRuneGood_Info.Replace("sPrice", this.theGood.thePrice.ToString().Colored(inst2.color_Input)));
			stringBuilder2.AppendLine().AppendLine();
			stringBuilder2.Append(this.theGood.theRune.GetInfo_Total());
			stringBuilder2.AppendLine().AppendLine().Append(runeInfo.tip_ViewDetail);
			return stringBuilder2.ToString();
		}
		default:
			Debug.LogError("Error_GoodType==??");
			return "Error";
		}
	}

	// Token: 0x04000442 RID: 1090
	[Header("Icons")]
	[SerializeField]
	private GameObject objIconGeometryCoin;

	// Token: 0x04000443 RID: 1091
	[SerializeField]
	private UI_Icon_Rune iconRune;

	// Token: 0x04000444 RID: 1092
	[SerializeField]
	private GameObject objIconPriceGeometryCoin;

	// Token: 0x04000445 RID: 1093
	[SerializeField]
	private GameObject objIconPriceStar;

	// Token: 0x04000446 RID: 1094
	[Header("Texts")]
	[SerializeField]
	private Text textGoodName;

	// Token: 0x04000447 RID: 1095
	[SerializeField]
	private Text textPrice;

	// Token: 0x04000448 RID: 1096
	[SerializeField]
	private UI_Icon_RuneStoreSingleGood.EnumGoodType goodType;

	// Token: 0x04000449 RID: 1097
	[SerializeField]
	public RuneGood theGood;

	// Token: 0x0400044A RID: 1098
	[SerializeField]
	private bool ifMouseAbove;

	// Token: 0x02000154 RID: 340
	private enum EnumGoodType
	{
		// Token: 0x040009D9 RID: 2521
		UNINITED,
		// Token: 0x040009DA RID: 2522
		GEOMETRYCOIN,
		// Token: 0x040009DB RID: 2523
		RUNEGOOD
	}
}
