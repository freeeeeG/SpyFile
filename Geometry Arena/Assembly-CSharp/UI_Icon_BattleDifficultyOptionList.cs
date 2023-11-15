using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000086 RID: 134
public class UI_Icon_BattleDifficultyOptionList : UI_Icon
{
	// Token: 0x060004C0 RID: 1216 RVA: 0x0001CD39 File Offset: 0x0001AF39
	public void Init(int i)
	{
		this.doID = i;
		this.UpdateText();
		base.TextSetNormal();
		this.icon_DO.Init(i);
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0001CD5C File Offset: 0x0001AF5C
	public void UpdateText()
	{
		DifficultyOption difficultyOption = DataBase.Inst.Data_DifficultyOptions[this.doID];
		this.textName.text = difficultyOption.Language_Name;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0001CD8C File Offset: 0x0001AF8C
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithString(UI_ToolTipInfo.GetInfo_DifficultyOptions(this.doID, true));
		base.TextSetHighlight();
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000406 RID: 1030
	public int doID;

	// Token: 0x04000407 RID: 1031
	public Text textName;

	// Token: 0x04000408 RID: 1032
	public UI_Icon_DifficultyOption icon_DO;
}
