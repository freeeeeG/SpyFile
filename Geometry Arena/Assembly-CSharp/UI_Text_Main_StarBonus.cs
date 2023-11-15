using System;
using UnityEngine.EventSystems;

// Token: 0x0200009E RID: 158
public class UI_Text_Main_StarBonus : UI_Icon
{
	// Token: 0x06000578 RID: 1400 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0001FA5A File Offset: 0x0001DC5A
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithString(UI_ToolTipInfo.GetInfo_Main_StarBonus());
		base.TextSetHighlight();
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}
}
