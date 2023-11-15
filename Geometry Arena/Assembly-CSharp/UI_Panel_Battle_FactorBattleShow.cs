using System;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class UI_Panel_Battle_FactorBattleShow : UI_Panel_Main_IconList
{
	// Token: 0x060005F0 RID: 1520 RVA: 0x00021E9F File Offset: 0x0002009F
	protected override int IconNum()
	{
		return 13;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x00021EA3 File Offset: 0x000200A3
	protected override bool IfAvailable(int ID)
	{
		return ID != 2 && ID != 4;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x00021EB0 File Offset: 0x000200B0
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_FactorBattle>().Init(ID);
	}
}
