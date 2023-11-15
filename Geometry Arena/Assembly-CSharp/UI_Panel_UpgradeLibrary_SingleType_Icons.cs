using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class UI_Panel_UpgradeLibrary_SingleType_Icons : UI_Panel_Main_IconList
{
	// Token: 0x06000664 RID: 1636 RVA: 0x00024BA6 File Offset: 0x00022DA6
	public void InitWithTypeID(int typeID)
	{
		this.typeID = typeID;
		this.InitIcons(null);
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00024BB6 File Offset: 0x00022DB6
	private List<Upgrade> GetListUpgrade_InRankOrder_ThisType()
	{
		if (this.listUpgrade_InRankOrder_ThisType != null)
		{
			return this.listUpgrade_InRankOrder_ThisType;
		}
		this.listUpgrade_InRankOrder_ThisType = DataSelector.GetUpgradesWithType_InRankOrder(this.typeID);
		return this.listUpgrade_InRankOrder_ThisType;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00024BDE File Offset: 0x00022DDE
	protected override int IconNum()
	{
		return this.GetListUpgrade_InRankOrder_ThisType().Count;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00024BEB File Offset: 0x00022DEB
	protected override bool IfAvailable(int ID)
	{
		return this.GetListUpgrade_InRankOrder_ThisType()[ID].ifAvailable;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00024C00 File Offset: 0x00022E00
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		UI_Icon_Upgrade component = obj.GetComponent<UI_Icon_UpgradeListMain>();
		int id = this.GetListUpgrade_InRankOrder_ThisType()[ID].id;
		component.Init(id);
		obj.GetComponent<RectTransform>().localScale = new Vector2(this.iconScale, this.iconScale);
	}

	// Token: 0x04000551 RID: 1361
	private int typeID = -1;

	// Token: 0x04000552 RID: 1362
	private List<Upgrade> listUpgrade_InRankOrder_ThisType;
}
