using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class UI_Panel_Main_Ability : UI_Panel_Main_IconList
{
	// Token: 0x060006A8 RID: 1704 RVA: 0x00020AD0 File Offset: 0x0001ECD0
	protected override int IconNum()
	{
		return 16;
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00020AD4 File Offset: 0x0001ECD4
	protected override bool IfAvailable(int ID)
	{
		return ID != 0 && ID != 5 && ID != 12;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00025C66 File Offset: 0x00023E66
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_AbilityMain>().Init(ID);
	}
}
