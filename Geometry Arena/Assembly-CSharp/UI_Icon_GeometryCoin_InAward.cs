using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200008D RID: 141
public class UI_Icon_GeometryCoin_InAward : UI_Icon
{
	// Token: 0x060004F1 RID: 1265 RVA: 0x0001D438 File Offset: 0x0001B638
	public void UpdateIcon(int basicGC, ref int finalGC)
	{
		this.basicGC = (float)basicGC;
		this.gCoinImg.sprite = ResourceLibrary.Inst.sprite_GeometryCoin;
		this.multiMastery = 1f + (float)Mastery.GetRankTotal() * 0.01f;
		EnumModeType modeType = TempData.inst.modeType;
		if (modeType != EnumModeType.ENDLESS)
		{
			if (modeType != EnumModeType.WANDER)
			{
				this.multiMode = 1f;
			}
			else
			{
				this.multiMode = 1.2f;
			}
		}
		else
		{
			this.multiMode = 1.2f;
		}
		finalGC = ((float)basicGC * this.multiMastery * this.multiMode).RoundToInt();
		this.finalGC = (float)finalGC;
		this.gCoinNum.text = finalGC.ToString();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rect);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0001D4F0 File Offset: 0x0001B6F0
	private string MultiInfo()
	{
		LanguageText.Tooltip_GeometryCoin tooltip_GeometryCoin = LanguageText.Inst.tooltip_GeometryCoin;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(this.basicGC).Append(tooltip_GeometryCoin.basic);
		if (this.multiMastery > 1f)
		{
			stringBuilder.AppendLine().Append(MyTool.GetColorfulStringWithTypeAndNum(0, 0, (double)this.multiMastery, false, 0f, true)).Append(tooltip_GeometryCoin.fromMastery);
		}
		if (this.multiMode > 1f)
		{
			stringBuilder.AppendLine().Append(MyTool.GetColorfulStringWithTypeAndNum(0, 0, (double)this.multiMode, false, 0f, true));
			EnumModeType modeType = TempData.inst.modeType;
			if (modeType != EnumModeType.ENDLESS)
			{
				if (modeType != EnumModeType.WANDER)
				{
					Debug.LogError("Error_MultiInfo_ModeError");
				}
				else
				{
					stringBuilder.Append(tooltip_GeometryCoin.fromMode_Wander);
				}
			}
			else
			{
				stringBuilder.Append(tooltip_GeometryCoin.fromMode_Endless);
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0001D5D0 File Offset: 0x0001B7D0
	private string ToolTipInfo()
	{
		LanguageText inst = LanguageText.Inst;
		UI_Setting inst2 = UI_Setting.Inst;
		return "" + inst.toolTip_TipStringsHead[11].TextSet(inst2.commonSets.blueSmallTile) + "\n" + inst.toolTip_TipStrings[11] + "\n\n" + this.MultiInfo();
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0001D634 File Offset: 0x0001B834
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithString(this.ToolTipInfo());
		base.TextSetHighlight();
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x0400041D RID: 1053
	public Text gCoinNum;

	// Token: 0x0400041E RID: 1054
	public Image gCoinImg;

	// Token: 0x0400041F RID: 1055
	[SerializeField]
	private float multiMastery = 1f;

	// Token: 0x04000420 RID: 1056
	[SerializeField]
	private float multiMode = 1f;

	// Token: 0x04000421 RID: 1057
	[SerializeField]
	private float basicGC;

	// Token: 0x04000422 RID: 1058
	[SerializeField]
	private float finalGC;

	// Token: 0x04000423 RID: 1059
	[SerializeField]
	private RectTransform rect;
}
