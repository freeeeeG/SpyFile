using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class UI_Panel_Main_VarColors : UI_Panel_Main_IconList
{
	// Token: 0x060006FA RID: 1786 RVA: 0x00026ECB File Offset: 0x000250CB
	protected override int IconNum()
	{
		return DataBase.Inst.Data_VarColors.Length;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00026ED9 File Offset: 0x000250D9
	protected override bool IfAvailable(int ID)
	{
		return DataBase.Inst.Data_VarColors[ID].avaiPlayer;
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00026EEC File Offset: 0x000250EC
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_Color>().Init(ID);
	}
}
