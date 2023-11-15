using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class UI_Panel_Battle_DifficultyOptionShow : UI_Panel_Main_IconList
{
	// Token: 0x060005EB RID: 1515 RVA: 0x00021E5A File Offset: 0x0002005A
	protected override int IconNum()
	{
		return DataBase.Inst.Data_DifficultyOptions.Length;
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x00021E68 File Offset: 0x00020068
	protected override bool IfAvailable(int ID)
	{
		return TempData.inst.diffiOptFlag[ID];
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x00021E76 File Offset: 0x00020076
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_BattleDifficultyOptionList>().Init(ID);
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x00021E84 File Offset: 0x00020084
	public int GetInt_Row()
	{
		return Mathf.CeilToInt((float)this.listIcons.Count / (float)this.columnNum);
	}
}
