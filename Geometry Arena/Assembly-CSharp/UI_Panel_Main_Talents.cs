using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class UI_Panel_Main_Talents : UI_Panel_Main_IconList
{
	// Token: 0x060006ED RID: 1773 RVA: 0x00026B54 File Offset: 0x00024D54
	protected override int IconNum()
	{
		this.jobId = TempData.inst.jobId;
		return DataBase.Inst.DataPlayerModels[this.jobId].talents.Length;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool IfAvailable(int ID)
	{
		return true;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00026B7E File Offset: 0x00024D7E
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_Talent>().Init(this.jobId, ID);
	}

	// Token: 0x040005C0 RID: 1472
	private int jobId;
}
