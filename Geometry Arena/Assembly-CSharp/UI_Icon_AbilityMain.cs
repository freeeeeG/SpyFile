using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000082 RID: 130
public class UI_Icon_AbilityMain : UI_Icon
{
	// Token: 0x0600049B RID: 1179 RVA: 0x0001C600 File Offset: 0x0001A800
	public void Init(int i)
	{
		this.abilityId = i;
		Factor totalFactor = TempData.inst.playerPreview.TotalFactor;
		LanguageText inst = LanguageText.Inst;
		this.textName.text = inst.factor[i];
		this.textNum.text = totalFactor.GetActualFactorInfo(this.abilityId);
		base.TextSetNormal();
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0001C65A File Offset: 0x0001A85A
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithAbilityMain(this.abilityId);
		base.TextSetHighlight();
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x040003EA RID: 1002
	public int abilityId;

	// Token: 0x040003EB RID: 1003
	public Text textName;

	// Token: 0x040003EC RID: 1004
	public Text textNum;
}
