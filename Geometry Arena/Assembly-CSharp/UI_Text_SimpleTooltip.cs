using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200009F RID: 159
public class UI_Text_SimpleTooltip : UI_Icon
{
	// Token: 0x0600057D RID: 1405 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0001FA74 File Offset: 0x0001DC74
	public override void OnPointerEnter(PointerEventData eventData)
	{
		LanguageText inst = LanguageText.Inst;
		DataBase inst2 = DataBase.Inst;
		UI_Setting inst3 = UI_Setting.Inst;
		base.TextSetHighlight();
		if (this.headID < 0 && this.indexID < 0)
		{
			return;
		}
		string text = "";
		if (this.headID >= 0)
		{
			switch (this.type)
			{
			case UI_Text_SimpleTooltip.EnumTipType.NORMAL:
				text += inst.toolTip_TipStringsHead[this.headID].TextSet(inst3.commonSets.blueSmallTile);
				break;
			case UI_Text_SimpleTooltip.EnumTipType.UPGRADETYPE:
				text += inst2.Data_UpgradeTypes[this.headID].Language_TypeName.TextSet(inst3.commonSets.blueSmallTile);
				break;
			case UI_Text_SimpleTooltip.EnumTipType.SHAPE:
				text += inst.shapes[this.headID].shapeName.TextSet(inst3.commonSets.blueSmallTile);
				break;
			}
			text += "\n";
		}
		switch (this.type)
		{
		case UI_Text_SimpleTooltip.EnumTipType.NORMAL:
			text += inst.toolTip_TipStrings[this.indexID];
			break;
		case UI_Text_SimpleTooltip.EnumTipType.UPGRADETYPE:
			text += inst2.Data_UpgradeTypes[this.headID].Language_TypeInfo;
			break;
		case UI_Text_SimpleTooltip.EnumTipType.SHAPE:
			text += inst.shapes[this.headID].shapeInfo;
			break;
		case UI_Text_SimpleTooltip.EnumTipType.SHAPETHEME:
			text += inst.shapeThemes[(int)Battle.inst.CurrentLevelType];
			break;
		}
		UI_ToolTip.inst.ShowWithString(text);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0001FBF8 File Offset: 0x0001DDF8
	public void InitAsUpgradeType(int ID)
	{
		if (ID < 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		UpgradeType upgradeType = DataBase.Inst.Data_UpgradeTypes[ID];
		base.gameObject.SetActive(true);
		this.indexID = ID;
		this.headID = ID;
		this.textHead.text = upgradeType.Language_TypeName;
		this.textNormalColor = upgradeType.typeColor;
		this.textHighlightColor = upgradeType.typeColor;
		this.textHead.color = upgradeType.typeColor;
		this.type = UI_Text_SimpleTooltip.EnumTipType.UPGRADETYPE;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000470 RID: 1136
	public UI_Text_SimpleTooltip.EnumTipType type;

	// Token: 0x04000471 RID: 1137
	[SerializeField]
	private Text textHead;

	// Token: 0x04000472 RID: 1138
	[SerializeField]
	public int headID = -1;

	// Token: 0x04000473 RID: 1139
	[SerializeField]
	public int indexID;

	// Token: 0x02000156 RID: 342
	public enum EnumTipType
	{
		// Token: 0x040009E1 RID: 2529
		NORMAL,
		// Token: 0x040009E2 RID: 2530
		UPGRADETYPE,
		// Token: 0x040009E3 RID: 2531
		SHAPE,
		// Token: 0x040009E4 RID: 2532
		SHAPETHEME
	}
}
