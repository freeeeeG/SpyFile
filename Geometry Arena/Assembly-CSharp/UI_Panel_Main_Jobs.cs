using System;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class UI_Panel_Main_Jobs : UI_Panel_Main_IconList
{
	// Token: 0x060006BA RID: 1722 RVA: 0x00025FA8 File Offset: 0x000241A8
	public override void InitIcons(Transform transformParent = null)
	{
		base.InitIcons(transformParent);
		if (this.listIcons.Count >= 10)
		{
			for (int i = 9; i < 12; i++)
			{
				this.listIcons[i].transform.localPosition = new Vector2(this.distX * 3f, this.distY * (float)(i - 9)) + this.startPos;
			}
		}
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0002601C File Offset: 0x0002421C
	protected override int IconNum()
	{
		return DataBase.Inst.DataPlayerModels.Length;
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0002602A File Offset: 0x0002422A
	protected override bool IfAvailable(int ID)
	{
		return DataBase.Inst.DataPlayerModels[ID].ifAvailable;
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0002603D File Offset: 0x0002423D
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_Job>().Init(ID);
	}
}
