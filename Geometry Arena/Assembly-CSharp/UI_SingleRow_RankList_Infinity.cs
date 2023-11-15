using System;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000B5 RID: 181
public class UI_SingleRow_RankList_Infinity : UI_Icon
{
	// Token: 0x0600065A RID: 1626 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerEnter(PointerEventData eventData)
	{
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00024670 File Offset: 0x00022870
	public void InitWithDatarow(int order, DataRow dataRow)
	{
		order++;
		DataBase inst = DataBase.Inst;
		this.texts[0].text = order.ToString();
		this.texts[1].text = MyTool.GetVersion(dataRow.GetInt(1));
		this.texts[2].text = dataRow.GetString(4);
		this.texts[3].text = inst.DataPlayerModels[dataRow.GetInt(6)].Language_JobName;
		this.texts[4].text = inst.Data_VarColors[dataRow.GetInt(7)].Language_Name;
		this.texts[5].text = dataRow.GetInt(8).ToString();
		this.texts[6].text = dataRow.GetString(9);
		this.texts[7].text = MyTool.DecimalToUnsignedPercentString((double)dataRow.GetFloat(10));
		int num = dataRow.GetInt(11);
		if (dataRow.GetInt(1) <= 807)
		{
			num = GameData.Convert_OldDLtoNewDL(num);
		}
		if (num >= GameParameters.Inst.difficultyLevel_FactorBattle.Length)
		{
			this.texts[8].text = "?";
		}
		else if (num < 0)
		{
			this.texts[8].text = GameParameters.Inst.difficultyLevel_FactorBattle[0].name;
		}
		else
		{
			this.texts[8].text = GameParameters.Inst.difficultyLevel_FactorBattle[num].name;
		}
		this.texts[9].text = dataRow.GetString(19);
		this.texts[10].text = dataRow.GetString(12);
		this.texts[11].text = dataRow.GetString(20);
		UI_Setting inst2 = UI_Setting.Inst;
		Color color = Color.white;
		if (order >= 1 && order <= 3)
		{
			color = inst2.rankColors[4 - order];
		}
		Color color2 = color;
		if (dataRow.GetString(3) == "76561198098878697")
		{
			color2 = inst2.rankColors[3];
		}
		else if (dataRow.GetString(2) == UI_Panel_RankList.inst.GetUniqueCode())
		{
			color2 = UI_Setting.Inst.rankList.colorMyRank;
		}
		for (int i = 0; i <= 11; i++)
		{
			if (i == 2)
			{
				this.texts[i].text = this.texts[i].text.Colored(color2);
			}
			else
			{
				this.texts[i].text = this.texts[i].text.Colored(color);
			}
		}
	}

	// Token: 0x0400054F RID: 1359
	[SerializeField]
	private Text[] texts = new Text[0];
}
