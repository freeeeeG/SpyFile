using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B8 RID: 184
public class UI_Panel_UpgradeLibrary_SingleType_Main : MonoBehaviour
{
	// Token: 0x0600066A RID: 1642 RVA: 0x00024C5C File Offset: 0x00022E5C
	public void InitWithType(int typeID)
	{
		this.typeID = typeID;
		this.textTypeName.text = DataBase.Inst.Data_UpgradeTypes[typeID].Language_TypeName;
		this.panelIcons.InitWithTypeID(typeID);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.panelIcons.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
		this.simpleTooltip.headID = typeID;
		this.simpleTooltip.indexID = typeID;
	}

	// Token: 0x04000553 RID: 1363
	[SerializeField]
	private int typeID;

	// Token: 0x04000554 RID: 1364
	[SerializeField]
	private UI_Panel_UpgradeLibrary_SingleType_Icons panelIcons;

	// Token: 0x04000555 RID: 1365
	[SerializeField]
	private Text textTypeName;

	// Token: 0x04000556 RID: 1366
	[SerializeField]
	private UI_Text_SimpleTooltip simpleTooltip;
}
